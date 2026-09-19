using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using World_Shopping.DataAccess.Implementation;
using World_Shopping.Models.Models;
using World_Shopping.Models.Repositories;
using World_Shopping.Models.ViewModels;
using World_Shopping.Utilities;

namespace World_Shopping.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles = SD.AdminRole)]
    public class OrderController : Controller
    {
      private readonly IUnitOfWork _unitOfWork;

      [BindProperty]
      public OrderVM OrderVM { get; set; }

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetData()
        {
            IEnumerable<OrderHeader> orderHeaders;
            orderHeaders = _unitOfWork.OrderHeader.GetAll(Includeword: "ApplicationUser");
            var res = orderHeaders;

            return Json(data: res);
        }


		public IActionResult Details(int id)
		{
			OrderVM orderVM = new OrderVM()
			{
				OrderHeader = _unitOfWork.OrderHeader.GetFirstorDafault(u => u.Id == id, Includeword: "ApplicationUser"),
				OrderDetails = _unitOfWork.OrderDetail.GetAll(x => x.OrderHeaderId == id, Includeword: "Product")
			};

			return View(orderVM);
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
		public IActionResult UpdateOrderDetails()
		{
            var OrderFromDb = _unitOfWork.OrderHeader.GetFirstorDafault(u => u.Id == OrderVM.OrderHeader.Id, "");
			OrderFromDb.Name = OrderVM.OrderHeader.Name;
			OrderFromDb.Phone = OrderVM.OrderHeader.Phone;
			OrderFromDb.Address = OrderVM.OrderHeader.Address;
			OrderFromDb.City = OrderVM.OrderHeader.City;

            if (OrderVM.OrderHeader.Carrier != null)
            {
                OrderFromDb.Carrier = OrderVM.OrderHeader.Carrier;

			}

			if (OrderVM.OrderHeader.TrackingNumber != null)
			{
				OrderFromDb.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;
			}

            _unitOfWork.OrderHeader.Update(OrderFromDb);
            _unitOfWork.complete();

			TempData["Update"] = "Data Has Updted Succesfully";
			return RedirectToAction("Details","Order", new {id = OrderFromDb.Id });
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult StartProccess()
		{

			_unitOfWork.OrderHeader.UpdateStatus(OrderVM.OrderHeader.Id, SD.Proccessing, null);
			_unitOfWork.complete();

			TempData["Update"] = "Order Status Has Updted Succesfully";
			return RedirectToAction("Details", "Order", new { id = OrderVM.OrderHeader.Id });
		}



		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult StartShip()
		{

			var OrderFromDb = _unitOfWork.OrderHeader.GetFirstorDafault(u => u.Id == OrderVM.OrderHeader.Id, "");
			OrderFromDb.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;
			OrderFromDb.Carrier = OrderVM.OrderHeader.Carrier;
			OrderFromDb.OrderStatus = SD.Shipped;
			OrderFromDb.ShoppingDate = DateTime.Now;

			_unitOfWork.OrderHeader.Update(OrderFromDb);
			_unitOfWork.complete();


			TempData["Update"] = "Order Has Shipped Succesfully";
			return RedirectToAction("Details", "Order", new { id = OrderVM.OrderHeader.Id });
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult CancelOrder()
		{
			var OrderFromDb = _unitOfWork.OrderHeader.GetFirstorDafault(u => u.Id == OrderVM.OrderHeader.Id, "");

			if (OrderFromDb.PaymentStatus == SD.Approve)
			{
				var option = new RefundCreateOptions
				{
					Reason = RefundReasons.RequestedByCustomer,
					PaymentIntent = OrderFromDb.PaymentIntenId,
				};

				var Service = new RefundService();
				Refund refund = Service.Create(option);

				_unitOfWork.OrderHeader.UpdateStatus(OrderFromDb.Id, SD.Cancelled, SD.Refund);
			}
			else
			{
				_unitOfWork.OrderHeader.UpdateStatus(OrderFromDb.Id, SD.Cancelled, SD.Cancelled);
			}

			_unitOfWork.complete();
			

			TempData["Update"] = "Order Has Cancelled Succesfully";
			return RedirectToAction("Details", "Order", new { id = OrderVM.OrderHeader.Id });
		}
	}
}
