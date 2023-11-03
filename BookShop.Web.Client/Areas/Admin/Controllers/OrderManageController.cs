using BookShop.BLL.ConfigurationModel.OrderModel;
using BookShop.BLL.ConfigurationModel.OrderPaymentModel;
using BookShop.BLL.IService;
using BookShop.DAL.Entities;
using BookShop.Web.Client.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;

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
		private readonly IOrderPaymentService _orderPaymentService;
		private readonly IStatusOrderService _statusOrderService;
		private readonly IPromotionService _promotionService;
		private readonly IProductService _productService;
		private readonly IProductBookService _productBookService;
		private readonly IBookService _bookService;
		private readonly ProductPreviewService _productPreviewService;
		private readonly AccumulatePointService _accumulatePointService;

		public OrderManageController(UserManager<Userr> userManager, IOrderService orderService, IOrderDetailService orderDetailService, IOrderPaymentService orderPaymentService, IStatusOrderService statusOrderService, IPromotionService promotionService, IProductService productService, IProductBookService productBookService, IBookService bookService)
		{
			_orders = new List<OrderViewModel>();
			_order = new OrderViewModel();
			_userManager = userManager;
			_orderService = orderService;
			_orderDetailService = orderDetailService;
			_orderPaymentService = orderPaymentService;
			_statusOrderService = statusOrderService;
			_promotionService = promotionService;
			_productService = productService;
			_productBookService = productBookService;
			_bookService = bookService;

			_productPreviewService = new ProductPreviewService();
			_accumulatePointService = new AccumulatePointService();
		}

		private Task<Userr> GetCurrentUserAsync()
		{
			return _userManager.GetUserAsync(HttpContext.User);
		}

		public async Task<IActionResult> GetOrder(int? status, string? keyWord)
		{

			if (status != null)
			{
				_orders = (await _orderService.GetAll()).Where(
					x => x.Status == Convert.ToInt32(status)
					).ToList();
			}
			else
			{
				_orders = await _orderService.GetAll();
			}
			if (!string.IsNullOrEmpty(keyWord))
			{
				_orders = _orders.Where(
					x => x.Code.ToLower().Contains(keyWord.ToLower())
					|| x.UserCode.ToLower().Contains(keyWord.ToLower())
					|| x.StaffCode.ToLower().Contains(keyWord.ToLower())
					|| x.NameUser.ToLower().Contains(keyWord.ToLower())
					|| x.NameStaff.ToLower().Contains(keyWord.ToLower())
					).ToList();
			}
			var orders = _orders.OrderByDescending(x => x.CreatedDate).ToList();
			return Json(orders);
		}
		public async Task<IActionResult> GetDetails(int id)
		{
			var result = await _orderDetailService.GetByOrder(id);
			return Json(result);
		}
		// GET: OrderManageController
		public IActionResult Index()
		{
			ViewBag.Filter = "1";
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
			if (staff != null && order != null)
			{
				// tru so luong sach
				var details = await _orderDetailService.GetByOrder(order.Id);
				foreach (var item in details)
				{
					var detailProducts = await _productBookService.GetByProduct(item.Id_Product);
					foreach (var item1 in detailProducts)
					{
						var book = await _bookService.GetById(item1.Id_Book);
						if (book.Quantity >= item.Quantity)
						{
							await _bookService.ChangeQuantity(book.Id, -item.Quantity); // giam so luong sach trong kho
						}
						else return Json(new { success = false, errorMessage = "\nSố lượng sách trong kho không đủ!" });
					}
				}
				if (order.Status == 1)
				{
					var statusId = (await _statusOrderService.GetAll()).Where(x => x.Status == 2).First().Id;
					order.Id_Status = statusId;
					order.AcceptDate = DateTime.Now;
					order.ModifiDate = DateTime.Now;
					order.ModifiNotes = order.ModifiNotes + DateTime.Now + " : Đơn được xác nhận bởi " + staff.Name + " Mã code [" + staff.Code + "]\n";
					order.Id_Staff = staff.Id;
					var result = await _orderService.Update(order);
					return Json(new { success = result });
				}
				return Json(new { success = false, errorMessage = "\nTrạng thái đơn hàng không hợp lệ!" });
			}
			else return Json(new { success = false, errorMessage = "\nBạn chưa đăng nhập hoặc đơn hàng không được tìm thấy! \nVui lòng kiểm tra lại!" });
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
					order.ModifiDate = DateTime.Now;
					order.ModifiNotes = order.ModifiNotes + "\n" + DateTime.Now + " : Đơn được xác nhận giao bởi " + staff.Name + " Mã code [" + staff.Code + "]\n";
					var result = await _orderService.Update(order);
					return Json(new { success = result });
				}
				return Json(new { success = false, errorMessage = "\nTrạng thái đơn hàng không hợp lệ!" });
			}
			else return Json(new { success = false, errorMessage = "\nBạn chưa đăng nhập hoặc đơn hàng không được tìm thấy! \nVui lòng kiểm tra lại!" });
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
					order.ModifiDate = DateTime.Now;
					order.ModifiNotes = order.ModifiNotes + "\n" + DateTime.Now + " : Đơn được xác nhận hoàn thành bởi " + staff.Name + " Mã code [" + staff.Code + "]\n";
					if (order.PaymentDate == null)
					{
						order.PaymentDate = DateTime.Now;
						var paymentMethods = await _orderPaymentService.GetByOrder(id);
						foreach (var item in paymentMethods)
						{
							var obj = new UpdateOrderPaymentModel()
							{
								Id_Payment = item.Id_Payment,
								Status = 1,
								PaymentAmount = item.paymentAmount,
							};
							await _orderPaymentService.Update(item.Id, obj);
						}
					}
					// cộng điểm vào ví điểm
					var details = await _orderDetailService.GetByOrder(order.Id);
					foreach (var item in details)
					{
						order.Total += item.Price * item.Quantity;
					}
					int point = Convert.ToInt32(Math.Floor(Convert.ToDouble(order.Total / 20000))); // 20k = 1 điểm
					if (point > 0)
					{
						await _accumulatePointService.Accumulate(order.Id_User, point);
					}
					var result = await _orderService.Update(order);
					return Json(new { success = result });
				}
				return Json(new { success = false, errorMessage = "\nTrạng thái đơn hàng không hợp lệ!" });
			}
			else return Json(new { success = false, errorMessage = "\nBạn chưa đăng nhập hoặc đơn hàng không được tìm thấy! \nVui lòng kiểm tra lại!" });
		}

		// ReturnOrder
		public async Task<IActionResult> ReturnOrder(int id, string modifyChange)
		{
			var order = await _orderService.GetById(id);
			var staff = await GetCurrentUserAsync();
			if (staff != null)
			{
				if (order.Status == 3)
				{
					var statusId = (await _statusOrderService.GetAll()).Where(x => x.Status == 5).First().Id;
					order.Id_Status = statusId;
					order.ModifiDate = DateTime.Now;
					order.ModifiNotes = order.ModifiNotes + "\n" + DateTime.Now + " : Đơn được xác nhận yêu cầu trả hàng bởi" + staff.Name + " Mã code [" + staff.Code + "\n Ghi chú: " + modifyChange + "\n";
					var result = await _orderService.Update(order);
					return Json(new { success = result });
				}
				return Json(new { success = false, errorMessage = "\nTrạng thái đơn hàng không hợp lệ!" });
			}
			else return Json(new { success = false, errorMessage = "\nBạn chưa đăng nhập hoặc đơn hàng không được tìm thấy! \nVui lòng kiểm tra lại!" });
		}

		// handleReturn trang thai chờ
		public async Task<IActionResult> WaitHandleReturn(int id)
		{
			var order = await _orderService.GetById(id);
			var staff = await GetCurrentUserAsync();
			if (staff != null)
			{
				if (order.Status == 5)
				{
					var statusId = (await _statusOrderService.GetAll()).Where(x => x.Status == 6).First().Id;
					order.Id_Status = statusId;
					order.ModifiDate = DateTime.Now;
					order.ModifiNotes = order.ModifiNotes + "\n" + DateTime.Now + " : Đơn được xác nhận đã hoàn trả và chờ xử lý hoàn trả bởi " + staff.Name + " Mã code [" + staff.Code + "\n";
					var result = await _orderService.Update(order);
					return Json(new { success = result });
				}
				return Json(new { success = false, errorMessage = "\nTrạng thái đơn hàng không hợp lệ!" });
			}
			else return Json(new { success = false, errorMessage = "\nBạn chưa đăng nhập hoặc đơn hàng không được tìm thấy! \nVui lòng kiểm tra lại!" });
		}

		// quyết định xử lý hàng trả
		public async Task<bool> HandleReturn(int id)
		{
			var detail = await _orderDetailService.GetById(id);
			if (detail != null)
			{
				var detailProducts = await _productBookService.GetByProduct(detail.Id_Product);
				foreach (var item1 in detailProducts)
				{
					var book = await _bookService.GetById(item1.Id_Book);
					await _bookService.ChangeQuantity(book.Id, detail.Quantity);
				}
				return true;
			}
			return false;
		}

		// SuccessReturn
		public async Task<IActionResult> SuccessReturn(int id, string modifyChange, Dictionary<int, int> myAction)
		{
			// return Json(new {id = id, message = modifyChange, list = myAction});
			var order = await _orderService.GetById(id);
			var staff = await GetCurrentUserAsync();
			if (staff != null)
			{
				if (order.Status == 6)
				{
					var statusId = (await _statusOrderService.GetAll()).Where(x => x.Status == 7).First().Id;
					order.Id_Status = statusId;
					order.ModifiDate = DateTime.Now;
					order.ModifiNotes = order.ModifiNotes + "\n" + DateTime.Now + " : Đơn được xác nhận hoàn thành xử lý hàng hoàn trả bởi " + staff.Name + " Mã code [" + staff.Code + "\n Ghi chú: " + modifyChange + "\n";
					var result = await _orderService.Update(order);
					if (result && order.Status == 6)
					{
						foreach (var action in myAction)
						{
							if (action.Value == 1)
							{
								await HandleReturn(action.Key);
							}
						}
					}
					return Json(new { success = result });
				}
				return Json(new { success = false, errorMessage = "\nTrạng thái đơn hàng không hợp lệ!" });
			}
			else return Json(new { success = false, errorMessage = "\nBạn chưa đăng nhập hoặc đơn hàng không được tìm thấy! \nVui lòng kiểm tra lại!" });
		}

		// CancelOrder
		public async Task<IActionResult> CancelOrder(int id, string modifyChange)
		{
			var order = await _orderService.GetById(id);
			var staff = await GetCurrentUserAsync();
			if (staff != null && order.Status <= 2)
			{
				var statusId = (await _statusOrderService.GetAll()).Where(x => x.Status == 6).First().Id;
				order.Id_Status = statusId;
				order.ModifiDate = DateTime.Now;
				order.ModifiNotes = order.ModifiNotes + "\n" + DateTime.Now + " : Đơn được xác nhận yêu cầu hủy bởi" + staff.Name + " Mã code [" + staff.Code + "\n Ghi chú: " + modifyChange + "\n";
				var result = await _orderService.Update(order);
				if (result)
				{
					if (order.Status == 2)
					{// tang lai so luong sach khi đơn chỉ mới xác nhận và chưa giao
						var details = await _orderDetailService.GetByOrder(order.Id);
						foreach (var item in details)
						{
							var detailProducts = await _productBookService.GetByProduct(item.Id_Product);
							foreach (var item1 in detailProducts)
							{
								var book = await _bookService.GetById(item1.Id_Book);
								await _bookService.ChangeQuantity(book.Id, item.Quantity); // tang so luong sach trong kho
							}
							await _productPreviewService.ChangeQuantity(item.Id_Product, item.Quantity);	// tang lai sp
						}
					}
				}
				return Json(new { success = result });
			}
			else return Json(new { success = false, errorMessage = "\nBạn chưa đăng nhập hoặc đơn hàng không được tìm thấy! \nVui lòng kiểm tra lại!" });
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
					order.ModifiDate = DateTime.Now;
					order.ModifiNotes = order.ModifiNotes + "\n" + DateTime.Now + " : Đơn được xác nhận đóng bởi" + staff.Name + " Mã code [" + staff.Code + "]\n Ngày đóng đơn : " + Convert.ToDateTime(order.CompleteDate).AddDays(3);
					var result = await _orderService.Update(order);
					return Json(new { success = result });
				}
				return Json(new { success = false, errorMessage = "\nTrạng thái đơn hàng không hợp lệ!" });
			}
			else return Json(new { success = false, errorMessage = "\nBạn chưa đăng nhập hoặc đơn hàng không được tìm thấy! \nVui lòng kiểm tra lại!" });
		}

	}
}
