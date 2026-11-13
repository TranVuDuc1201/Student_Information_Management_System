using System.ComponentModel.DataAnnotations;

namespace SIMS.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Student ID / Code is required")]
        [MaxLength(20, ErrorMessage = "Student Code cannot exceed 20 characters")]
        [Display(Name = "Student Code")]
        public string StudentCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Full Name is required")]
        [MaxLength(100, ErrorMessage = "Full Name cannot exceed 100 characters")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Institutional Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [MaxLength(100)]
        [Display(Name = "Institutional Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Program is required")]
        [Display(Name = "Enrolled Program")]
        public string Program { get; set; } = "Software Engineering";

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please confirm your password")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}

