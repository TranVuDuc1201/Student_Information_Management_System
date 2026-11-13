using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SIMS.Services.Interfaces;

namespace SIMS.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var data = await _studentService.GetDashboardDataAsync();
            return View("Dashboard", data);
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var data = await _studentService.GetDashboardDataAsync();
            return View(data);
        }
    }
}

