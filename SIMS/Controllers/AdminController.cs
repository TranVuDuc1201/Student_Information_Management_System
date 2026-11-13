using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIMS.Models.ViewModels;
using SIMS.Services.Interfaces;

namespace SIMS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? search = null, string? program = null, string? status = null)
        {
            var data = await _adminService.GetDashboardDataAsync(search, program, status);
            return View("Dashboard", data);
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard(string? search = null, string? program = null, string? status = null)
        {
            var data = await _adminService.GetDashboardDataAsync(search, program, status);
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateStudent(AdminDashboardViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.NewStudent.FullName) || string.IsNullOrWhiteSpace(model.NewStudent.StudentCode))
            {
                TempData["ErrorMessage"] = "Student Code and Full Name are mandatory.";
                return RedirectToAction("Dashboard");
            }

            var success = await _adminService.CreateStudentAsync(model.NewStudent);
            if (success)
            {
                TempData["SuccessMessage"] = $"Student {model.NewStudent.StudentCode} ({model.NewStudent.FullName}) registered successfully!";
            }
            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFaculty(AdminDashboardViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.NewFaculty.FacultyName) || string.IsNullOrWhiteSpace(model.NewFaculty.Email))
            {
                TempData["ErrorMessage"] = "Faculty Name and Institutional Email are required.";
                return RedirectToAction("Dashboard");
            }

            var success = await _adminService.CreateFacultyAsync(model.NewFaculty);
            if (success)
            {
                TempData["SuccessMessage"] = $"Faculty account for {model.NewFaculty.FacultyName} created successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to create faculty account. Ensure email is unique.";
            }
            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteStudent(Guid id)
        {
            var success = await _adminService.DeleteStudentAsync(id);
            if (success)
            {
                TempData["SuccessMessage"] = "Student record has been archived.";
            }
            return RedirectToAction("Dashboard");
        }
    }
}
