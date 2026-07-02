using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MVC_Dashboard.BLL;
using MVC_Dashboard.BLL.Interfaces;
using MVC_Dashboard.BLL.Repositories;
using MVC_Dashboard.DAL.Data;
using MVC_Dashboard.DAL.Models;
using MVC_Dashboard_PL.Helpers;
using MVC_Dashboard_PL.Services.EmailSender;
using MVC_Dashboard_PL.Services.SendEmail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_Dashboard_PL
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
             
            services.AddDbContext<ApplicationDbContext>(option =>
                     option.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IGenericRepository<Department>, GenericRepository<Department>>();

            services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IEmailSender, EmailSender>();

            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //                     .AddCookie(options =>{ options.LoginPath = "/Account/SignIn"; });
            

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredUniqueChars = 2;
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredLength = 5;

                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(5);

                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<ApplicationDbContext>()
              .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/SignIn";
                options.AccessDeniedPath = "/Home/Error";
                options.LogoutPath = "/Account/SignIn";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
            });

            //services.AddAuthentication(); // This Overload => already registerd in AddIdentity

            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = "Identity.Application"; // By Default
            //    options.
            //});

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
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
                
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
