using Microsoft.EntityFrameworkCore;
using WeaponShop.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;
using Microsoft.AspNetCore.Authentication.Cookies;
using WeaponShop.Controllers;
using WeaponShop.Filters;
using static WeaponShop.Controllers.AccountController;

namespace WeaponShop
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddSerilog();
            });
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            // Фильтр для админ-панели
            services.AddScoped<AdminFilter>();
            // Подключение к ДБ PostgreSQL
            services.AddDbContext<DatabaseContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDistributedMemoryCache();
            services.AddHttpClient();
            services.AddScoped<IUserFactory, UserFactory>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.IdleTimeout = TimeSpan.FromMinutes(20); 
            });
            services.AddTransient<IDbAdapter, PostgreSQLAdapter>();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.HttpOnly = true;
                    options.Cookie.IsEssential = true;
                    options.ExpireTimeSpan = TimeSpan.FromDays(1);
                    });
            
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseSession();
            app.UseSerilogRequestLogging();

           

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Register}/{id?}");
                endpoints.MapControllerRoute(
                    name: "dbshow",
                    pattern: "{controller=DBshow}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                name: "profile",
                pattern: "{controller=User}/{action=ShowProfile}/{id?}");
                endpoints.MapControllerRoute(
                    name: "productDetails",
                    pattern: "ProductDetails/Details/{id}",
                    defaults: new { controller = "ProductDetails", action = "Details" });
                endpoints.MapControllerRoute(
                    name: "products",
                    pattern: "AllProducts/Products/{productType?}/{orderType?}/{searchTerm?}",
                    defaults: new { controller = "AllProducts", action = "Products" });
                endpoints.MapControllerRoute(
                    name: "adminpanel",
                    pattern: "Admin/Panel/",
                    defaults: new { controller = "AdminPanel", action = "Panel" });
            });
        }
    }
}
