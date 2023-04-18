using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using staff_register.Models;
using System.Diagnostics;

namespace staff_register.Controllers
{
    public class HomeController : Controller
    {
        RegeditContext db;
        public HomeController(RegeditContext context)
        {
            db = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await db.Users.ToListAsync());
        }
        [Authorize(Roles = "Пользователь")]
        public async Task<IActionResult> Search()
        {
            return View(await db.Staff.ToListAsync());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}