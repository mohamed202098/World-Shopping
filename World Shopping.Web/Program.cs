using Microsoft.EntityFrameworkCore;
using NToastNotify;
using World_Shopping.DataAccess;
using World_Shopping.DataAccess.Implementation;
using World_Shopping.Models.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using World_Shopping.Utilities;
using Microsoft.AspNetCore.Identity.UI.Services;
using Stripe;
using World_Shopping.DataAccess.DbInitalizer;

namespace World_Shopping.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            //(1) Add Run Time Compilation
            builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
            //(2) Add DbContext
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
                builder.Configuration.GetConnectionString("My_World_Shopping")
            ));

            builder.Services.Configure<StripeData>(builder.Configuration.GetSection("stripe"));

            builder.Services.AddIdentity<IdentityUser,IdentityRole>(option=>option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(4))
                
                .AddDefaultTokenProviders().AddDefaultUI()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddSingleton<IEmailSender, EmailSender>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IDbInitalizer, DbInitalizer>();

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession();


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

            StripeConfiguration.ApiKey = builder.Configuration.GetSection("stripe:Secretkey").Get<string>();

            SeedDb();

            app.UseAuthorization();

            app.UseSession();

            app.MapRazorPages();



            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Admin}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "Customer",
                pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

            app.Run();

            void SeedDb()
            {
                using (var Scope = app.Services.CreateScope())
                {
                    var dbInitalizer = Scope.ServiceProvider.GetRequiredService<IDbInitalizer>();
                    dbInitalizer.Initalizer();
                }
                           
            }

        }
    }
}
