using System;
using System.Threading.Tasks;
using SIMS.Models.ViewModels;

namespace SIMS.Services.Interfaces
{
    public interface IStudentService
    {
        Task<StudentDashboardViewModel> GetDashboardDataAsync(Guid? studentId = null);
    }
}

