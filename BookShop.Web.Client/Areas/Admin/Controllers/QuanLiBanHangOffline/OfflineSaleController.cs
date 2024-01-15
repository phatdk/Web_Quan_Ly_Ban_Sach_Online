using BookShop.BLL.ConfigurationModel.CartDetailModel;
using BookShop.BLL.ConfigurationModel.OrderDetailModel;
using BookShop.BLL.ConfigurationModel.OrderModel;
using BookShop.BLL.ConfigurationModel.OrderPaymentModel;
using BookShop.BLL.ConfigurationModel.OrderPromotionModel;
using BookShop.BLL.ConfigurationModel.PointTranHistoryModel;
using BookShop.BLL.ConfigurationModel.ProductModel;
using BookShop.BLL.ConfigurationModel.PromotionModel;
using BookShop.BLL.ConfigurationModel.UserModel;
using BookShop.BLL.IService;
using BookShop.BLL.Service;
using BookShop.DAL.Entities;
using BookShop.Web.Client.Areas.Admin.Models;
using BookShop.Web.Client.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Asn1.X509;
using System.Security.Policy;
using System.Text.RegularExpressions;

namespace BookShop.Web.Client.Areas.Admin.Controllers.QuanLiBanHangOffline
{
	[Area("Admin")]
	[Authorize(Roles = "Admin,Staff")]
	public class OfflineSaleController : Controller
	{
		private readonly IOrderService _orderService;
		private readonly IOrderDetailService _orderDetailService;
		private readonly IProductService _productService;
		private readonly IProductBookService _productBookService;
		private readonly IUserService _userService;
		private readonly IStatusOrderService _statusService;
		private readonly IPaymentFormService _paymentFormService;
		private readonly IOrderPromotionService _orderPromotionService;
		private readonly IOrderPaymentService _orderPaymentService;
		private readonly IBookService _bookService;
		private readonly IPromotionService _promotionService;
		private readonly UserManager<Userr> _userManager;
		private readonly ProductPreviewService _productPreviewService;
		private readonly PointNPromotionSerVice _pointNPromotionService;

		public OfflineSaleController(IOrderService orderService, IOrderDetailService orderDetailService, IProductService productService, IProductBookService productBookService, IUserService userService, UserManager<Userr> userManager, IStatusOrderService statusOrderService, IPaymentFormService paymentFormService, IOrderPaymentService orderPaymentService, IBookService bookService, IPromotionService promotionService, IOrderPromotionService orderPromotionService)
		{
			_orderService = orderService;
			_orderDetailService = orderDetailService;
			_productService = productService;
			_productBookService = productBookService;
			_userService = userService;
			_userManager = userManager;
			_statusService = statusOrderService;
			_paymentFormService = paymentFormService;
			_orderPaymentService = orderPaymentService;
			_bookService = bookService;
			_promotionService = promotionService;
			_orderPromotionService = orderPromotionService;

			_productPreviewService = new ProductPreviewService();
			_pointNPromotionService = new PointNPromotionSerVice();
			_orderPromotionService = orderPromotionService;
		}

		// GET: OfflineSaleController
		public ActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> GetWaitingOrder()
		{
			var listOrder = (await _orderService.GetAll()).Where(x => x.Status == 0).ToList();
			var newList = listOrder == null ? new List<OrderViewModel>() : listOrder;
			return Json(newList);
		}

		public async Task<IActionResult> ClearTemporary()
		{
			HttpContext.Session.Remove("sessionOrder");
			return Json(new { success = true });
		}

		public async Task<IActionResult> GetDetails(int id)
		{
			var order = await _orderService.GetById(id);
			if (order == null)
			{
				order = new OrderViewModel();
				var user = (await _userService.GetAll()).Where(x => x.Code.Equals("KH0000000")).FirstOrDefault();
				order.Id_User = user.Id;
				order.NameUser = user.Name;
				order.UserCode = user.Code;
			}
			var sessionDetails = HttpContext.Session.GetString("sessionOrder");
			if (string.IsNullOrEmpty(sessionDetails))
			{
				var details = await _orderDetailService.GetByOrder(id);
				HttpContext.Session.SetString("sessionOrder", JsonConvert.SerializeObject(details));
				sessionDetails = HttpContext.Session.GetString("sessionOrder");
			}
			var data = JsonConvert.DeserializeObject<List<OrderDetailViewModel>>(sessionDetails);
			int total = 0;
			foreach (var item in data)
			{
				total += item.Price * item.Quantity;
			}
			return Json(new { order = order, details = data, total = total });
		}

		public async Task<IActionResult> GetProducts(int page, string? keyWord)
		{
			var list = (await _productService.GetAll()).Where(x => x.Status == 1 && x.Type == 1).OrderByDescending(x => x.CreatedDate).ToList();
			if (!string.IsNullOrEmpty(keyWord))
			{
				list = list.Where(x => x.Name.ToLower().Contains(keyWord.ToLower())).OrderByDescending(x => x.CreatedDate).ToList();
			}
			var pageSize = 10;
			double totalPage = (double)list.Count / pageSize;
			list = list.Skip((page - 1) * pageSize).Take(pageSize).ToList();
			return Json(new { data = list, page = page, max = Math.Ceiling(totalPage) });
		}

		public async Task<IActionResult> GetUser(string keyWord)
		{
			var user = new UserModel();
			if (!string.IsNullOrEmpty(keyWord))
			{
				var userList = (await _userService.GetAll()).Where(x => x.Status == 1);
				user = userList.Where(
					x => x.Email.ToLower().Equals(keyWord.ToLower())
					|| x.Code.ToLower().Equals(keyWord.ToLower())
					).FirstOrDefault();
			}
			return Json(user);
		}

		public async Task<IActionResult> CheckActivePromotion(int total)
		{
			var promotions = (await _pointNPromotionService.GetActivePromotion()).Where(x => x.NameType.Equals("Phiếu khuyến mãi áp dụng tự động"));
			var validPromotions = new List<PromotionViewModel>();
			foreach (var item in promotions)
			{
				if (item.Condition <= total)
				{
					validPromotions.Add(item);
				}
			}
			var usePromotion = validPromotions.OrderByDescending(x => x.Condition).ThenByDescending(x => x.CreatedDate).FirstOrDefault();
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

		public async Task<IActionResult> AddProduct(int id, int orderId, int quantity)
		{
			// return Ok(id + orderId + quantity);
			var sessionOrder = HttpContext.Session.GetString("sessionOrder");
			var listDetails = new List<OrderDetailViewModel>();
			var product = await _productService.GetById(id);
			if (product == null) return Json(new { success = false, errorMessage = "Không tìm thấy sản phẩm" });
			if (!string.IsNullOrEmpty(sessionOrder))
			{
				listDetails = JsonConvert.DeserializeObject<List<OrderDetailViewModel>>(sessionOrder);
				var pd = listDetails.FirstOrDefault(x => x.Id_Product == product.Id);
				if (pd != null)
				{
					pd.Quantity += quantity;
					if (pd.Quantity <= 0)
					{
						listDetails.Remove(pd);
					}
					goto editquantity;
				}
			}
			var detail = new OrderDetailViewModel()
			{
				Id_Product = product.Id,
				Quantity = quantity,
				NameProduct = product.Name,
				Price = product.NewPrice,
				Id_Order = orderId,
			};
			listDetails.Add(detail);
		editquantity:
			HttpContext.Session.SetString("sessionOrder", JsonConvert.SerializeObject(listDetails));
			return Json(new { success = true });
		}

		public async Task<string> GenerateCode(int length)
		{
			// Khởi tạo đối tượng Random
			Random random = new Random();

			// Tạo một chuỗi các ký tự ngẫu nhiên
			string characters = "0123456789";
			string code = "";
			for (int i = 0; i < length; i++)
			{
				code += characters[random.Next(characters.Length)];
			}
			var duplicate = (await _orderService.GetAll()).Where(c => c.Code.Equals(code));
			if (!duplicate.Any())
			{
				return code;
			}
			return (await GenerateCode(length)).ToString();
		}

		public async Task<OrderViewModel> SaveOrder(OrderViewModel request)
		{
			if (request.Id_User == 0)
			{
				var user = (await _userService.GetAll()).Where(x => x.Code.Equals("KH0000000")).FirstOrDefault();
				request.Id_User = user.Id;
				request.NameUser = user.Name;
			}
			if (string.IsNullOrEmpty(request.Receiver))
			{
				request.Receiver = request.NameUser;
			}
			if (request.Id != 0) // đơn đã được lưu trước đó
			{
				var order = await _orderService.GetById(request.Id);
				order.Id_Status = request.Id_Status;
				order.Id_User = request.Id_User;
				order.Id_Staff = request.Id_Staff;
				order.Receiver = request.Receiver;
				order.Email = request.Email;
				order.Phone = request.Phone;
				var result = await _orderService.Update(order);
				if (result)
				{
					var sessionDetails = HttpContext.Session.GetString("sessionOrder");
					if (!string.IsNullOrEmpty(sessionDetails))
					{
						var data = JsonConvert.DeserializeObject<List<OrderDetailViewModel>>(sessionDetails);
						var details = await _orderDetailService.GetByOrder(request.Id);
						foreach (var detail in details) // kiem tra cac phan tu cu 
						{
							for (var i = 0; i < data.Count(); i++)
							{
								if (detail.Id_Product == data[i].Id_Product)
								{
									if (detail.Quantity <= 0)
									{
										if (!await _orderDetailService.Delete(detail.Id)) goto skipAction1;
									}
									else if (detail.Quantity != data[i].Quantity)
									{
										detail.Quantity = data[i].Quantity;
										detail.Price = data[i].Price;
										if (!await _orderDetailService.Update(detail.Id, detail)) goto skipAction1;
									}
									if (!await _productPreviewService.ChangeQuantity(data[i].Id_Product, detail.Quantity - data[i].Quantity)) goto skipAction1;
									data.RemoveAt(i);
									goto skipSave;
								}
							}
							await _orderDetailService.Delete(detail.Id);
						skipSave:;
						}
						foreach (var item in data) // them phan tu moi
						{
							if (!await _orderDetailService.Add(new OrderDetailViewModel()
							{
								Id_Order = request.Id,
								Id_User = request.Id_User,
								Id_Product = item.Id_Product,
								Price = item.Price,
								Quantity = item.Quantity,
							})) goto skipAction1;
							if (!await _productPreviewService.ChangeQuantity(item.Id_Product, -item.Quantity)) goto skipAction1;
						}
					}
					return request;
				}
			skipAction1:;
				request.Id = 0;
				return request;
			}
			else // đơn chưa được lưu
			{
				request.Id_Status = request.Id_Status; // trạng thái đơn
				request.Code = "OF" + await GenerateCode(8);
				var result = await _orderService.Add(request); // thêm mới đơn
				if (result.Id != 0)
				{
					var sessionDetails = HttpContext.Session.GetString("sessionOrder");
					if (!string.IsNullOrEmpty(sessionDetails))
					{
						var data = JsonConvert.DeserializeObject<List<OrderDetailViewModel>>(sessionDetails);
						foreach (var item in data)
						{
							if (!await _productPreviewService.ChangeQuantity(item.Id_Product, -item.Quantity)) goto skipAction2; // giam so luong san pham
							var od = new OrderDetailViewModel()
							{
								Id_Order = result.Id,
								Id_User = request.Id_User,
								Id_Product = item.Id_Product,
								Price = item.Price,
								Quantity = item.Quantity,
							};
							if (!await _orderDetailService.Add(od)) goto skipAction2;
						}
					}
					return result;
				}
			skipAction2:;
				return request;
			}
		}

		public async Task<bool> SuccessOfflineOrder(OrderViewModel request)
		{
			var order = await _orderService.GetById(request.Id);
			var user = await _userService.GetById(order.Id_User);
			var details = await _orderDetailService.GetByOrder(request.Id);
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
					else return false;
				}
				order.Total += item.Quantity * item.Price; // tinh tien
			}

			order.TotalPayment = order.Total;
			if (request.Id_Promotions != null)
			{
				foreach (var item in request.Id_Promotions)
				{
					var promotion = await _promotionService.GetById(Convert.ToInt32(item));
					var opro = new OrderPromotionViewModel()
					{
						Id_Order = order.Id,
						Id_Promotion = item,
					};
					var checkpromotion = await _orderPromotionService.Add(opro);
					if (checkpromotion)
					{
						if (promotion.PercentReduct != null)
						{
							var amount = Convert.ToInt32(Math.Floor(Convert.ToDouble((order.Total / 100) * promotion.PercentReduct)));
							if (amount > promotion.ReductMax) amount = promotion.ReductMax;
							order.TotalPayment -= amount;
						}
						else order.TotalPayment -= Convert.ToInt32(promotion.AmountReduct);
					}
					else return false;
				}
			}
			var paymentId = (await _paymentFormService.GetAll()).Where(x => x.Name.Equals("Thanh toán tại quầy")).First().Id;
			var opay = new CreateOrderPaymentModel()
			{
				Id_Order = order.Id,
				Id_Payment = paymentId,
				paymentAmount = order.TotalPayment,
				Status = 1,
			};
			if (!await _orderPaymentService.Add(opay)) return false; // lưu phương thức thanh toán
			if (!user.Code.Equals("KH0000000"))
			{
				int point = Convert.ToInt32(Math.Floor(Convert.ToDouble(order.Total / 20000))); // 20k = 1 điểm
				if (point > 0)
				{
					var history = new PointTranHistoryViewModel()
					{
						PointUserd = point,
						Id_User = user.Id,
						Id_Order = order.Id,
					};
					if (!await _pointNPromotionService.Accumulate(order.Id_User, point, history)) return false; // lưu lịch sử tích điểm
				}
			}
			return true;
		}

		// GET: OfflineSaleController/Create
		public async Task<IActionResult> CreateOfflineOrder()
		{
			HttpContext.Session.Remove("sessionOrder");
			var staff = await _userManager.GetUserAsync(HttpContext.User);
			if (staff != null)
			{
				ViewBag.Users = (await _userService.GetAll()).Where(x => x.Status == 1).ToList();
				var createModel = new OrderViewModel();
				var user = (await _userService.GetAll()).Where(x => x.Code.Equals("KH0000000")).FirstOrDefault();
				createModel.Id_User = user.Id;
				createModel.NameUser = user.Name;
				createModel.UserCode = user.Code;
				ViewBag.Staff = staff;
				return View(createModel);
			}
			return Redirect("/login");
		}

		// POST: OfflineSaleController/Create
		[HttpPost]
		public async Task<IActionResult> CreateOfflineOrder(OrderViewModel request)
		{
			try
			{
				request.Id_Status = (await _statusService.GetAll()).Where(x => x.Status == 4).FirstOrDefault().Id;
				var result = await SaveOrder(request);
				if (result.Id != 0)
				{
					var success = await SuccessOfflineOrder(result);
					if (success)
					{
						await ClearTemporary();
						return Json(new { success = true, message = "Tạo đơn thành công!" });
					}
					else return Json(new { success = false, errorMessage = "Tạo đơn thất bại\n Có lỗi trong quá trình hoàn thành đơn hàng!" });
				}
				return Json(new { success = false, errorMessage = "Tạo thất bại\n Có lỗi trong quá trình lưu đơn hàng!" });
			}
			catch
			{
				return Json(new { success = false, errorMessage = "Xuất hiện lỗi ở đâu đó!" });
			}
		}

		public async Task<IActionResult> SaveTemprory(OrderViewModel request)
		{
			ResultModel resultJson = new ResultModel();
			try
			{
				request.Id_Status = (await _statusService.GetAll()).Where(x => x.Status == 0).FirstOrDefault().Id;
				var result = await SaveOrder(request);
				if (request.Id != 0)
				{
					await ClearTemporary();
					return Json(new { success = true, message = "Lưu thành công" });
				}
				return Json(new { success = false, errorMessage = "Lưu thất bại" });
			}
			catch
			{
				return Json(new { success = false, errorMessage = "Xuất hiện lỗi ở đâu đó" });
			}
		}
	}
}
