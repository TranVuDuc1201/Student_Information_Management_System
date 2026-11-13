using System.Threading.Tasks;
using SIMS.Models.ViewModels;
using SIMS.Services.Interfaces;

namespace SIMS.Services.Mocks
{
    public class MockAuthService : IAuthService
    {
        public Task<(bool Success, string Message, string RedirectAction, string RedirectController, string Role, string FullName, string Identifier)> AuthenticateAsync(LoginViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password))
            {
                return Task.FromResult((false, "Invalid username or password.", "Login", "Auth", string.Empty, string.Empty, string.Empty));
            }

            if (model.Role == UserRoleType.SystemAdmin)
            {
                return Task.FromResult((true, "Welcome Administrator", "Index", "Admin", "Admin", "System Administrator", "admin"));
            }

            return Task.FromResult((true, "Welcome Student", "Index", "Student", "Student", "Tran Vu Duc", "BH02383"));
        }

        public Task<(bool Success, string Message)> RegisterStudentAsync(RegisterViewModel model)
        {
            return Task.FromResult((true, "Student registered successfully."));
        }
    }
}
