using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using World_Shopping.DataAccess;
using World_Shopping.Models.Repositories;
using World_Shopping.Web.Entities.Models;



namespace World_Shopping.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        //Inject  ApplicationDbContext
        private IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
       
        //Retrieve data from database
        public IActionResult Index()
        {
            var categories = _unitOfWork.Category.GetAll();
            return View(categories);
        }
        
        //Take Data From the User To the View
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        
        //Add Data To the Database
        [HttpPost]
        [ValidateAntiForgeryToken] //Protection From Hackers
        public IActionResult Create(Category category)
        {
            //Data Validation Server Side 
            if (ModelState.IsValid)
            {
               
                _unitOfWork.Category.Add(category);
              
                _unitOfWork.complete();
                TempData["Create"] = "Item Has Created Successfully";
                return RedirectToAction("Index");
            }
            return View(category);
        }

        //Enter Data From the User
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null | id == 0)
            {
                NotFound();
            }
            //var categoryIndb = _context.Categories.Find(id);
            var categoryIndb = _unitOfWork.Category.GetFirstorDafault(x => x.Id == id, null);
            return View(categoryIndb);
        }
        //Save the Modification in the Database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                //_context.Categories.Update(category);
                _unitOfWork.Category.Update(category);
                //_context.SaveChanges();
                _unitOfWork.complete();
                TempData["Update"] = "Item Has Updated Successfully";
                return RedirectToAction("Index");
            }
            return View(category);
        }
        
        //Delete Data From Database
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null | id == 0)
            {
                NotFound();
            }
            var categoryIndb = _unitOfWork.Category.GetFirstorDafault(x => x.Id == id, null);
            return View(categoryIndb);
        }

        [HttpPost]
        public IActionResult DeleteCategory(int? id)
        {
            var categoryIndb = _unitOfWork.Category.GetFirstorDafault(x => x.Id == id, null);
            if (categoryIndb == null)
            {
                NotFound();
            }
            //_context.Categories.Remove(categoryIndb);
            _unitOfWork.Category.Remove(categoryIndb);
            //_context.SaveChanges();
            _unitOfWork.complete();
            TempData["Delete"] = "Item Has Deleted Successfully";
            return RedirectToAction("Index");

        }
    }
}
