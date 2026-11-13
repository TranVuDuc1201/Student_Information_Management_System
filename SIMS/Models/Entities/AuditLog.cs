using System;
using System.ComponentModel.DataAnnotations;

namespace SIMS.Models.Entities
{
    public class AuditLog
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, MaxLength(100)]
        public string Action { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string PerformedBy { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Role { get; set; } = "System";

        [MaxLength(100)]
        public string TargetEntity { get; set; } = string.Empty;

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        [MaxLength(20)]
        public string Severity { get; set; } = "Info";
    }
}

