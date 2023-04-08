using Microsoft.AspNetCore.Mvc;
using staff_register.Models;
using System.Diagnostics;

namespace staff_register.Controllers
{
    public class UsersController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public UsersController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Autorization()
        {
            return View();
        }
        public IActionResult Registration()
        {
            return View();
        }
        public IActionResult UserProfile()
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
