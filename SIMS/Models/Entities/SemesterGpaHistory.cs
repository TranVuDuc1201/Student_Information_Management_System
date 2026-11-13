using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIMS.Models.Entities
{
    public class SemesterGpaHistory
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid StudentId { get; set; }

        [Required, MaxLength(50)]
        public string Semester { get; set; } = string.Empty;

        public double Gpa { get; set; }

        // Navigation Properties
        [ForeignKey(nameof(StudentId))]
        public virtual Student Student { get; set; } = null!;
    }
}

