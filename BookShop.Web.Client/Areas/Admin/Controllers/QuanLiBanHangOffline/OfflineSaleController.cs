using BookShop.BLL.IService;
using BookShop.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Web.Client.Areas.Admin.Controllers.QuanLiBanHangOffline
{
	[Area("Admin")]
	public class OfflineSaleController : Controller
	{

		private readonly IOrderService _orderService;
		private readonly IOrderDetailService _orderDetailService;
		private readonly IProductService _productService;
		private readonly IProductBookService _productBookService;
		private readonly IUserService _userService;
		private readonly UserManager<Userr> _userManager;

		public OfflineSaleController(IOrderService orderService, IOrderDetailService orderDetailService, IProductService productService, IProductBookService productBookService, IUserService userService, UserManager<Userr> userManager)
		{
			_orderService = orderService;
			_orderDetailService = orderDetailService;
			_productService = productService;
			_productBookService = productBookService;
			_userService = userService;
			_userManager = userManager;
		}

		// GET: OfflineSaleController
		public ActionResult Index()
		{
			return View();
		}

		// GET: OfflineSaleController/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}

		// GET: OfflineSaleController/Create
		public async Task<IActionResult> CreateOfflineOrder()
		{
			var user = await _userManager.GetUserAsync(HttpContext.User);
			if (user != null) return View();
			return RedirectToAction("Login", "Account", new {Areas = "Identity"});
		}

		// POST: OfflineSaleController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult CreateOfflineOrder(IFormCollection collection)
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

		// GET: OfflineSaleController/Edit/5
		public ActionResult Edit(int id)
		{
			return View();
		}

		// POST: OfflineSaleController/Edit/5
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

		// GET: OfflineSaleController/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: OfflineSaleController/Delete/5
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
