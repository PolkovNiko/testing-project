using Microsoft.AspNetCore.Mvc;
using staff_register.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Linq;

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


        //-------------------------------------------------------------------------------------------
        //Method Get


        [HttpGet]
        public IActionResult Autorization()
        {
            return View();
        }

        [Authorize(Policy = "User")] //тут же добавиться только авторизированным пользователям
        public  IActionResult UserProfile()
        {
            //ViewData["type"] = "update";
            var data = db.Staff.FirstOrDefault(test => test.Id.ToString() == HttpContext.Request.Cookies["Id"]);
            if (data is null) 
            {
                newStaff st = new newStaff() { Id = Convert.ToInt32(HttpContext.Request.Cookies["Id"]) };
                ViewData["Id"] = HttpContext.Request.Cookies["Id"];
                return View("CreateProfile", st);
            } 
            return View(data); //тут выборка на данные авторизированного пользвателя(теперь правильно)
        }

        [Authorize(Policy = "Admin")]
        public IActionResult Registration()
        {
            return View();
        }

        [Authorize(Policy = "User")]
        public IActionResult CreateProfile(newStaff staf)
        {
            ViewData["type"] = "create";
            return View(staf);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [Authorize(Policy = "Admin")]
        public IActionResult UpdateUser(int? id)
        {
            if (id != null)
            {
                User? users = db.Users.FirstOrDefault(p => p.Id == id);
                if (users != null)
                {
                    ViewData["idUser"] = users.Id;
                    return View(db.Users.ToList());
                }
            }
            return NotFound();
        }



        //Method Post

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
            HttpContext.Response.Cookies.Append("Id", valid.Id.ToString());
            HttpContext.Response.Cookies.Append("rank", valid.Rank);
            ViewData["Id"] = valid.Id.ToString();
            return Results.Redirect(returnUrl??"/");
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            db.Users.Add(user);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStaff(newStaff newStaff)
        {
            if (newStaff == null)
            {
                return BadRequest("Поля не заполнены!");
            }
            Staff? temp = db.Staff.FirstOrDefault(u=>u.Id == newStaff.Id);
            //Staff staff = new Staff { Fio = newStaff.Fio, Birthday = newStaff.Birthday, Adress = newStaff.Adress,
            //FamilyStatus = newStaff.FamilyStatus, Wage = newStaff.Wage, Department = newStaff.Department, Number = newStaff.Number};
            if (temp != null)
            {
                temp.Fio = newStaff.Fio;
                temp.Birthday = newStaff.Birthday;
                temp.Number = newStaff.Number;
                temp.Adress = newStaff.Adress;
                temp.FamilyStatus = newStaff.FamilyStatus;
                temp.Wage = newStaff.Wage;
                temp.Department = newStaff.Department;
            }
            else Results.NotFound();
            byte[] image = null;
            if(newStaff.Photo != null)
            {
                using (var binaryReader = new BinaryReader(newStaff.Photo.OpenReadStream()))
                {
                    image = binaryReader.ReadBytes((int)newStaff.Photo.Length);
                }
                temp.Photo = image;
            }
            db.Staff.Update(temp);
            await db.SaveChangesAsync();
            return RedirectToAction("", "");
        }

        [HttpPost]
        public async Task<IActionResult> CreateStaff(newStaff newStaff)
        {
            User users = new User();
            var i = db.Users.FirstOrDefault(u => u.Id == newStaff.Id);
            if(i != null)
            {
                Staff staff = new Staff
                {
                    Id = i.Id,
                    Fio = newStaff.Fio,
                    Birthday = newStaff.Birthday,
                    Adress = newStaff.Adress,
                    FamilyStatus = newStaff.FamilyStatus,
                    Wage = newStaff.Wage,
                    Department = newStaff.Department,
                    Number = newStaff.Number
                };
                using (var binaryReader = new BinaryReader(newStaff.Photo.OpenReadStream()))
                {
                    byte[] image = null;
                    image = binaryReader.ReadBytes((int)newStaff.Photo.Length);
                    staff.Photo = image;
                }
                db.Staff.Add(staff);
                await db.SaveChangesAsync();

            }
            return RedirectToAction("Search", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(User newUser)
        {
            if(newUser != null)
            {
                User? newU = db.Users.FirstOrDefault(p=> p.Id == newUser.Id);
                if(newU != null)
                {
                    newU.Login = newUser.Login;
                    newU.Password = newUser.Password;
                    newU.Email = newUser.Email;
                    newU.Rank = newUser.Rank;
                    db.Users.Update(newU);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index", "Home");
                }

            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(int? id)
        {
            if(id != null)
            {
                User? delU = db.Users.FirstOrDefault(p => p.Id == id);
                Staff? delS = db.Staff.FirstOrDefault(p => p.Id == id);
                Departmen? delD = db.Departmens.FirstOrDefault(p => p.IdBoss == id);
                if (delU != null && delS != null && delD != null)
                {
                    db.Departmens.Remove(delD);
                    db.Staff.Remove(delS);
                    db.Users.Remove(delU);
                    //this must bu logger
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index", "Home");
                }
                else if (delU != null && delS != null)
                {
                    db.Staff.Remove(delS);
                    //this must bu logger
                    db.Users.Remove(delU);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index", "Home");
                }
                else if (delU != null)
                {
                    db.Users.Remove(delU);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index", "Home");
                }
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
