using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using World_Shopping.DataAccess;
using World_Shopping.Utilities;

namespace World_Shopping.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.AdminRole)]
 
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            string userId = claim.Value;

            return View(_context.ApplicationUsers.Where(x => x.Id != userId).ToList());
        }
       
        public IActionResult LockUnlock(string? id)
        {
            var User = _context.ApplicationUsers.FirstOrDefault(x => x.Id == id);
            if (User == null)
            {
                return NotFound();
            }

           

            if (User.IsActive)
            {
                User.IsActive = false;
            }
            else
            {
                User.IsActive = true;
            }

            _context.SaveChanges();
            return RedirectToAction("Index", "Users", new { area = "Admin" });
        }


        [HttpGet]
        public IActionResult Delete(string? id)
        {
            var User = _context.ApplicationUsers.FirstOrDefault(x => x.Id == id);
            if (User == null)
            {
                return NotFound();
            }

            _context.ApplicationUsers.Remove(User);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
