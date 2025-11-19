using Microsoft.EntityFrameworkCore;
using PROG6212_POE.Models;

namespace PROG6212_POE
{
    /* Troelsen and Japikse, 2021*/
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

;

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpClient();
            builder.Services.AddSession();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // 1. Add sessionservices to the dependency injection container.
            builder.Services.AddSession(options =>
            {
                //You can set options here, like the session timeout.
                options.IdleTimeout = TimeSpan.FromMinutes(20); //The session will expire after 20 minutes of inactivity.
                options.Cookie.HttpOnly = true; // Makes the cookie inaccessible to client-side script.
                options.Cookie.IsEssential = true; // Required for GDPR compliance
            });
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
            app.UseSession(); //It has to go here...
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
/*Reference List:
Troelsen,A. and Japikse, P. 2021.Pro C# 9 with .NET 5:Foundational Principles and Practices in Programming. 10th ed.Apress.
*/