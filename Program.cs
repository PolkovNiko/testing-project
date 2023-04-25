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
                conf.AddPolicy("Admin", policy =>
                {
                    policy.RequireClaim(ClaimTypes.Role, "Admin");
                });
                conf.AddPolicy("User", policy =>
                {
                    policy.RequireClaim(ClaimTypes.Role, "User");
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
                if (context.Request.Cookies.ContainsKey("Id"))
                {
                    context.Response.Cookies.Delete("Id");
                }
                return Results.Redirect("/");
            });
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.Run();
        }
    }
}