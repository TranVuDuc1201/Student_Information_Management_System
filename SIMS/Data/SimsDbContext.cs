using System;
using Microsoft.EntityFrameworkCore;
using SIMS.Models.Entities;

namespace SIMS.Data
{
    public class SimsDbContext : DbContext
    {

        public SimsDbContext(DbContextOptions<SimsDbContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings =>
                warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));

            base.OnConfiguring(optionsBuilder);
        }

        // Database Tables
        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<Course> Courses { get; set; } = null!;
        public DbSet<Faculty> Faculties { get; set; } = null!;
        public DbSet<Enrollment> Enrollments { get; set; } = null!;
        public DbSet<SemesterGpaHistory> SemesterGpaHistories { get; set; } = null!;
        public DbSet<Notification> Notifications { get; set; } = null!;
        public DbSet<AuditLog> AuditLogs { get; set; } = null!;
        public DbSet<SystemAdmin> SystemAdmins { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Course -> Faculty Relationship (N:1, optional)
            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(c => c.CourseId);

                entity.HasOne(c => c.Faculty)
                      .WithMany(f => f.AssignedCourses)
                      .HasForeignKey(c => c.FacultyId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // 2. Enrollment -> Student & Course Relationships (N:1 each with composite uniqueness)
            modelBuilder.Entity<Enrollment>(entity =>
            {
                entity.HasKey(e => e.EnrollmentId);

                entity.HasIndex(e => new { e.StudentId, e.CourseId })
                      .IsUnique();

                entity.HasOne(e => e.Student)
                      .WithMany(s => s.Enrollments)
                      .HasForeignKey(e => e.StudentId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Course)
                      .WithMany(c => c.Enrollments)
                      .HasForeignKey(e => e.CourseId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // 3. SemesterGpaHistory -> Student Relationship (N:1)
            modelBuilder.Entity<SemesterGpaHistory>(entity =>
            {
                entity.HasKey(g => g.Id);

                entity.HasOne(g => g.Student)
                      .WithMany(s => s.GpaHistories)
                      .HasForeignKey(g => g.StudentId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // 4. Notification -> Student Relationship (N:1)
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(n => n.Id);

                entity.HasOne(n => n.Student)
                      .WithMany(s => s.Notifications)
                      .HasForeignKey(n => n.StudentId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // 5. AuditLog Configuration
            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.HasKey(a => a.Id);
            });

            // 6. SystemAdmin Configuration
            modelBuilder.Entity<SystemAdmin>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.HasIndex(a => a.Email).IsUnique();
            });

            // Seed Initial Data
            SeedData(modelBuilder);
        }

        private static string HashPassword(string password)
        {
            // Cố định Salt (chuỗi 22 ký tự hợp lệ của BCrypt) để Hash sinh ra luôn tĩnh
            // Giải quyết triệt để lỗi PendingModelChangesWarning của Entity Framework
            string fixedSalt = "$2a$11$ABCDEFGHIJKLMNOPQRSTUV";
            return BCrypt.Net.BCrypt.HashPassword(password, fixedSalt);
        }

        private static void SeedData(ModelBuilder modelBuilder)
        {
            // Deterministic Primary Keys for Seed Data
            var facultyId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var course1Id = Guid.Parse("22222222-2222-2222-2222-222222222221");
            var course2Id = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var studentId = Guid.Parse("33333333-3333-3333-3333-333333333333");
            var adminId = Guid.Parse("99999999-9999-9999-9999-999999999999");

            var gpa1Id = Guid.Parse("44444444-4444-4444-4444-444444444441");
            var gpa2Id = Guid.Parse("44444444-4444-4444-4444-444444444442");
            var gpa3Id = Guid.Parse("44444444-4444-4444-4444-444444444443");
            var gpa4Id = Guid.Parse("44444444-4444-4444-4444-444444444444");

            var notif1Id = Guid.Parse("55555555-5555-5555-5555-555555555551");
            var notif2Id = Guid.Parse("55555555-5555-5555-5555-555555555552");

            var enroll1Id = Guid.Parse("66666666-6666-6666-6666-666666666661");
            var enroll2Id = Guid.Parse("66666666-6666-6666-6666-666666666662");

            var auditLogId = Guid.Parse("77777777-7777-7777-7777-777777777777");

            // Seed Passwords (fixed pre-hashed or hashed via helper)
            var defaultUserPasswordHash = HashPassword("password123");
            var adminPasswordHash = HashPassword("duc123");

            // 1. Seed System Admin (adminduc000@gmail.com / duc123)
            modelBuilder.Entity<SystemAdmin>().HasData(
                new SystemAdmin
                {
                    Id = adminId,
                    Email = "adminduc000@gmail.com",
                    PasswordHash = adminPasswordHash,
                    FullName = "Duc Tran (System Admin)"
                }
            );

            // 2. Seed Faculty (Dr. Michael Vance / password123)
            modelBuilder.Entity<Faculty>().HasData(
                new Faculty
                {
                    FacultyId = facultyId,
                    FacultyName = "Dr. Michael Vance",
                    Email = "vancem@fpt.edu.vn",
                    PasswordHash = defaultUserPasswordHash,
                    Department = "Department of Software Engineering & Architecture"
                }
            );

            // 3. Seed Courses
            modelBuilder.Entity<Course>().HasData(
                new Course
                {
                    CourseId = course1Id,
                    CourseCode = "APDP301",
                    CourseName = "Applied Programming & Design Principles",
                    Credits = 4,
                    ScheduleTime = "Mon, Wed (09:00 - 11:30)",
                    Room = "Lab 402 - Tech Wing",
                    FacultyId = facultyId
                },
                new Course
                {
                    CourseId = course2Id,
                    CourseCode = "CS402",
                    CourseName = "Cloud Architecture & Microservices",
                    Credits = 3,
                    ScheduleTime = "Tue, Thu (13:30 - 15:30)",
                    Room = "Auditorium 2B",
                    FacultyId = facultyId
                }
            );

            // 4. Seed Student (Tran Vu Duc / password123)
            modelBuilder.Entity<Student>().HasData(
                new Student
                {
                    Id = studentId,
                    StudentCode = "BH02383",
                    FullName = "Tran Vu Duc",
                    Email = "ductvbh02383@fpt.edu.vn",
                    PasswordHash = defaultUserPasswordHash,
                    Program = "Software Engineering",
                    EnrollmentDate = new DateTime(2023, 9, 1, 0, 0, 0, DateTimeKind.Utc),
                    Status = "Active",
                    Gpa = 3.82
                }
            );

            // 5. Seed Semester GPA Histories
            modelBuilder.Entity<SemesterGpaHistory>().HasData(
                new SemesterGpaHistory
                {
                    Id = gpa1Id,
                    StudentId = studentId,
                    Semester = "Sem 1",
                    Gpa = 3.65
                },
                new SemesterGpaHistory
                {
                    Id = gpa2Id,
                    StudentId = studentId,
                    Semester = "Sem 2",
                    Gpa = 3.72
                },
                new SemesterGpaHistory
                {
                    Id = gpa3Id,
                    StudentId = studentId,
                    Semester = "Sem 3",
                    Gpa = 3.80
                },
                new SemesterGpaHistory
                {
                    Id = gpa4Id,
                    StudentId = studentId,
                    Semester = "Sem 4",
                    Gpa = 3.82
                }
            );

            // 6. Seed Notifications
            modelBuilder.Entity<Notification>().HasData(
                new Notification
                {
                    Id = notif1Id,
                    StudentId = studentId,
                    Title = "Midterm Examination Scheduled",
                    Message = "APDP301 design presentation is confirmed for next Monday at 09:00 AM.",
                    Category = "Exam",
                    IsUnread = true,
                    Timestamp = new DateTime(2026, 11, 12, 8, 0, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = notif2Id,
                    StudentId = studentId,
                    Title = "Course Schedule Published",
                    Message = "Your enrolled semester courses are now active in SIMS.",
                    Category = "Notice",
                    IsUnread = false,
                    Timestamp = new DateTime(2026, 11, 10, 10, 0, 0, DateTimeKind.Utc)
                }
            );

            // 7. Seed Enrollments
            modelBuilder.Entity<Enrollment>().HasData(
                new Enrollment
                {
                    EnrollmentId = enroll1Id,
                    StudentId = studentId,
                    CourseId = course1Id,
                    AttendanceScore = 10.0,
                    MidtermScore = 28.5,
                    AssignmentScore = 19.0,
                    FinalScore = 38.0,
                    TotalScore = 95.5,
                    LetterGrade = "A+",
                    Status = "Submitted",
                    Note = "Outstanding architectural documentation."
                },
                new Enrollment
                {
                    EnrollmentId = enroll2Id,
                    StudentId = studentId,
                    CourseId = course2Id,
                    AttendanceScore = 9.5,
                    MidtermScore = 25.0,
                    AssignmentScore = 18.0,
                    FinalScore = 34.0,
                    TotalScore = 86.5,
                    LetterGrade = "A-",
                    Status = "Submitted",
                    Note = "Solid cloud architecture patterns."
                }
            );

            // 8. Seed Audit Log
            modelBuilder.Entity<AuditLog>().HasData(
                new AuditLog
                {
                    Id = auditLogId,
                    Action = "Database Synchronized",
                    PerformedBy = "System Administrator",
                    Role = "Admin",
                    TargetEntity = "Database: SIMSDatabase",
                    Timestamp = new DateTime(2026, 11, 13, 0, 0, 0, DateTimeKind.Utc),
                    Severity = "Info"
                }
            );
        }
    }
}
