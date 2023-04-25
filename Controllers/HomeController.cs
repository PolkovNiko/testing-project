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

        //-------------------------------------------------------------------------------------------
        //Method Get


        public async Task<IActionResult> Index()
        {
            ViewData["rank"] = HttpContext.Request.Cookies["rank"];
            return View(await db.Users.ToListAsync());
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> Search()
        {
            return View(await db.Staff.ToListAsync());
        }





        [HttpPost]
        public IActionResult Search(Search querys)
        {
            querys.Fio = HttpContext.Request.Form["Fio"].ToString() == "on" ? true : false;
            querys.Number = HttpContext.Request.Form["Number"].ToString() == "on" ? true : false;
            querys.Adress = HttpContext.Request.Form["Adress"].ToString() == "on" ? true : false;
            Dictionary<(bool, bool, bool), Func<IQueryable<Staff>>> quary = new Dictionary<(bool, bool, bool), Func<IQueryable<Staff>>>()
            {
                [(true, false, false)] = () => { var x = db.Staff.Where(p => EF.Functions.Like(p.Fio, querys.Query + "%")); return x; },
                [(true, true, false)] = () => db.Staff.Where(p => EF.Functions.Like(p.Fio, querys.Query + "%") || EF.Functions.Like(p.Number, querys.Query + "%")),
                [(true, true, true)] = () => db.Staff.Where(p => EF.Functions.Like(p.Fio, querys.Query + "%") || EF.Functions.Like(p.Number, querys.Query + "%") || EF.Functions.Like(p.Adress, querys.Query + "%")),

                [(false, true, false)] = () => db.Staff.Where(p => EF.Functions.Like(p.Number, querys.Query + "%")),
                [(false, true, true)] = () => db.Staff.Where(p => EF.Functions.Like(p.Number, querys.Query + "%") || EF.Functions.Like(p.Adress, querys.Query + "%")),

                [(false, false, true)] = () => db.Staff.Where(p => EF.Functions.Like(p.Adress, querys.Query + "%")),
                [(true, false, true)] = () => db.Staff.Where(p => EF.Functions.Like(p.Adress, querys.Query + "%") || EF.Functions.Like(p.Fio, querys.Query + "%"))
            };
            var result = quary[(querys.Fio, querys.Number, querys.Adress)];
            if (result != null)
            {
                return View(result.Invoke().ToList());
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