using System;
using System.Collections.Generic;

namespace SIMS.Models.ViewModels
{
    public class CourseEnrollmentCardViewModel
    {
        public Guid CourseId { get; set; }
        public string CourseCode { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public string InstructorName { get; set; } = string.Empty;
        public int Credits { get; set; }
        public int ProgressPercentage { get; set; }
        public string CurrentGrade { get; set; } = "N/A";
        public string ScheduleTime { get; set; } = string.Empty;
        public string Room { get; set; } = string.Empty;
        public string ColorClass { get; set; } = "primary";
    }

    public class StudentNotificationItemViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string TimeAgo { get; set; } = string.Empty;
        public string Category { get; set; } = "General"; // Exam, Assignment, Notice
        public bool IsUnread { get; set; }
    }

    public class GpaBreakdownItemViewModel
    {
        public string Semester { get; set; } = string.Empty;
        public double Gpa { get; set; }
    }

    public class StudentDashboardViewModel
    {
        public Guid StudentId { get; set; }
        public string StudentCode { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string MajorProgram { get; set; } = string.Empty;
        public string AcademicYear { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;

        // GPA Metrics
        public double CumulativeGpa { get; set; }
        public double MaxGpa { get; set; } = 4.0;
        public int CompletedCredits { get; set; }
        public int TotalRequiredCredits { get; set; }
        public string AcademicStanding { get; set; } = "Dean's List";

        public List<CourseEnrollmentCardViewModel> EnrolledCourses { get; set; } = new();
        public List<StudentNotificationItemViewModel> Notifications { get; set; } = new();
        public List<GpaBreakdownItemViewModel> SemesterGpaHistory { get; set; } = new();
    }
}

