using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using SIMS.Data;
using SIMS.Services.Interfaces;
using SIMS.Services.Implementations;

namespace SIMS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. Configure Database Context (Entity Framework Core with SQL Server)
            builder.Services.AddDbContext<SimsDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // 2. Configure Cookie Authentication & Authorization
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Auth/Login";
                    options.LogoutPath = "/Auth/Logout";
                    options.AccessDeniedPath = "/Auth/Login";
                    options.ExpireTimeSpan = TimeSpan.FromDays(7);
                    options.SlidingExpiration = true;
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
                });

            builder.Services.AddAuthorization();

            // 3. Add MVC controllers with views
            builder.Services.AddControllersWithViews();

            // 4. Register SQL Services for Dependency Injection
            builder.Services.AddScoped<IAuthService, SqlAuthService>();
            builder.Services.AddScoped<IStudentService, SqlStudentService>();
            builder.Services.AddScoped<IFacultyService, SqlFacultyService>();
            builder.Services.AddScoped<IAdminService, SqlAdminService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Auth/Login");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            // Authentication must run before Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            
            // Set Auth/Login as default route
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Auth}/{action=Login}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
