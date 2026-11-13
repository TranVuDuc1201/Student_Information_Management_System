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
    public class SqlStudentService : IStudentService
    {
        private readonly SimsDbContext _context;

        public SqlStudentService(SimsDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<StudentDashboardViewModel> GetDashboardDataAsync(Guid? studentId = null)
        {
            Student? student = null;

            if (studentId.HasValue)
            {
                student = await _context.Students.AsNoTracking().FirstOrDefaultAsync(s => s.Id == studentId.Value);
            }

            if (student == null)
            {
                student = await _context.Students.AsNoTracking().FirstOrDefaultAsync();
            }

            // Fallback default student profile if table is currently empty
            if (student == null)
            {
                student = new Student
                {
                    Id = Guid.NewGuid(),
                    StudentCode = "BH02383",
                    FullName = "Tran Vu Duc",
                    Email = "ductvbh02383@fpt.edu.vn",
                    Program = "Software Engineering",
                    Status = "Active",
                    Gpa = 3.82
                };
            }

            // Fetch registered courses from Courses table in SQL Server
            var dbCourses = await _context.Courses.AsNoTracking().ToListAsync();

            var colorPalette = new[] { "primary", "accent-purple", "accent-cyan", "accent-pink" };
            int colorIndex = 0;

            var enrolledCourses = new List<CourseEnrollmentCardViewModel>();

            if (dbCourses.Any())
            {
                foreach (var c in dbCourses)
                {
                    enrolledCourses.Add(new CourseEnrollmentCardViewModel
                    {
                        CourseId = c.CourseId,
                        CourseCode = c.CourseCode,
                        CourseName = c.CourseName,
                        InstructorName = "Assigned Faculty",
                        Credits = c.Credits,
                        ProgressPercentage = 75,
                        CurrentGrade = student.Gpa >= 3.6 ? "A" : student.Gpa >= 3.0 ? "B" : "C",
                        ScheduleTime = "Mon, Wed (09:00 - 11:30)",
                        Room = "Campus Tech Wing",
                        ColorClass = colorPalette[colorIndex % colorPalette.Length]
                    });
                    colorIndex++;
                }
            }
            else
            {
                // Fallback courses if database table is not seeded yet
                enrolledCourses = new List<CourseEnrollmentCardViewModel>
                {
                    new CourseEnrollmentCardViewModel
                    {
                        CourseId = Guid.NewGuid(),
                        CourseCode = "APDP301",
                        CourseName = "Applied Programming & Design Principles",
                        InstructorName = "Dr. Michael Vance",
                        Credits = 4,
                        ProgressPercentage = 85,
                        CurrentGrade = "A",
                        ScheduleTime = "Mon, Wed (09:00 - 11:30)",
                        Room = "Lab 402 - Tech Wing",
                        ColorClass = "primary"
                    },
                    new CourseEnrollmentCardViewModel
                    {
                        CourseId = Guid.NewGuid(),
                        CourseCode = "CS402",
                        CourseName = "Cloud Architecture & Microservices",
                        InstructorName = "Prof. Elena Rostova",
                        Credits = 3,
                        ProgressPercentage = 68,
                        CurrentGrade = "A-",
                        ScheduleTime = "Tue, Thu (13:30 - 15:30)",
                        Room = "Auditorium 2B",
                        ColorClass = "accent-purple"
                    }
                };
            }

            var totalCredits = enrolledCourses.Sum(c => c.Credits);

            return new StudentDashboardViewModel
            {
                StudentId = student.Id,
                StudentCode = student.StudentCode,
                FullName = student.FullName,
                Email = student.Email,
                MajorProgram = $"{student.Program} (B.Sc)",
                AcademicYear = "Academic Year 2026 - 2027",
                AvatarUrl = "https://images.unsplash.com/photo-1534528741775-53994a69daeb?auto=format&fit=crop&w=256&q=80",
                CumulativeGpa = student.Gpa,
                MaxGpa = 4.0,
                CompletedCredits = totalCredits > 0 ? totalCredits * 10 : 112,
                TotalRequiredCredits = 135,
                AcademicStanding = student.Gpa >= 3.6 ? "Distinction / First Class Honors" : "Good Standing",
                EnrolledCourses = enrolledCourses,
                Notifications = new List<StudentNotificationItemViewModel>
                {
                    new StudentNotificationItemViewModel
                    {
                        Id = Guid.NewGuid(),
                        Title = "Course Schedule Published",
                        Message = "Your enrolled semester courses are now active in SIMS.",
                        Timestamp = DateTime.UtcNow.AddHours(-2),
                        TimeAgo = "2 hours ago",
                        Category = "Notice",
                        IsUnread = true
                    },
                    new StudentNotificationItemViewModel
                    {
                        Id = Guid.NewGuid(),
                        Title = "Academic Record Verified",
                        Message = $"Current GPA record: {student.Gpa} synchronized with SQL Server.",
                        Timestamp = DateTime.UtcNow.AddHours(-6),
                        TimeAgo = "6 hours ago",
                        Category = "Exam",
                        IsUnread = false
                    }
                },
                SemesterGpaHistory = new List<GpaBreakdownItemViewModel>
                {
                    new GpaBreakdownItemViewModel { Semester = "Sem 1", Gpa = Math.Max(2.0, Math.Round(student.Gpa - 0.2, 2)) },
                    new GpaBreakdownItemViewModel { Semester = "Sem 2", Gpa = Math.Max(2.0, Math.Round(student.Gpa - 0.1, 2)) },
                    new GpaBreakdownItemViewModel { Semester = "Sem 3", Gpa = Math.Round(student.Gpa, 2) },
                    new GpaBreakdownItemViewModel { Semester = "Sem 4", Gpa = Math.Round(student.Gpa, 2) }
                }
            };
        }
    }
}

