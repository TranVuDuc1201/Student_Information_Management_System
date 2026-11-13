using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SIMS.Models.Entities
{
    public class Faculty
    {
        [Key]
        public Guid FacultyId { get; set; } = Guid.NewGuid();

        [Required, MaxLength(100)]
        public string FacultyName { get; set; } = string.Empty;

        [EmailAddress, MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Department { get; set; } = string.Empty;

        // Navigation Properties
        public virtual ICollection<Course> AssignedCourses { get; set; } = new List<Course>();
    }
}
