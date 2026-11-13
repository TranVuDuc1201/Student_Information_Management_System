using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SIMS.Models.ViewModels;
using SIMS.Services.Interfaces;

namespace SIMS.Controllers
{
    public class FacultyController : Controller
    {
        private readonly IFacultyService _facultyService;

        public FacultyController(IFacultyService facultyService)
        {
            _facultyService = facultyService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(Guid? courseId = null)
        {
            var data = await _facultyService.GetGradingSheetAsync(courseId);
            return View("Portal", data);
        }

        [HttpGet]
        public async Task<IActionResult> Portal(Guid? courseId = null)
        {
            var data = await _facultyService.GetGradingSheetAsync(courseId);
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveGrades(FacultyGradingViewModel model)
        {
            await _facultyService.SaveGradesAsync(model);
            TempData["SuccessMessage"] = $"Grades for {model.SelectedCourseCode} saved & calculated successfully!";
            return RedirectToAction("Portal", new { courseId = model.SelectedCourseId });
        }
    }
}

