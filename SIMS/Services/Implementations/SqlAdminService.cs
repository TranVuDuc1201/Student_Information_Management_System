using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SIMS.Data;
using SIMS.Models.Entities;
using SIMS.Models.ViewModels;
using SIMS.Services.Interfaces;

namespace SIMS.Services.Implementations
{
    public class SqlAdminService : IAdminService
    {
        private readonly SimsDbContext _context;

        public SqlAdminService(SimsDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<AdminDashboardViewModel> GetDashboardDataAsync(string? searchQuery = null, string? programFilter = null, string? statusFilter = null)
        {
            var query = _context.Students.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                var q = searchQuery.Trim().ToLower();
                query = query.Where(s => s.FullName.ToLower().Contains(q) ||
                                         s.StudentCode.ToLower().Contains(q) ||
                                         s.Email.ToLower().Contains(q));
            }

            if (!string.IsNullOrWhiteSpace(programFilter) && programFilter != "All")
            {
                query = query.Where(s => s.Program == programFilter);
            }

            if (!string.IsNullOrWhiteSpace(statusFilter) && statusFilter != "All")
            {
                query = query.Where(s => s.Status == statusFilter);
            }

            var studentsList = await query
                .OrderBy(s => s.StudentCode)
                .Select(s => new StudentRecordItemViewModel
                {
                    Id = s.Id,
                    StudentCode = s.StudentCode,
                    FullName = s.FullName,
                    Email = s.Email,
                    Program = s.Program,
                    EnrollmentDate = s.EnrollmentDate,
                    Status = s.Status,
                    Gpa = s.Gpa
                })
                .ToListAsync();

            var totalStudents = await _context.Students.CountAsync();
            var totalFaculty = await _context.Faculties.CountAsync();
            var activeCourses = await _context.Courses.CountAsync();
            var averageGpa = totalStudents > 0
                ? Math.Round(await _context.Students.AverageAsync(s => s.Gpa), 2)
                : 0.0;

            var auditLogs = await _context.AuditLogs.AsNoTracking()
                .OrderByDescending(a => a.Timestamp)
                .Take(10)
                .Select(a => new SystemAuditLogViewModel
                {
                    Id = a.Id,
                    Action = a.Action,
                    PerformedBy = a.PerformedBy,
                    Role = a.Role,
                    TargetEntity = a.TargetEntity,
                    Timestamp = a.Timestamp,
                    TimeAgo = "Recent",
                    Severity = a.Severity
                })
                .ToListAsync();

            var viewModel = new AdminDashboardViewModel
            {
                AdminName = "Duc Tran (System Admin)",
                TotalStudents = totalStudents,
                TotalFaculty = totalFaculty,
                ActiveCourses = activeCourses,
                AverageSystemGpa = averageGpa,
                SystemAlertsCount = 1,
                SearchQuery = searchQuery ?? string.Empty,
                SelectedProgramFilter = programFilter ?? "All",
                SelectedStatusFilter = statusFilter ?? "All",
                Students = studentsList,
                AuditLogs = auditLogs,
                NewStudent = new CreateStudentInputModel(),
                NewFaculty = new CreateFacultyInputModel()
            };

            return viewModel;
        }

        public async Task<bool> CreateStudentAsync(CreateStudentInputModel input)
        {
            if (input == null || string.IsNullOrWhiteSpace(input.FullName) || string.IsNullOrWhiteSpace(input.StudentCode))
            {
                return false;
            }

            var rawPassword = string.IsNullOrWhiteSpace(input.Password) ? "password123" : input.Password;
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(rawPassword, 11);

            var student = new Student
            {
                Id = Guid.NewGuid(),
                StudentCode = input.StudentCode.Trim().ToUpperInvariant(),
                FullName = input.FullName.Trim(),
                Email = string.IsNullOrWhiteSpace(input.Email)
                    ? $"{input.StudentCode.Trim().ToLowerInvariant()}@fpt.edu.vn"
                    : input.Email.Trim(),
                PasswordHash = hashedPassword,
                Program = string.IsNullOrWhiteSpace(input.Program) ? "Software Engineering" : input.Program.Trim(),
                EnrollmentDate = input.EnrollmentDate,
                Status = "Active",
                Gpa = 3.50
            };

            await _context.Students.AddAsync(student);

            var audit = new AuditLog
            {
                Id = Guid.NewGuid(),
                Action = "New Student Created",
                PerformedBy = "System Administrator",
                Role = "Admin",
                TargetEntity = $"Student: {student.StudentCode} ({student.FullName})",
                Timestamp = DateTime.UtcNow,
                Severity = "Info"
            };
            await _context.AuditLogs.AddAsync(audit);

            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> CreateFacultyAsync(CreateFacultyInputModel input)
        {
            if (input == null || string.IsNullOrWhiteSpace(input.FacultyName) || string.IsNullOrWhiteSpace(input.Email))
            {
                return false;
            }

            var email = input.Email.Trim().ToLowerInvariant();
            var exists = await _context.Faculties.AnyAsync(f => f.Email.ToLower() == email);
            if (exists)
            {
                return false;
            }

            var rawPassword = string.IsNullOrWhiteSpace(input.Password) ? "faculty123" : input.Password;
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(rawPassword, 11);

            var faculty = new Faculty
            {
                FacultyId = Guid.NewGuid(),
                FacultyName = input.FacultyName.Trim(),
                Email = input.Email.Trim(),
                PasswordHash = hashedPassword,
                Department = string.IsNullOrWhiteSpace(input.Department)
                    ? "Department of Software Engineering & Architecture"
                    : input.Department.Trim()
            };

            await _context.Faculties.AddAsync(faculty);

            var audit = new AuditLog
            {
                Id = Guid.NewGuid(),
                Action = "New Faculty Created",
                PerformedBy = "System Administrator",
                Role = "Admin",
                TargetEntity = $"Faculty: {faculty.FacultyName} ({faculty.Email})",
                Timestamp = DateTime.UtcNow,
                Severity = "Info"
            };
            await _context.AuditLogs.AddAsync(audit);

            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteStudentAsync(Guid id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return false;
            }

            _context.Students.Remove(student);

            var audit = new AuditLog
            {
                Id = Guid.NewGuid(),
                Action = "Student Record Archived/Deleted",
                PerformedBy = "System Administrator",
                Role = "Admin",
                TargetEntity = $"Student: {student.StudentCode} ({student.FullName})",
                Timestamp = DateTime.UtcNow,
                Severity = "Warning"
            };
            await _context.AuditLogs.AddAsync(audit);

            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}
