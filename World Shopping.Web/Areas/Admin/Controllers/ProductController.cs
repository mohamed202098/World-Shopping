using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify.Helpers;
using System.Text.Json;
using System.Web.WebPages.Html;
using World_Shopping.DataAccess;
using World_Shopping.Models.Models;
using World_Shopping.Models.Repositories;
using World_Shopping.Models.ViewModels;
using World_Shopping.Web.Entities.Models;



namespace World_Shopping.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        //Inject  ApplicationDbContext
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        //Retrieve data from database
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetData()
        {
            var Products = _unitOfWork.Product.GetAll(Includeword: "Category");
            var res = Products;

            return Json(data: res);
            
        }

        //Take Data From the User To the View
        [HttpGet]
        public IActionResult Create()
        {

            var categoryList = _unitOfWork.Category.GetAll().Select(x => new Category
            {
                Id = x.Id,
                Name = x.Name.ToString()
            }).ToList();

            ViewBag.CategoryList = new SelectList(categoryList, "Id", "Name");

            return View();
        }
        
        //Add Data To the Database
        [HttpPost]
        [ValidateAntiForgeryToken] //Protection From Hackers
        public IActionResult Create(ProductVM productVM, IFormFile file)
        {

            //Data Validation Server Side 
            if (ModelState.IsValid)
            {
                string RootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString();
                    var Upload = Path.Combine(RootPath, @"Images\Products");
                    var ext = Path.GetExtension(file.FileName);
                    var ImageName = Guid.NewGuid().ToString() + ext;

                    using (var fileStream = new FileStream(Path.Combine(Upload, ImageName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVM.Product.Image = ImageName;
                }

                //_context.Categories.Add(product);
                _unitOfWork.Product.Add(productVM.Product);
                //_context.SaveChanges();
                _unitOfWork.complete();
                TempData["Create"] = "Data Has Created Successfully";
                return RedirectToAction("Index");
            }
            return View(productVM);
        }

        //Enter Data From the User
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null | id == 0)
            {
                NotFound();
            }
            //var productIndb = _context.Categories.Find(id);
            var productIndb = _unitOfWork.Product.GetFirstorDafault(x => x.Id == id, null);
           
            var categoryList = _unitOfWork.Category.GetAll().Select(x => new Category
            {
                Id = x.Id,
                Name = x.Name.ToString()
            }).ToList();

            ViewBag.CategoryList = new SelectList(categoryList, "Id", "Name");

            var productVM = new ProductVM
            {
                Product = productIndb,
                CategoryList = null
            };

            ViewBag.ImagePath = "/Images/Products/";
            return View(productVM);
           
        }
        //Save the Modification in the Database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductVM productVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string RootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    var Upload = Path.Combine(RootPath, @"Images\Products");
                    var ext = Path.GetExtension(file.FileName);
                    var ImageName = Guid.NewGuid().ToString() + ext;

                    if (productVM.Product.Image != null)
                    {
                        var oladImage = Path.Combine(RootPath, productVM.Product.Image.TrimStart('\\'));
                        if (System.IO.File.Exists(oladImage))
                        {
                            System.IO.File.Delete(oladImage);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(Upload, ImageName + ext), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVM.Product.Image = ImageName;
                }

                //_context.Categories.Update(product);
                _unitOfWork.Product.Update(productVM.Product);
                //_context.SaveChanges();
                _unitOfWork.complete();
                TempData["Update"] = "Data Has Updated Successfully";
                return RedirectToAction("Index");
            }
            return View(productVM.Product);
        }
     
        [HttpDelete]
        public IActionResult DeleteProduct(int? id)
        {
            var productIndb = _unitOfWork.Product.GetFirstorDafault(x => x.Id == id, null);
            if (productIndb == null)
            {
                return Json(new { success = false, Message = "Error While Deleting" });
            }
            _unitOfWork.Product.Remove(productIndb);
            var oladImage = Path.Combine(_webHostEnvironment.WebRootPath, productIndb.Image.TrimStart('\\'));
            if (System.IO.File.Exists(oladImage))
            {
                System.IO.File.Delete(oladImage);
            }
            _unitOfWork.complete();
            TempData["Delete"] = "Data Has Deleted Successfully";
            return Json(new { success = true, Message = "File Has been Deleted" });
           

        }
    }
}
