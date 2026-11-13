using System;
using System.Threading.Tasks;
using SIMS.Models.ViewModels;

namespace SIMS.Services.Interfaces
{
    public interface IFacultyService
    {
        Task<FacultyGradingViewModel> GetGradingSheetAsync(Guid? courseId = null);
        Task<bool> SaveGradesAsync(FacultyGradingViewModel model);
    }
}

