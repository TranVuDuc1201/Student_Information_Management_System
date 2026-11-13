using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIMS.Models.Entities
{
    public class Course
    {
        [Key]
        public Guid CourseId { get; set; } = Guid.NewGuid();

        [Required, MaxLength(20)]
        public string CourseCode { get; set; } = string.Empty;

        [Required, MaxLength(200)]
        public string CourseName { get; set; } = string.Empty;

        public int Credits { get; set; }

        [MaxLength(100)]
        public string ScheduleTime { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Room { get; set; } = string.Empty;

        public Guid? FacultyId { get; set; }

        // Navigation Properties
        [ForeignKey(nameof(FacultyId))]
        public virtual Faculty? Faculty { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
