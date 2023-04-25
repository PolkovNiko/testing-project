using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using staff_register.Models;
using System.Data;
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


        //-------------------------------------------------------------------------------------------
        //Method Get


        public IActionResult Departments()
        {
            ViewData["Id"] = HttpContext.Request.Cookies["Id"];
            return View(_db.Departmens.ToList());
        }

        [Authorize(Roles = "User")]
        public IActionResult CreateDep()
        {
            ViewData["Id"] = HttpContext.Request.Cookies["Id"];
            return View();
        }

        [Authorize(Roles = "User")]
        public IActionResult UpdateDep(int? id)
        {
            if(id != null)
            {
                Departmen? deps = _db.Departmens.FirstOrDefault(p=> p.Id == id && p.IdBoss.ToString() == HttpContext.Request.Cookies["Id"]);
                if(deps != null)
                {
                    ViewData["Id"] = HttpContext.Request.Cookies["Id"];
                    ViewData["iddep"] = deps.Id;
                    return View(_db.Departmens.ToList());
                }
            }
            return Forbid();
        }

        [Authorize(Roles = "User")]
        public IActionResult Staff()
        {
            return View();
        }

        [Authorize(Roles = "User")]
        public IActionResult StaffOfDep(int? id)
        {
            ViewData["dep"] = id;
            return View(_db.Staff.ToList());
        }

        //-------------------------------------------------------------------------------------------
        //Method Post


        [Authorize(Roles = "User")]
        [HttpPost]
        public IResult CreateDep(Departmen deps)
        {
            if(deps is null) { return Results.BadRequest("Данные не введены"); }
            _db.Departmens.Add(deps);
            _db.SaveChanges();
            return Results.Redirect("Departments");
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public IActionResult DeleteDep(int? id)
        {
            if(id != null)
            {
                Departmen? dep = _db.Departmens.FirstOrDefault(p => p.Id == id && p.IdBoss.ToString() == HttpContext.Request.Cookies["Id"]);
                if(dep != null)
                {
                    _db.Departmens.Remove(dep);
                    _db.SaveChanges();
                    return RedirectToAction("Departments");
                }
            }
            return Forbid();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDep(Departmen dep)
        {
            if(dep != null)
            {
                _db.Departmens.Update(dep);
                await _db.SaveChangesAsync();
                return RedirectToAction("Departments");
                //return View("Departments", _db.Departmens.ToList());
            }
            return NotFound();
        }

        //-------------------------------------------------------------------------------------------

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
