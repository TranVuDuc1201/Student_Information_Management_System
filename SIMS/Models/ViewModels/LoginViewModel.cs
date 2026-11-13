using System.ComponentModel.DataAnnotations;

namespace SIMS.Models.ViewModels
{
    public enum UserRoleType
    {
        Academic,
        SystemAdmin
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username or Institutional Email is required")]
        [Display(Name = "Username / Email")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; } = false;

        public UserRoleType Role { get; set; } = UserRoleType.Academic;

        public string? ReturnUrl { get; set; }

        public string? ErrorMessage { get; set; }
    }
}
