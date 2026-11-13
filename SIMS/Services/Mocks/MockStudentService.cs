using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SIMS.Models.ViewModels;
using SIMS.Services.Interfaces;

namespace SIMS.Services.Mocks
{
    public class MockStudentService : IStudentService
    {
        public Task<StudentDashboardViewModel> GetDashboardDataAsync(Guid? studentId = null)
        {
            var vm = new StudentDashboardViewModel
            {
                StudentId = studentId ?? Guid.NewGuid(),
                StudentCode = "BH02383",
                FullName = "Tran Vu Duc",
                Email = "ductvbh02383@fpt.edu.vn",
                MajorProgram = "Software Engineering (B.Sc)",
                AcademicYear = "Senior Year (2026 - 2027)",
                AvatarUrl = "https://images.unsplash.com/photo-1534528741775-53994a69daeb?auto=format&fit=crop&w=256&q=80",
                CumulativeGpa = 3.82,
                MaxGpa = 4.0,
                CompletedCredits = 112,
                TotalRequiredCredits = 135,
                AcademicStanding = "Distinction / First Class Honors",
                EnrolledCourses = new List<CourseEnrollmentCardViewModel>
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
                    },
                    new CourseEnrollmentCardViewModel
                    {
                        CourseId = Guid.NewGuid(),
                        CourseCode = "DB305",
                        CourseName = "High-Performance Distributed Databases",
                        InstructorName = "Assoc. Prof. Alex Chen",
                        Credits = 3,
                        ProgressPercentage = 92,
                        CurrentGrade = "A+",
                        ScheduleTime = "Friday (08:00 - 11:00)",
                        Room = "Server Lab Alpha",
                        ColorClass = "accent-cyan"
                    },
                    new CourseEnrollmentCardViewModel
                    {
                        CourseId = Guid.NewGuid(),
                        CourseCode = "UX210",
                        CourseName = "Design Systems & Human-Centered UX",
                        InstructorName = "Sarah Jenkins, M.Des",
                        Credits = 3,
                        ProgressPercentage = 75,
                        CurrentGrade = "B+",
                        ScheduleTime = "Thu (15:45 - 18:00)",
                        Room = "Studio 105",
                        ColorClass = "accent-pink"
                    }
                },
                Notifications = new List<StudentNotificationItemViewModel>
                {
                    new StudentNotificationItemViewModel
                    {
                        Id = Guid.NewGuid(),
                        Title = "Midterm Examination Scheduled",
                        Message = "APDP301 design presentation is confirmed for next Monday at 09:00 AM.",
                        Timestamp = DateTime.UtcNow.AddHours(-2),
                        TimeAgo = "2 hours ago",
                        Category = "Exam",
                        IsUnread = true
                    },
                    new StudentNotificationItemViewModel
                    {
                        Id = Guid.NewGuid(),
                        Title = "Course Materials Published",
                        Message = "New slides for Cloud Architecture (Week 8) are available for download.",
                        Timestamp = DateTime.UtcNow.AddHours(-6),
                        TimeAgo = "6 hours ago",
                        Category = "Notice",
                        IsUnread = true
                    },
                    new StudentNotificationItemViewModel
                    {
                        Id = Guid.NewGuid(),
                        Title = "Grading Feedback Released",
                        Message = "Your Lab Assignment 3 for Distributed Databases was graded: 98/100.",
                        Timestamp = DateTime.UtcNow.AddDays(-1),
                        TimeAgo = "Yesterday",
                        Category = "Assignment",
                        IsUnread = false
                    }
                },
                SemesterGpaHistory = new List<GpaBreakdownItemViewModel>
                {
                    new GpaBreakdownItemViewModel { Semester = "Sem 1", Gpa = 3.65 },
                    new GpaBreakdownItemViewModel { Semester = "Sem 2", Gpa = 3.72 },
                    new GpaBreakdownItemViewModel { Semester = "Sem 3", Gpa = 3.80 },
                    new GpaBreakdownItemViewModel { Semester = "Sem 4", Gpa = 3.82 }
                }
            };

            return Task.FromResult(vm);
        }
    }
}

