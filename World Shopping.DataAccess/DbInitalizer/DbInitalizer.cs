using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using World_Shopping.Utilities;
using World_Shopping.Web.Entities.Models;

namespace World_Shopping.DataAccess.DbInitalizer
{
    public class DbInitalizer : IDbInitalizer
    {
       
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly  ApplicationDbContext _context;
        public DbInitalizer(

            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context
            )

        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }
        public void Initalizer()
        {
            try
            {
                if (_context.Database.GetAppliedMigrations().Count() > 0)
                {
                    _context.Database.Migrate();
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            //Roles
            if (!_roleManager.RoleExistsAsync(SD.AdminRole).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.AdminRole)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.EditorRole)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.CustomerRole)).GetAwaiter().GetResult();


                //Users
                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "Admin@WorldShopping.com",
                    Email = "Admin@WorldShopping.com",
                    Name = "AdminStrator",
                    PhoneNumber = "1234567890",
                    Address = "Sohag",
                    City = "Sohag",

                },"Admin.101010").GetAwaiter().GetResult();

                ApplicationUser User = _context.ApplicationUsers.FirstOrDefault(u => u.Email == "Admin@WorldShopping.com");

                _userManager.AddToRoleAsync(User, SD.AdminRole).GetAwaiter().GetResult(); 
            }

            return;
        }
    }
}
