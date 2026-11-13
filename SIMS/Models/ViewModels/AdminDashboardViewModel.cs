using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SIMS.Models.ViewModels
{
    public class StudentRecordItemViewModel
    {
        public Guid Id { get; set; }
        public string StudentCode { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Program { get; set; } = string.Empty;
        public DateTime EnrollmentDate { get; set; }
        public string Status { get; set; } = "Active"; // Active, Suspended, Graduated
        public double Gpa { get; set; }
    }

    public class SystemAuditLogViewModel
    {
        public Guid Id { get; set; }
        public string Action { get; set; } = string.Empty;
        public string PerformedBy { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string TargetEntity { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string TimeAgo { get; set; } = string.Empty;
        public string Severity { get; set; } = "Info"; // Info, Warning, Critical
    }

    public class CreateStudentInputModel
    {
        [Required]
        public string StudentCode { get; set; } = string.Empty;

        [Required]
        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
        public string Program { get; set; } = "Software Engineering";
        public string DegreeLevel { get; set; } = "Undergraduate";
        public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;
        public string Password { get; set; } = "password123";
    }

    public class CreateFacultyInputModel
    {
        [Required(ErrorMessage = "Faculty name is required")]
        public string FacultyName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Institutional email is required"), EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string Department { get; set; } = "Department of Software Engineering & Architecture";

        [Required, MinLength(6)]
        public string Password { get; set; } = "faculty123";
    }

    public class AdminDashboardViewModel
    {
        public string AdminName { get; set; } = string.Empty;
        public int TotalStudents { get; set; }
        public int TotalFaculty { get; set; }
        public int ActiveCourses { get; set; }
        public double AverageSystemGpa { get; set; }
        public int SystemAlertsCount { get; set; }

        public string SearchQuery { get; set; } = string.Empty;
        public string SelectedProgramFilter { get; set; } = "All";
        public string SelectedStatusFilter { get; set; } = "All";

        public List<StudentRecordItemViewModel> Students { get; set; } = new();
        public List<SystemAuditLogViewModel> AuditLogs { get; set; } = new();
        public CreateStudentInputModel NewStudent { get; set; } = new();
        public CreateFacultyInputModel NewFaculty { get; set; } = new();
    }
}
