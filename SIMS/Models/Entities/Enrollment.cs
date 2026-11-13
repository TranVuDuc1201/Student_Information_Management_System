using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIMS.Models.Entities
{
    public class Enrollment
    {
        [Key]
        public Guid EnrollmentId { get; set; } = Guid.NewGuid();

        [Required]
        public Guid StudentId { get; set; }

        [Required]
        public Guid CourseId { get; set; }

        public double? AttendanceScore { get; set; }

        public double? MidtermScore { get; set; }

        public double? AssignmentScore { get; set; }

        public double? FinalScore { get; set; }

        public double? TotalScore { get; set; }

        [MaxLength(5)]
        public string LetterGrade { get; set; } = "F";

        [MaxLength(50)]
        public string Status { get; set; } = "Draft";

        [MaxLength(500)]
        public string? Note { get; set; }

        // Navigation Properties
        [ForeignKey(nameof(StudentId))]
        public virtual Student Student { get; set; } = null!;

        [ForeignKey(nameof(CourseId))]
        public virtual Course Course { get; set; } = null!;
    }
}

