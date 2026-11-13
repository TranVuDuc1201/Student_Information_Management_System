using System.Threading.Tasks;
using SIMS.Models.ViewModels;

namespace SIMS.Services.Interfaces
{
    public interface IAuthService
    {
        Task<(bool Success, string Message, string RedirectAction, string RedirectController, string Role, string FullName, string Identifier)> AuthenticateAsync(LoginViewModel model);
        Task<(bool Success, string Message)> RegisterStudentAsync(RegisterViewModel model);
    }
}
