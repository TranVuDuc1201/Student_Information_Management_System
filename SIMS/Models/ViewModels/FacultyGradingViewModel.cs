using System;
using System.Collections.Generic;

namespace SIMS.Models.ViewModels
{
    public class StudentGradeEntryViewModel
    {
        public Guid EnrollmentId { get; set; }
        public Guid StudentId { get; set; }
        public string StudentCode { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;

        public double? AttendanceScore { get; set; } // 10%
        public double? MidtermScore { get; set; }    // 30%
        public double? AssignmentScore { get; set; } // 20%
        public double? FinalScore { get; set; }      // 40%

        public double? TotalCalculatedScore { get; set; }
        public string LetterGrade { get; set; } = "F";
        public string Status { get; set; } = "Draft"; // Draft, Submitted, Published
        public string? Note { get; set; }
    }

    public class CourseSelectionViewModel
    {
        public Guid CourseId { get; set; }
        public string CourseCode { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public int TotalEnrolled { get; set; }
        public int MaxCapacity { get; set; }
    }

    public class FacultyGradingViewModel
    {
        public Guid FacultyId { get; set; }
        public string FacultyName { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;

        // Current Course Selected
        public Guid SelectedCourseId { get; set; }
        public string SelectedCourseCode { get; set; } = string.Empty;
        public string SelectedCourseName { get; set; } = string.Empty;
        public string Semester { get; set; } = "Fall 2026";

        public string GradingStrategy { get; set; } = "Standard-Weighted (10-30-20-40)";
        public bool AutoCalculateGpa { get; set; } = true;

        public List<CourseSelectionViewModel> AssignedCourses { get; set; } = new();
        public List<StudentGradeEntryViewModel> StudentGrades { get; set; } = new();

        public int GradedCount => StudentGrades.FindAll(s => s.FinalScore.HasValue).Count;
        public int PendingCount => StudentGrades.Count - GradedCount;
    }
}

