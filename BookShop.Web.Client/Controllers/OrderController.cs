using BookShop.BLL.ConfigurationModel.CartDetailModel;
using BookShop.BLL.ConfigurationModel.OrderDetailModel;
using BookShop.BLL.ConfigurationModel.OrderModel;
using BookShop.BLL.ConfigurationModel.OrderPaymentModel;
using BookShop.BLL.ConfigurationModel.PointTranHistoryModel;
using BookShop.BLL.ConfigurationModel.PromotionModel;
using BookShop.BLL.ConfigurationModel.UerPromotionModel;
using BookShop.BLL.ConfigurationModel.UserModel;
using BookShop.BLL.IService;
using BookShop.BLL.Service;
using BookShop.DAL.Entities;
using BookShop.Web.Client.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BookShop.Web.Client.Controllers
{
	public class OrderController : Controller
	{
		private List<OrderViewModel> _orders;
		private OrderViewModel _order;
		private readonly UserManager<Userr> _userManager;
		private readonly IOrderService _orderService;
		private readonly IProductService _productService;
		private readonly IOrderDetailService _orderDetailService;
		private readonly IPaymentFormService _paymentFormService;
		private readonly IOrderPaymentService _orderPaymentService;
		private readonly IUserService _userService;
		private readonly IUserPromotionService _userPromotionService;
		private readonly IStatusOrderService _statusService;
		private readonly ICartDetailService _cartDetailService;
		private readonly IPromotionService _promotionService;
		private readonly ProductPreviewService _productPreviewService;
		private readonly PointNPromotionSerVice _pointNPromotionService;


		public OrderController(UserManager<Userr> userManager, IOrderService orderService, IProductService productService, IOrderDetailService orderDetailService, IPaymentFormService paymentFormService, IOrderPaymentService orderPaymentService, IUserService userService, IStatusOrderService statusOrderService, ICartDetailService cartDetailService, IPromotionService promotionService, IUserPromotionService userPromotionService)
		{
			_orders = new List<OrderViewModel>();
			_order = new OrderViewModel();
			_userManager = userManager;
			_orderService = orderService;
			_productService = productService;
			_orderDetailService = orderDetailService;
			_paymentFormService = paymentFormService;
			_orderPaymentService = orderPaymentService;
			_userService = userService;
			_statusService = statusOrderService;
			_cartDetailService = cartDetailService;
			_promotionService = promotionService;
			_userPromotionService = userPromotionService;

			_productPreviewService = new ProductPreviewService();
			_pointNPromotionService = new PointNPromotionSerVice();
		}

		private Task<Userr> GetCurrentUserAsync()
		{
			return _userManager.GetUserAsync(HttpContext.User);
		}
		// GET: OrderController
		public async Task<IActionResult> Index()
		{
			var user = await GetCurrentUserAsync();
			if (user != null)
			{
				_orders = (await _orderService.GetAll()).Where(x => x.Id_User == user.Id).ToList();
				return View(_orders);
			}
			else
			{
				return RedirectToAction("Login", "Account", new { area = "Identity" });
			}
		}

		// GET: OrderController/Details/5

		public async Task<IActionResult> OrderDetails(int id)
		{
			_order = await _orderService.GetById(id);
			var details = await _orderDetailService.GetByOrder(id);
			ViewBag.Details = details;
			foreach (var item in details)
			{
				_order.Total += item.Quantity * item.Price;
			}
			if (_order.Id_Promotion != null && _order.Id_Promotion != 0)
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

		// GET: OrderController/Create
		public async Task<IActionResult> CreateOnlineOrder(int id, int quantity)
		{
			var createModel = new OrderViewModel();
			createModel.orderDetails = new List<OrderDetailViewModel>();
			var orderdetail = new OrderDetailViewModel();
			var user = await GetCurrentUserAsync();
			var cartUse = 0;
			// kiem tra login
			if (user != null)
			{
				createModel.NameUser = user.Name;
				createModel.Email = user.Email;
				createModel.Phone = user.PhoneNumber;
				createModel.Id_User = user.Id;
				ViewBag.UserPromotions = await _userPromotionService.GetByUser(user.Id);
				ViewBag.User = await _userService.GetById(user.Id);
			}
			// mua trong gio hang
			if (id == 0)
			{
				List<CartDetailViewModel> cd = new List<CartDetailViewModel>();
				if (user != null) // non-account
				{
					cd = await _cartDetailService.GetByCart(user.Id);
				}
				else
				{
					var customCartChar = HttpContext.Session.GetString("sessionCart");
					if (!string.IsNullOrEmpty(customCartChar))
					{
						cd = JsonConvert.DeserializeObject<List<CartDetailViewModel>>(customCartChar);
					}
				}
				// duyệt sản phẩm
				foreach (var prod in cd)
				{
					var product = await _productService.GetById(prod.Id_Product);
					foreach (var item in product.bookViewModels)
					{
						createModel.Weight += (item.Weight * prod.Quantity);
						createModel.Width += (item.Widght * prod.Quantity);
						createModel.Length += (item.Length * prod.Quantity);
						createModel.Height += (item.Height * prod.Quantity);
					}
					orderdetail = new OrderDetailViewModel()
					{
						NameProduct = product.Name,
						Img = product.imageViewModels.FirstOrDefault().ImageUrl,
						Id_Product = product.Id,
						Price = product.Price,
						Quantity = prod.Quantity,
					};
					createModel.orderDetails.Add(orderdetail);
					createModel.Total += prod.Quantity * product.Price;
				}
				cartUse = 1;
			}
			else // mua sản phẩm chi định
			{
				var product = await _productService.GetById(id);
				foreach (var item in product.bookViewModels)
				{
					createModel.Weight += item.Weight * quantity;
					createModel.Width += item.Widght;
					createModel.Length += item.Length;
					createModel.Height += item.Height * quantity;
				}
				orderdetail = new OrderDetailViewModel()
				{
					NameProduct = product.Name,
					Img = product.imageViewModels.FirstOrDefault().ImageUrl,
					Id_Product = product.Id,
					Price = product.Price,
					Quantity = quantity,
				};
				createModel.orderDetails.Add(orderdetail);
				createModel.Total += quantity * product.Price;
			}
			createModel.Weight = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(createModel.Weight / 1000)));
			createModel.Width = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(createModel.Width / 100)));
			createModel.Length = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(createModel.Length / 100)));
			createModel.Height = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(createModel.Height / 100)));
			var validPromotion = new List<PromotionViewModel>();
			foreach (var promotion in (await _pointNPromotionService.GetActivePromotion()).Where(x => x.NameType.Equals("Tự động")))
			{
				if (promotion.Condition <= createModel.Total)
				{
					validPromotion.Add(promotion);
				}
			}
			var usePromotion = validPromotion.OrderByDescending(x => x.Condition).ThenByDescending(x => x.CreatedDate).FirstOrDefault();
			if (usePromotion != null)
			{
				if (usePromotion.PercentReduct != null && usePromotion.PercentReduct > 0)
				{
					usePromotion.TotalReduct = Convert.ToInt32(Math.Floor(Convert.ToDouble((createModel.Total / 100) * usePromotion.PercentReduct)));
					if (usePromotion.TotalReduct > usePromotion.ReductMax)
					{
						usePromotion.TotalReduct = usePromotion.ReductMax;
					}
				}
				else usePromotion.TotalReduct = Convert.ToInt32(usePromotion.AmountReduct);
			}
			createModel.Id_Promotion = usePromotion == null ? null : usePromotion.Id;
			ViewBag.Payments = (await _paymentFormService.GetAll()).Where(x => x.Status == 1).ToList();
			ViewBag.Promotion = usePromotion;
			ViewBag.Cart = cartUse;
			return View(createModel);
		}

		// POST: OrderController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateOnlineOrder(OrderViewModel request, int cartUse)
		{
			try
			{
				//return Ok();
				UserModel user = new UserModel();
				if (request.Id_User == 0)
				{
					user = (await _userService.GetAll()).Where(x => x.Email.Equals(request.Email)).FirstOrDefault();
					if (user == null)
					{
						user = (await _userService.GetAll()).Where(x => x.Code.Equals("KH0000000")).FirstOrDefault();
					}
					request.Id_User = user.Id;
				}
				request.ModifiDate = DateTime.Now;
				request.ModifiNotes = DateTime.Now + " : Đơn hàng được tạo\n";
				request.Id_Status = (await _statusService.GetAll()).Where(x => x.Status == 1).FirstOrDefault().Id; // hóa đơn chờ
				request.IsOnlineOrder = true;
				request.IsUsePoint = request.PointUsed > 0 ? true : false;

				// return Ok(request);
				var result = await _orderService.Add(request);
				if (result.Id != 0)
				{
					foreach (var item in request.orderDetails)
					{
						await _productPreviewService.ChangeQuantity(item.Id_Product, -item.Quantity);
					}
					var op = new CreateOrderPaymentModel()
					{
						Id_Order = result.Id,
						Id_Payment = request.paymentId,
						paymentAmount = request.TotalPayment,
						Status = 0,
					};
					await _orderPaymentService.Add(op);
					if (result.Id_Promotion != 0) // chuyen trang thai khuyen mai
					{
						await _pointNPromotionService.ModifyUserPromotion(result.Id_User, Convert.ToInt32(result.Id_Promotion), 0);
					}
					if (request.IsUsePoint && request.PointUsed > 0) // tru diem dã dung
					{
						var history = new CreatePointTranHistoryModel()
						{
							PointUserd = -Convert.ToInt32(request.PointUsed),
							Remaining = user.Point - Convert.ToInt32(request.PointUsed),
							Id_User = user.Id,
							Id_Parents = result.Id,
						};
						await _pointNPromotionService.Accumulate(request.Id_User, -Convert.ToInt32(request.PointUsed), history);
					}
					if (cartUse == 1)
					{
						var cartUser = await GetCurrentUserAsync();
						if (cartUser != null)
						{
							await _cartDetailService.DeleteByCart(user.Id);
						}
						else
						{
							HttpContext.Session.Remove("sessionCart");
						}
					}
					return RedirectToAction("OrderDetails", new { id = result.Id });
				}
				return BadRequest();
			}
			catch
			{
				return BadRequest();
			}
		}

		public async Task<IActionResult> GetPromotionByUser(int id)
		{
			var promotions = (await _userPromotionService.GetByUser(id)).Where(x => x.Status == 1);
			var validPromotions = new List<UserPromotionViewModel>();
			foreach (var item in promotions)
			{
				DateTime createdDate = Convert.ToDateTime(item.CreatedDate);
				if (createdDate.AddDays(item.StorageTerm).CompareTo(DateTime.Now) >= 0)
				{
					validPromotions.Add(item);
				}
			}
			return Json(promotions);
		}

		public async Task<IActionResult> GetPromotion(int id, int total)
		{
			var usePromotion = await _promotionService.GetById(id);
			if (usePromotion != null)
			{
				if (usePromotion.PercentReduct != null && usePromotion.PercentReduct > 0)
				{
					usePromotion.TotalReduct = Convert.ToInt32(Math.Floor(Convert.ToDouble((total / 100) * usePromotion.PercentReduct)));
					if (usePromotion.TotalReduct > usePromotion.ReductMax)
					{
						usePromotion.TotalReduct = usePromotion.ReductMax;
					}
				}
				else usePromotion.TotalReduct = Convert.ToInt32(usePromotion.AmountReduct);
			}
			return Json(usePromotion);
		}

		// GET: OrderController/Edit/5
		public async Task<IActionResult> Edit(int id)
		{
			return View();
		}

		// POST: OrderController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, IFormCollection collection)
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

		// GET: OrderController/Delete/5
		public async Task<IActionResult> DeleteOrder(int id)
		{
			var order = await _orderService.GetById(id);
			bool result = false;
			var statusId = (await _statusService.GetAll()).Where(x => x.Status == 8).FirstOrDefault().Id;
			order.Id_Status = statusId;
			order.ModifiDate = DateTime.Now;
			order.ModifiNotes = order.ModifiNotes + "\n" + DateTime.Now + " : Đơn hàng được hủy bởi khách hàng\n";
			result = await _orderService.Update(order);
			if (result)
			{
				// tang lại so luong sp
				await _productPreviewService.ChangeQuantity(id, 1);
				if (order.Id_Promotion != 0 && order.Id_Promotion != null)
				{
					await _pointNPromotionService.ModifyUserPromotion(order.Id_User, Convert.ToInt32(order.Id_Promotion), 1);
				}
				return Json(new { success = true });
			}
			else return Json(new { success = false });
		}

	}
}
