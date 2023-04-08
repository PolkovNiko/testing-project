using Microsoft.AspNetCore.Mvc;
using staff_register.Models;
using System.Diagnostics;

namespace staff_register.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public DepartmentController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Departments()
        {
            return View();
        }

        public IActionResult Staff()
        {
            return View();
        }
        public IActionResult StaffOfDep()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
