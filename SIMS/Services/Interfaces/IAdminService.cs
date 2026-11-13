using System;
using System.Threading.Tasks;
using SIMS.Models.ViewModels;

namespace SIMS.Services.Interfaces
{
    public interface IAdminService
    {
        Task<AdminDashboardViewModel> GetDashboardDataAsync(string? searchQuery = null, string? programFilter = null, string? statusFilter = null);
        Task<bool> CreateStudentAsync(CreateStudentInputModel input);
        Task<bool> CreateFacultyAsync(CreateFacultyInputModel input);
        Task<bool> DeleteStudentAsync(Guid id);
    }
}
