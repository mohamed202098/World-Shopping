using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using World_Shopping.Models.Models;
using World_Shopping.Models.Repositories;
using World_Shopping.Utilities;
using X.PagedList;

namespace World_Shopping.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(int ? Page)
        {
            var PageNumber = Page ?? 1;
            int PageSize = 8;


            var products = _unitOfWork.Product.GetAll().ToPagedList(PageNumber, PageSize);
            ViewBag.ImagePath = "/Images/Products/";
            return View(products);
        }

        public IActionResult Details(int id)
        {
            ShoppingCart Object = new ShoppingCart()
            {
                Product = _unitOfWork.Product.GetFirstorDafault(v => v.Id == id, Includeword: "Category"),
                Count = 1
            };

            ViewBag.ImagePath = "/Images/Products/";
            return View(Object);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult AddToCard(int id)
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCart cartObject = _unitOfWork.ShoppingCart.GetFirstorDafault(
                u => u.ApplicationUserId == claim.Value && u.ProductId == id, "");

            if (cartObject == null)
            {
                var shoppingCart = new ShoppingCart();
                shoppingCart.ProductId = id;
                shoppingCart.ApplicationUserId = claim.Value;
                shoppingCart.Count = 1;
                _unitOfWork.ShoppingCart.Add(shoppingCart);
                _unitOfWork.complete();

                HttpContext.Session.SetInt32(SD.SessionKey,
                    _unitOfWork.ShoppingCart.GetAll(x => x.ApplicationUserId == claim.Value).ToList().Count() 
                );
               
            }
            else
            {
                _unitOfWork.ShoppingCart.IncreaseCount(cartObject, 1);
                _unitOfWork.complete();
            }

           

            return RedirectToAction("Index");
        }
    }
}
