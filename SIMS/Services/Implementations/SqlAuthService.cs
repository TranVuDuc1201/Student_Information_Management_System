using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SIMS.Data;
using SIMS.Models.Entities;
using SIMS.Models.ViewModels;
using SIMS.Services.Interfaces;

namespace SIMS.Services.Implementations
{
    public class SqlAuthService : IAuthService
    {
        private readonly SimsDbContext _context;

        public SqlAuthService(SimsDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<(bool Success, string Message, string RedirectAction, string RedirectController, string Role, string FullName, string Identifier)> AuthenticateAsync(LoginViewModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password))
            {
                return (false, "Please provide username/email and password.", "Login", "Auth", string.Empty, string.Empty, string.Empty);
            }

            var identifier = model.Username.Trim().ToLowerInvariant();

            if (model.Role == UserRoleType.Academic)
            {
                // 1. Check Students Table first (by StudentCode or Email)
                var student = await _context.Students.AsNoTracking()
                    .FirstOrDefaultAsync(s => s.StudentCode.ToLower() == identifier || s.Email.ToLower() == identifier);

                if (student != null)
                {
                    bool isPasswordValid = false;
                    try
                    {
                        isPasswordValid = BCrypt.Net.BCrypt.Verify(model.Password, student.PasswordHash);
                    }
                    catch
                    {
                        isPasswordValid = false;
                    }

                    if (isPasswordValid)
                    {
                        return (true, $"Welcome back, {student.FullName}!", "Index", "Student", "Student", student.FullName, student.StudentCode);
                    }

                    return (false, "Incorrect password for student account.", "Login", "Auth", string.Empty, string.Empty, string.Empty);
                }

                // 2. Check Faculties Table if not found in Students (by Email or FacultyName)
                var faculty = await _context.Faculties.AsNoTracking()
                    .FirstOrDefaultAsync(f => f.Email.ToLower() == identifier || f.FacultyName.ToLower() == identifier);

                if (faculty != null)
                {
                    bool isPasswordValid = false;
                    try
                    {
                        isPasswordValid = BCrypt.Net.BCrypt.Verify(model.Password, faculty.PasswordHash);
                    }
                    catch
                    {
                        isPasswordValid = false;
                    }

                    if (isPasswordValid)
                    {
                        return (true, $"Welcome back, Professor {faculty.FacultyName}!", "Index", "Faculty", "Faculty", faculty.FacultyName, faculty.Email);
                    }

                    return (false, "Incorrect password for faculty account.", "Login", "Auth", string.Empty, string.Empty, string.Empty);
                }

                return (false, "No Academic account (Student or Faculty) found matching the provided credentials.", "Login", "Auth", string.Empty, string.Empty, string.Empty);
            }
            else // UserRoleType.SystemAdmin
            {
                // Check SystemAdmins Table (by Email)
                var admin = await _context.SystemAdmins.AsNoTracking()
                    .FirstOrDefaultAsync(a => a.Email.ToLower() == identifier);

                if (admin != null)
                {
                    bool isPasswordValid = false;
                    try
                    {
                        isPasswordValid = BCrypt.Net.BCrypt.Verify(model.Password, admin.PasswordHash);
                    }
                    catch
                    {
                        isPasswordValid = false;
                    }

                    if (isPasswordValid)
                    {
                        return (true, $"Welcome Administrator, {admin.FullName}!", "Index", "Admin", "Admin", admin.FullName, admin.Email);
                    }

                    return (false, "Incorrect password for administrator account.", "Login", "Auth", string.Empty, string.Empty, string.Empty);
                }

                return (false, "No System Administrator account found with this email.", "Login", "Auth", string.Empty, string.Empty, string.Empty);
            }
        }

        public async Task<(bool Success, string Message)> RegisterStudentAsync(RegisterViewModel model)
        {
            if (model == null)
            {
                return (false, "Invalid registration details.");
            }

            var studentCode = model.StudentCode.Trim().ToUpperInvariant();
            var email = model.Email.Trim().ToLowerInvariant();

            // Verify uniqueness
            var exists = await _context.Students.AnyAsync(s => s.StudentCode == studentCode || s.Email.ToLower() == email);
            if (exists)
            {
                return (false, "A student account with this Student ID or Email already exists in the system.");
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password, 11);

            var student = new Student
            {
                Id = Guid.NewGuid(),
                StudentCode = studentCode,
                FullName = model.FullName.Trim(),
                Email = model.Email.Trim(),
                PasswordHash = hashedPassword,
                Program = string.IsNullOrWhiteSpace(model.Program) ? "Software Engineering" : model.Program.Trim(),
                EnrollmentDate = DateTime.UtcNow,
                Status = "Active",
                Gpa = 3.50
            };

            await _context.Students.AddAsync(student);

            // Create notification welcoming new student
            var welcomeNotif = new Notification
            {
                Id = Guid.NewGuid(),
                StudentId = student.Id,
                Title = "Welcome to SIMS Portal",
                Message = $"Welcome {student.FullName}! Your student profile has been created successfully.",
                Category = "Notice",
                IsUnread = true,
                Timestamp = DateTime.UtcNow
            };
            await _context.Notifications.AddAsync(welcomeNotif);

            // Audit Log
            var audit = new AuditLog
            {
                Id = Guid.NewGuid(),
                Action = "Student Self-Registered",
                PerformedBy = student.StudentCode,
                Role = "Student",
                TargetEntity = $"Student: {student.StudentCode} ({student.FullName})",
                Timestamp = DateTime.UtcNow,
                Severity = "Info"
            };
            await _context.AuditLogs.AddAsync(audit);

            await _context.SaveChangesAsync();
            return (true, "Registration successful! You may now sign in with your credentials.");
        }
    }
}
