using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using staff_register.Models;
using Microsoft.AspNetCore.Mvc;

namespace staff_register
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            string connection = builder.Configuration.GetConnectionString("regedit");
            builder.Services.AddDbContext<RegeditContext>(option => option.UseSqlServer(connection));


            // Add services to the container.
            builder.Services.AddAuthentication("Cookies")
                .AddCookie(adress =>
                {
                    adress.LoginPath = "/Users/Autorization";
                    adress.AccessDeniedPath = "/Users/AccessDenied";
                });
            builder.Services.AddAuthorization(conf =>
            {
                conf.AddPolicy("Админ", policy =>
                {
                    policy.RequireClaim(ClaimTypes.Role, "Админ");
                });
                conf.AddPolicy("Пользователь", policy =>
                {
                    policy.RequireClaim(ClaimTypes.Role, "Пользователь");
                });
            });
            builder.Services.AddControllersWithViews();

            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapGet("/logout", async (HttpContext context) =>
            {
                await context.SignOutAsync("Cookies");
                return Results.Redirect("/");
            });
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            //app.Map("/test", (ClaimsPrincipal claimsPrincipal) =>
            //{
            //    var user = claimsPrincipal.Identity;
            //    if (user is not null && user.IsAuthenticated)
            //    {
            //        return $"Пользователь аутентифицирован";
            //    }
            //    else
            //    {
            //        return "Пользователь НЕ аутентифицирован";
            //    }
            //});
            //app.Map("/1", (HttpContext context) =>
            //{
            //    var name = context.User.FindFirst(ClaimTypes.Name);
            //    var role = context.User.FindFirst(ClaimTypes.Role);

            //    return $"Имя = {name?.Value} --- Role = {role?.Value}";
            //});
            app.Run();
        }
    }
}