﻿using BookShop.BLL.ConfigurationModel.OrderDetailModel;
using BookShop.BLL.ConfigurationModel.OrderModel;
using BookShop.BLL.ConfigurationModel.OrderPaymentModel;
using BookShop.BLL.ConfigurationModel.PointTranHistoryModel;
using BookShop.BLL.ConfigurationModel.ReturnOrderModel;
using BookShop.BLL.IService;
using BookShop.DAL.Entities;
using BookShop.Web.Client.Models;
using BookShop.Web.Client.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using System;

namespace BookShop.Web.Client.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class OrderManageController : Controller
	{
		private List<OrderViewModel> _orders;
		private OrderViewModel _order;
		private readonly UserManager<Userr> _userManager;
		private readonly IUserService _userService;
		private readonly IOrderService _orderService;
		private readonly IOrderDetailService _orderDetailService;
		private readonly IOrderPaymentService _orderPaymentService;
		private readonly IOrderPromotionService _orderPromotionService;
		private readonly IStatusOrderService _statusOrderService;
		private readonly IReturnOrderService _returnOrderService;
		private readonly IPromotionService _promotionService;
		private readonly IProductService _productService;
		private readonly IProductBookService _productBookService;
		private readonly IBookService _bookService;
		private readonly ProductPreviewService _productPreviewService;
		private readonly PointNPromotionSerVice _pointNPromotionService;

		public OrderManageController(UserManager<Userr> userManager, IUserService userService, IOrderService orderService, IOrderDetailService orderDetailService, IOrderPaymentService orderPaymentService, IStatusOrderService statusOrderService, IReturnOrderService returnOrderService, IPromotionService promotionService, IProductService productService, IProductBookService productBookService, IBookService bookService, IOrderPromotionService orderPromotionService)
		{
			_orders = new List<OrderViewModel>();
			_order = new OrderViewModel();
			_userManager = userManager;
			_userService = userService;
			_orderService = orderService;
			_orderDetailService = orderDetailService;
			_orderPromotionService = orderPromotionService;
			_orderPaymentService = orderPaymentService;
			_statusOrderService = statusOrderService;
			_returnOrderService = returnOrderService;
			_promotionService = promotionService;
			_productService = productService;
			_productBookService = productBookService;
			_bookService = bookService;

			_productPreviewService = new ProductPreviewService();
			_pointNPromotionService = new PointNPromotionSerVice();
		}

		private Task<Userr> GetCurrentUserAsync()
		{
			return _userManager.GetUserAsync(HttpContext.User);
		}

		public async Task<IActionResult> GetDetails(int id)
		{
			var result = await _orderDetailService.GetByOrder(id);
			return Json(result);
		}
		// GET: OrderManageController
		public async Task<IActionResult> Index()
		{
			return View();
		}

		public async Task<IActionResult> GetOrder(int page, int? status, int? type, string? keyWord)
		{
			_orders = await _orderService.GetAll();
			if (status != null)
			{
				_orders = _orders.Where(
					x => x.Status == Convert.ToInt32(status)
					).ToList();
			}
			if(type != null)
			{
				if (type == 1) _orders = _orders.Where(x => x.IsOnlineOrder == true).ToList();
				else if (type == 0) _orders = _orders.Where(x=>x.IsOnlineOrder == false).ToList();
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
			int pageSize = 15;
			double totalPage = (double)orders.Count / pageSize;
			orders = orders.Skip((page - 1) * pageSize).Take(pageSize).ToList();
			return Json(new { data = orders, page = page, max = Math.Ceiling(totalPage) });
		}

		// GET: OrderManageController/Details/5
		public async Task<IActionResult> Details(int id)
		{
			_order = await _orderService.GetById(id);
			var details = await _orderDetailService.GetByOrder(id);
			_order.orderDetails = details;
			foreach (var item in details) // chi tiet don hang
			{
				var product = await _productService.GetById(item.Id_Product);
				foreach (var itemProd in product.bookViewModels)
				{
					_order.Weight += (itemProd.Weight * item.Quantity);
					_order.Width = _order.Width > itemProd.Widght ? _order.Width : itemProd.Widght;
					_order.Length = _order.Length > itemProd.Length ? _order.Length : itemProd.Length;
					_order.Height += (itemProd.Height * item.Quantity);
				}
				_order.Total += item.Quantity * item.Price;
			}
			_order.Weight = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(_order.Weight / 1000)));
			_order.Width = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(_order.Width / 100)));
			_order.Length = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(_order.Length / 100)));
			_order.Height = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(_order.Height / 100)));
			_order.TotalPayment = _order.Total + Convert.ToInt32(_order.Shipfee);
			var promotions = await _orderPromotionService.GetByOrder(id);
			_order.orderPromotions = promotions;
			if (promotions != null) // thong tin khuyen mai
			{
				foreach (var promotion in promotions)
				{
					if (promotion.PercentReduct != null)
					{
						promotion.TotalReduct = Convert.ToInt32(Math.Floor(Convert.ToDouble((_order.Total / 100) * promotion.PercentReduct)));

						if (promotion.TotalReduct > promotion.ReductMax) promotion.TotalReduct = promotion.ReductMax;
						_order.TotalPayment -= promotion.TotalReduct;
					}
					else
					{
						promotion.TotalReduct = Convert.ToInt32(promotion.AmountReduct);
						_order.TotalPayment -= promotion.TotalReduct;
					}
				}
			}
			if (_order.IsUsePoint) // su dung diem
			{
				_order.TotalPayment -= Convert.ToInt32(_order.PointAmount);
			}
			_order.orderPayments = await _orderPaymentService.GetByOrder(id);
			_order.returnOrders = await _returnOrderService.GetByOrder(id);
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
					order.ModifiNotes += "\n" + DateTime.Now + " : Đơn được xác nhận bởi " + staff.Name + " - Mã code [" + staff.Code + "]\n";
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
					order.ModifiNotes += "\n" + DateTime.Now + " : Đơn được xác nhận giao bởi " + staff.Name + " - Mã code [" + staff.Code + "]\n";
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
			var user = await _userService.GetById(order.Id_User);
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
					order.ModifiNotes += "\n" + DateTime.Now + " : Đơn được xác nhận hoàn thành bởi " + staff.Name + " - Mã code [" + staff.Code + "]\n";
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
						var history = new PointTranHistoryViewModel()
						{
							PointUserd = point,
							Id_User = user.Id,
							Id_Order = id,
						};
						await _pointNPromotionService.Accumulate(order.Id_User, point, history);
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
					try
					{
						var statusId = (await _statusOrderService.GetAll()).Where(x => x.Status == 5).First().Id;
						order.Id_Status = statusId;
						order.ModifiDate = DateTime.Now;
						order.ModifiNotes += "\n" + DateTime.Now + " : Đơn được xác nhận yêu cầu trả hàng bởi" + staff.Name + " - Mã code [" + staff.Code + "]\n";
						var result1 = _orderService.Update(order);
						var result2 = _returnOrderService.Add(new CreateReturnOrderModel { Notes = modifyChange, Status = 1, Id_Order = order.Id });
						await Task.WhenAll(result1, result2);
						return Json(new { success = true });
					}
					catch (Exception ex)
					{
						return Json(new { success = false, errorMessage = ex.Message });
					}
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
					order.ModifiNotes += "\n" + DateTime.Now + " : Đơn được xác nhận đã hoàn trả và chờ xử lý hoàn trả bởi " + staff.Name + " - Mã code [" + staff.Code + "]\n";
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
				foreach (var item1 in detailProducts) // tang lại so luong
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
					var actionNote = "";
					var condition = false;
					foreach (var action in myAction)
					{
						var product = await _productService.GetById(action.Key);
						if (action.Value == 1)
						{
							if (!await HandleReturn(action.Key))
							{
								condition = false;
								break;
							}
							else
							{
								condition = true;
								actionNote += "Sản phẩm " + product.Name + " | trả về kho\n";
							}
						}
						else
						{
							condition = true;
							actionNote += "Sản phẩm " + product.Name + " | lỗi không được trả về kho\n";
						}
					}
					if (condition)
					{
						order.ModifiNotes += "\n" + DateTime.Now + " : Đơn được xác nhận hoàn thành xử lý hàng hoàn trả bởi " + staff.Name + " - Mã code [" + staff.Code + "]\n" + actionNote + " Ghi chú: " + modifyChange + "\n";
						var result = await _orderService.Update(order);
						var returnOrderList = await _returnOrderService.GetByOrder(order.Id);
						foreach (var item in returnOrderList)
						{
							await _returnOrderService.Update(item.Id, new UpdateReturnOrderModel { Notes = item.Notes, Status = 0 });
						}
						return Json(new { success = result });
					}
					return Json(new { success = false, errorMessage = "\nLỗi khi sử lí hàng trả lại vui lòng kiểm tra lại hệ thống!" });
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
				order.ModifiNotes += "\n" + DateTime.Now + " : Đơn được xác nhận yêu cầu hủy bởi" + staff.Name + " - Mã code [" + staff.Code + "]\n Ghi chú: " + modifyChange + "\n";
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
							await _productPreviewService.ChangeQuantity(item.Id_Product, item.Quantity);    // tang lai sp
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
				if (order.Status == 4)
				{
					var statusId = (await _statusOrderService.GetAll()).Where(x => x.Status == 9).First().Id;
					order.Id_Status = statusId;
					order.ModifiDate = DateTime.Now;
					order.ModifiNotes += "\n" + DateTime.Now + " : Đơn được xác nhận đóng bởi " + staff.Name + " - Mã code [" + staff.Code + "]\n Ngày đóng đơn : " + Convert.ToDateTime(order.CompleteDate).AddDays(3);
					var result = await _orderService.Update(order);
					return Json(new { success = result });
				}
				return Json(new { success = false, errorMessage = "\nTrạng thái đơn hàng không hợp lệ!" });
			}
			else return Json(new { success = false, errorMessage = "\nBạn chưa đăng nhập hoặc đơn hàng không được tìm thấy! \nVui lòng kiểm tra lại!" });
		}

	}
}