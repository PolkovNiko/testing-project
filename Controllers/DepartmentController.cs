using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using staff_register.Models;
using System.Diagnostics;

namespace staff_register.Controllers
{
    public class DepartmentController : Controller
    {
        RegeditContext _db;
        public DepartmentController(RegeditContext db) 
        { 
            _db = db;
        }

        public IActionResult Departments()
        {
            return View(_db.Departmens.ToList());
        }
        [Authorize(Roles = "Пользователь")]
        public IActionResult CreateDep()
        {
            return View();
        }
        [Authorize(Roles = "Пользователь")]
        public IActionResult DeleteDep()
        {

            return View();
        }
        //public IActionResult CreateDep()
        //{
        //    return View();
        //}
        [Authorize(Roles = "Пользователь")]
        public IActionResult Staff()
        {
            return View();
        }
        [Authorize(Roles = "Пользователь")]
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
