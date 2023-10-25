using BookShop.BLL.ConfigurationModel.OrderModel;
using BookShop.BLL.IService;
using BookShop.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Web.Client.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class OrderManageController : Controller
	{
		private List<OrderViewModel> _orders;
		private OrderViewModel _order;
		private readonly UserManager<Userr> _userManager;
		private readonly IOrderService _orderService;
		private readonly IOrderDetailService _orderDetailService;
		private readonly IStatusOrderService _statusOrderService;
		private readonly IPromotionService _promotionService;
		private readonly IProductService _productService;

		public OrderManageController(UserManager<Userr> userManager, IOrderService orderService, IOrderDetailService orderDetailService, IStatusOrderService statusOrderService, IPromotionService promotionService, IProductService productService)
		{
			_orders = new List<OrderViewModel>();
			_order = new OrderViewModel();
			_userManager = userManager;
			_orderService = orderService;
			_orderDetailService = orderDetailService;
			_statusOrderService = statusOrderService;
			_promotionService = promotionService;
			_productService = productService;
		}

		private Task<Userr> GetCurrentUserAsync()
		{
			return _userManager.GetUserAsync(HttpContext.User);
		}

		// GET: OrderManageController
		public async Task<IActionResult> Index()
		{
			_orders = await _orderService.GetAll();
			ViewBag.Orders = _orders;
			return View();
		}

		// GET: OrderManageController/Details/5
		public async Task<IActionResult> Details(int id)
		{
			_order = await _orderService.GetById(id);
			var details = await _orderDetailService.GetByOrder(id);
			ViewBag.Details = details;
			foreach (var item in details)
			{
				_order.Total += item.Quantity * item.Price;
			}
			if (_order.Id_Promotion != null)
			{
				var promotion = await _promotionService.GetById(Convert.ToInt32(_order.Id_Promotion));
				if (promotion.PercentReduct != null)
				{
					_order.Total -= Convert.ToInt32(Math.Floor(Convert.ToDouble((_order.Total / 100) * promotion.PercentReduct)));
				}
				else _order.Total -= Convert.ToInt32(promotion.AmountReduct);
			}
			if (_order.IsUsePoint)
			{
				_order.Total -= Convert.ToInt32(_order.PointAmount);
			}
			return View(_order);
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
		public async Task<IActionResult> Edit(int id)
		{
			_order = await _orderService.GetById(id);
			return View(_order);
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

		// POST: OrderManageController/Accept/5
		public async Task<IActionResult> AcceptOrder(int id)
		{
			var order = await _orderService.GetById(id);
			var staff = await GetCurrentUserAsync();
			if (staff != null)
			{
				if (order.Status == 1)
				{
					var statusId = (await _statusOrderService.GetAll()).Where(x => x.Status == 2).First().Id;
					order.Id_Status = statusId;
					order.AcceptDate = DateTime.Now;
					order.Id_Staff = staff.Id;
					var result = await _orderService.Update(order);
					// trừ sách trong kho
					return Json(new { success = result });
				}
				return Json(new { success = false });
			}
			else return RedirectToAction("Login", "Account", new { area = "Identity" });
		}

		// POST: DeliOrder
		public async Task<IActionResult> DeliveryOrder(int id)
		{
			var order = await _orderService.GetById(id);
			var staff = await GetCurrentUserAsync();
			if (staff != null)
			{
				if (order.Status == 2)
				{
					var statusId = (await _statusOrderService.GetAll()).Where(x => x.Status == 3).First().Id;
					order.Id_Status = statusId;
					order.DeliveryDate = DateTime.Now;
					var result = await _orderService.Update(order);
					return Json(new { success = result });
				}
				return Json(new { success = false });
			}
			else return RedirectToAction("Login", "Account", new { ares = "Identity" });
		}

		// POST: SuccessOrder
		public async Task<IActionResult> SuccessOrder(int id)
		{
			var order = await _orderService.GetById(id);
			var staff = await GetCurrentUserAsync();
			if (staff != null)
			{
				if (order.Status == 3)
				{
					var statusId = (await _statusOrderService.GetAll()).Where(x => x.Status == 4).First().Id;
					order.Id_Status = statusId;
					order.CompleteDate = DateTime.Now;
					order.ReceiveDate = DateTime.Now;
					if (order.PaymentDate == null)
					{
						order.PaymentDate = DateTime.Now;
						// đổi trạng thái payment order
					}
					var result = await _orderService.Update(order);
					return Json(new { success = result });
				}
				return Json(new { success = false });
			}
			else return RedirectToAction("Login", "Account", new { ares = "Identity" });
		}

		// ReturnOrder
		public async Task<IActionResult> ReturnOrder(int id, string modifyChange)
		{
			var order = await _orderService.GetById(id);
			var staff = await GetCurrentUserAsync();
			if (staff != null)
			{
				var statusId = (await _statusOrderService.GetAll()).Where(x => x.Status == 5).First().Id;
				order.Id_Status = statusId;
				order.ModifiDate = DateTime.Now;
				order.ModifiNotes = order.ModifiNotes == null ? "" : order.ModifiNotes;
				order.ModifiNotes = order.ModifiNotes + "\n" + DateTime.Now + " : " + modifyChange;
				var result = await _orderService.Update(order);
				return Json(new { success = result });
			}
			else return RedirectToAction("Login", "Account", new { ares = "Identity" });
		}

		// handleReturn
		public async Task<IActionResult> HandleReturn(int id)
		{
			var order = await _orderService.GetById(id);
			var staff = await GetCurrentUserAsync();
			if (staff != null)
			{
				if (order.Id_Status == 5)
				{
					var statusId = (await _statusOrderService.GetAll()).Where(x => x.Status == 6).First().Id;
					order.Id_Status = statusId;
					order.ModifiDate = DateTime.Now;
					order.ModifiNotes = order.ModifiNotes == null ? "" : order.ModifiNotes;
					order.ModifiNotes = order.ModifiNotes + "\n" + DateTime.Now + " : " + "Hàng được trả về đến nơi, chờ xử lý";
					var result = await _orderService.Update(order);
					return Json(new { success = result });
				}
				return Json(new { success = false });
			}
			else return RedirectToAction("Login", "Account", new { ares = "Identity" });
		}

		// SuccessReturn
		public async Task<IActionResult> SuccessReturn(int id, string modifyChange)
		{
			var order = await _orderService.GetById(id);
			var staff = await GetCurrentUserAsync();
			if (staff != null)
			{
				if (order.Id_Status == 5)
				{
					var statusId = (await _statusOrderService.GetAll()).Where(x => x.Status == 6).First().Id;
					order.Id_Status = statusId;
					order.ModifiDate = DateTime.Now;
					order.ModifiNotes = order.ModifiNotes == null ? "" : order.ModifiNotes;
					order.ModifiNotes = order.ModifiNotes + "\n" + DateTime.Now + " : " + modifyChange;
					var result = await _orderService.Update(order);
					return Json(new { success = result });
				}
				return Json(new { success = false });
			}
			else return RedirectToAction("Login", "Account", new { ares = "Identity" });
		}

		// CancelOrder
		public async Task<IActionResult> CancelOrder(int id, string modifyChange)
		{
			var order = await _orderService.GetById(id);
			var staff = await GetCurrentUserAsync();
			if (staff != null)
			{
				var statusId = (await _statusOrderService.GetAll()).Where(x => x.Status == 6).First().Id;
				order.Id_Status = statusId;
				order.ModifiDate = DateTime.Now;
				order.ModifiNotes = order.ModifiNotes == null ? "" : order.ModifiNotes;
				order.ModifiNotes = order.ModifiNotes + "\n" + DateTime.Now + " : " + modifyChange;
				var result = await _orderService.Update(order);
				return Json(new { success = result });
			}
			else return RedirectToAction("Login", "Account", new { ares = "Identity" });
		}

		// CloseOrder
		public async Task<IActionResult> CloseOrder(int id)
		{
			var order = await _orderService.GetById(id);
			var staff = await GetCurrentUserAsync();
			if (staff != null)
			{
				if (order.Id_Status == 5)
				{
					var statusId = (await _statusOrderService.GetAll()).Where(x => x.Status == 6).First().Id;
					order.Id_Status = statusId;
					order.ModifiDate = Convert.ToDateTime(order.CompleteDate).AddDays(7);
					order.ModifiNotes = order.ModifiNotes == null ? "" : order.ModifiNotes;
					order.ModifiNotes = order.ModifiNotes + "\n" + DateTime.Now + " : " + "Đơn hàng đã đóng";
					var result = await _orderService.Update(order);
					return Json(new { success = result });
				}
				return Json(new { success = false });
			}
			else return RedirectToAction("Login", "Account", new { ares = "Identity" });
		}

	}
}
