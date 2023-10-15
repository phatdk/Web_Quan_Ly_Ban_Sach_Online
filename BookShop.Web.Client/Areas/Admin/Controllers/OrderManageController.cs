using BookShop.BLL.ConfigurationModel.OrderModel;
using BookShop.BLL.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Web.Client.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class OrderManageController : Controller
	{
		private List<OrderViewModel> _orders;
		private readonly IOrderService _orderService;
		private readonly IOrderDetailService _orderDetailService;
		private readonly IStatusOrderService _statusOrderService;
		private readonly IPromotionService _promotionService;
		private readonly IProductService _productService;

		public OrderManageController(IOrderService orderService, IOrderDetailService orderDetailService, IStatusOrderService statusOrderService, IPromotionService promotionService, IProductService productService)
		{
			_orders = new List<OrderViewModel>();
			_orderService = orderService;
			_orderDetailService = orderDetailService;
			_statusOrderService = statusOrderService;
			_promotionService = promotionService;
			_productService = productService;
		}

		// GET: OrderManageController
		public async Task<IActionResult> Index()
		{
			_orders = await _orderService.GetAll();
			ViewBag.Orders = _orders;
			return View();
		}

		// GET: OrderManageController/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}

		// GET: OrderManageController/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: OrderManageController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: OrderManageController/Edit/5
		public ActionResult Edit(int id)
		{
			return View();
		}

		// POST: OrderManageController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: OrderManageController/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: OrderManageController/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}
	}
}
