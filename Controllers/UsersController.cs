using Microsoft.AspNetCore.Mvc;
using staff_register.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace staff_register.Controllers
{
    public class UsersController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        RegeditContext db;
        public UsersController(ILogger<HomeController> logger, RegeditContext db)
        {
            _logger = logger;
            this.db = db;
        }

        [HttpGet]
        public IActionResult Autorization()
        {
            return View();
        }

        [HttpPost]
        public async Task<IResult> Autorization(User users, string? returnUrl)
        {
            if(users.Login.IsNullOrEmpty() || users.Password.IsNullOrEmpty()) return Results.BadRequest("Поля не заполнены");
            User? valid = db.Users.FirstOrDefault(val => val.Login == users.Login && val.Password == users.Password);
            if (valid is null) return Results.Unauthorized();
            var claims = new List<Claim> { new Claim(ClaimTypes.Role, valid.Rank) };
            var claimIdentitys = new ClaimsIdentity(claims, "Cookies");
            var claimParticle = new ClaimsPrincipal(claimIdentitys);
            await HttpContext.SignInAsync(claimParticle);
            return Results.Redirect(returnUrl??"/");
        }

        [Authorize(Policy = "Админ")]
        public IActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            db.Users.Add(user);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }
        [Authorize(Policy = "Пользователь")] //тут же добавиться только авторизированным пользователям
        public  IActionResult UserProfile(int? value)
        {
            ViewData["type"] = "update";
            List<Staff> data = new List<Staff> { db.Staff.FirstOrDefault(test => test.Id == value) };
            return View(data); //тут выборка на данные авторизированного пользвателя(теперь правильно)
        }
        public IActionResult CreateStaff()
        {
            ViewData["type"] = "create";
            return View("UserProfile");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UpdateStaff(newStaff newStaff)
        {
            if (newStaff != null)
            {
                return BadRequest("Поля не заполнены!");
            }
            Staff staff = new Staff { Fio = newStaff.Fio, Birthday = newStaff.Birthday, Adress = newStaff.Adress,
            FamilyStatus = newStaff.FamilyStatus, Wage = newStaff.Wage, Department = newStaff.Department, Number = newStaff.Number};
            byte[] image = null;
            using (var binaryReader = new BinaryReader(newStaff.Photo.OpenReadStream()))
            {
                image = binaryReader.ReadBytes((int)newStaff.Photo.Length);
            }
            staff.Photo = image;
            db.Staff.Update(staff);
            await db.SaveChangesAsync();
            return RedirectToAction("", "");
        }
        [HttpPost]
        public async Task<IActionResult> CreateStaff(newStaff newStaff)
        {
            Staff staff = new Staff
            {
                Fio = newStaff.Fio,
                Birthday = newStaff.Birthday,
                Adress = newStaff.Adress,
                FamilyStatus = newStaff.FamilyStatus,
                Wage = newStaff.Wage,
                Department = newStaff.Department,
                Number = newStaff.Number
            };
            byte[] image = null;
            using (var binaryReader = new BinaryReader(newStaff.Photo.OpenReadStream()))
            {
                image = binaryReader.ReadBytes((int)newStaff.Photo.Length);
            }
            staff.Photo = image;
            db.Staff.Add(staff);
            await db.SaveChangesAsync();
            return RedirectToAction("Search", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
