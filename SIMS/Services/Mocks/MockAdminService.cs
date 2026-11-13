using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SIMS.Models.ViewModels;
using SIMS.Services.Interfaces;

namespace SIMS.Services.Mocks
{
    public class MockAdminService : IAdminService
    {
        private static readonly List<StudentRecordItemViewModel> _students = new()
        {
            new StudentRecordItemViewModel
            {
                Id = Guid.NewGuid(),
                StudentCode = "BH02383",
                FullName = "Tran Vu Duc",
                Email = "ductvbh02383@fpt.edu.vn",
                Program = "Software Engineering",
                EnrollmentDate = DateTime.UtcNow.AddYears(-3),
                Status = "Active",
                Gpa = 3.82
            },
            new StudentRecordItemViewModel
            {
                Id = Guid.NewGuid(),
                StudentCode = "BH02190",
                FullName = "Nguyen Thi Mai",
                Email = "maintbh02190@fpt.edu.vn",
                Program = "Artificial Intelligence",
                EnrollmentDate = DateTime.UtcNow.AddYears(-2),
                Status = "Active",
                Gpa = 3.68
            },
            new StudentRecordItemViewModel
            {
                Id = Guid.NewGuid(),
                StudentCode = "BH02411",
                FullName = "Le Hoang Nam",
                Email = "namlhbh02411@fpt.edu.vn",
                Program = "Cybersecurity",
                EnrollmentDate = DateTime.UtcNow.AddYears(-3),
                Status = "Active",
                Gpa = 3.45
            },
            new StudentRecordItemViewModel
            {
                Id = Guid.NewGuid(),
                StudentCode = "BH02505",
                FullName = "Pham Quoc Bao",
                Email = "baopqbh02505@fpt.edu.vn",
                Program = "Software Engineering",
                EnrollmentDate = DateTime.UtcNow.AddYears(-1),
                Status = "Active",
                Gpa = 3.91
            },
            new StudentRecordItemViewModel
            {
                Id = Guid.NewGuid(),
                StudentCode = "BH01992",
                FullName = "Vu Thi Huong",
                Email = "huongvtbh01992@fpt.edu.vn",
                Program = "Information Systems",
                EnrollmentDate = DateTime.UtcNow.AddYears(-4),
                Status = "Graduated",
                Gpa = 3.75
            },
            new StudentRecordItemViewModel
            {
                Id = Guid.NewGuid(),
                StudentCode = "BH02730",
                FullName = "Hoang Minh Khang",
                Email = "khanghmbh02730@fpt.edu.vn",
                Program = "Artificial Intelligence",
                EnrollmentDate = DateTime.UtcNow.AddMonths(-6),
                Status = "Suspended",
                Gpa = 2.10
            }
        };

        private static readonly List<SystemAuditLogViewModel> _auditLogs = new()
        {
            new SystemAuditLogViewModel
            {
                Id = Guid.NewGuid(),
                Action = "Course Grading Finalized",
                PerformedBy = "Dr. Michael Vance",
                Role = "Faculty",
                TargetEntity = "Course: APDP301",
                Timestamp = DateTime.UtcNow.AddMinutes(-12),
                TimeAgo = "12 mins ago",
                Severity = "Info"
            },
            new SystemAuditLogViewModel
            {
                Id = Guid.NewGuid(),
                Action = "New Student Registered",
                PerformedBy = "System Administrator",
                Role = "Admin",
                TargetEntity = "Student: BH02730",
                Timestamp = DateTime.UtcNow.AddHours(-1),
                TimeAgo = "1 hour ago",
                Severity = "Info"
            },
            new SystemAuditLogViewModel
            {
                Id = Guid.NewGuid(),
                Action = "Failed Authentication Attempt",
                PerformedBy = "192.168.1.145",
                Role = "Unknown",
                TargetEntity = "Auth Endpoint",
                Timestamp = DateTime.UtcNow.AddHours(-3),
                TimeAgo = "3 hours ago",
                Severity = "Warning"
            },
            new SystemAuditLogViewModel
            {
                Id = Guid.NewGuid(),
                Action = "Database Backup Snapshot",
                PerformedBy = "Scheduled Cron Daemon",
                Role = "System",
                TargetEntity = "MySQL SIMS_DB",
                Timestamp = DateTime.UtcNow.AddHours(-8),
                TimeAgo = "8 hours ago",
                Severity = "Info"
            }
        };

        public Task<AdminDashboardViewModel> GetDashboardDataAsync(string? searchQuery = null, string? programFilter = null, string? statusFilter = null)
        {
            var query = _students.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                var q = searchQuery.Trim().ToLowerInvariant();
                query = query.Where(s => s.FullName.ToLowerInvariant().Contains(q) || s.StudentCode.ToLowerInvariant().Contains(q) || s.Email.ToLowerInvariant().Contains(q));
            }

            if (!string.IsNullOrWhiteSpace(programFilter) && programFilter != "All")
            {
                query = query.Where(s => s.Program.Equals(programFilter, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(statusFilter) && statusFilter != "All")
            {
                query = query.Where(s => s.Status.Equals(statusFilter, StringComparison.OrdinalIgnoreCase));
            }

            var vm = new AdminDashboardViewModel
            {
                AdminName = "System Administrator",
                TotalStudents = _students.Count,
                TotalFaculty = 24,
                ActiveCourses = 58,
                AverageSystemGpa = Math.Round(_students.Average(s => s.Gpa), 2),
                SystemAlertsCount = 1,
                SearchQuery = searchQuery ?? string.Empty,
                SelectedProgramFilter = programFilter ?? "All",
                SelectedStatusFilter = statusFilter ?? "All",
                Students = query.ToList(),
                AuditLogs = _auditLogs.OrderByDescending(l => l.Timestamp).ToList(),
                NewStudent = new CreateStudentInputModel()
            };

            return Task.FromResult(vm);
        }

        public Task<bool> CreateStudentAsync(CreateStudentInputModel input)
        {
            if (string.IsNullOrWhiteSpace(input.FullName) || string.IsNullOrWhiteSpace(input.StudentCode))
            {
                return Task.FromResult(false);
            }

            _students.Insert(0, new StudentRecordItemViewModel
            {
                Id = Guid.NewGuid(),
                StudentCode = input.StudentCode.Trim().ToUpperInvariant(),
                FullName = input.FullName.Trim(),
                Email = string.IsNullOrWhiteSpace(input.Email) ? $"{input.StudentCode.ToLowerInvariant()}@fpt.edu.vn" : input.Email.Trim(),
                Program = input.Program,
                EnrollmentDate = input.EnrollmentDate,
                Status = "Active",
                Gpa = 3.50
            });

            _auditLogs.Insert(0, new SystemAuditLogViewModel
            {
                Id = Guid.NewGuid(),
                Action = "New Student Created",
                PerformedBy = "System Administrator",
                Role = "Admin",
                TargetEntity = $"Student: {input.StudentCode}",
                Timestamp = DateTime.UtcNow,
                TimeAgo = "Just now",
                Severity = "Info"
            });

            return Task.FromResult(true);
        }

        public Task<bool> CreateFacultyAsync(CreateFacultyInputModel input)
        {
            if (string.IsNullOrWhiteSpace(input.FacultyName) || string.IsNullOrWhiteSpace(input.Email))
            {
                return Task.FromResult(false);
            }

            _auditLogs.Insert(0, new SystemAuditLogViewModel
            {
                Id = Guid.NewGuid(),
                Action = "New Faculty Created",
                PerformedBy = "System Administrator",
                Role = "Admin",
                TargetEntity = $"Faculty: {input.FacultyName} ({input.Email})",
                Timestamp = DateTime.UtcNow,
                TimeAgo = "Just now",
                Severity = "Info"
            });

            return Task.FromResult(true);
        }

        public Task<bool> DeleteStudentAsync(Guid id)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);
            if (student != null)
            {
                _students.Remove(student);
                _auditLogs.Insert(0, new SystemAuditLogViewModel
                {
                    Id = Guid.NewGuid(),
                    Action = "Student Record Archived/Deleted",
                    PerformedBy = "System Administrator",
                    Role = "Admin",
                    TargetEntity = $"Student: {student.StudentCode} ({student.FullName})",
                    Timestamp = DateTime.UtcNow,
                    TimeAgo = "Just now",
                    Severity = "Warning"
                });
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
    }
}

