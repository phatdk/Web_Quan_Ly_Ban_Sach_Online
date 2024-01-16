// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Linq;
using System.Threading.Tasks;
using App.Areas.Identity.Models.ManageViewModels;
using BookShop.Web.Client.ExtendMethods;
using BookShop.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BookShop.BLL.IService;
using BookShop.BLL.ConfigurationModel.OrderModel;
using BookShop.BLL.Service;
using BookShop.BLL.ConfigurationModel.UerPromotionModel;
using BookShop.Web.Client.Models;
using static NuGet.Packaging.PackagingConstants;
using GemBox.Spreadsheet;
using BookShop.BLL.ConfigurationModel.PromotionModel;
using BookShop.Web.Client.Services;
using BookShop.BLL.ConfigurationModel.PointTranHistoryModel;
using System.Drawing;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace App.Areas.Identity.Controllers
{

	// [Authorize]
	[Area("Identity")]
	[Route("/Member/[action]")]
	public class ManageController : Controller
	{
		private readonly UserManager<Userr> _userManager;
        private readonly IOrderService _orderService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IOrderPaymentService _orderPaymentService;
        private readonly IOrderPromotionService _orderPromotionService;
        private readonly IPointTranHistoryService _historyService;
		private readonly IPromotionService _promotionService;
		private readonly IUserPromotionService _userPromotionService;
		private readonly IOrderService _Iorder;
		private readonly SignInManager<Userr> _signInManager;
		private readonly IEmailSender _emailSender;
		private readonly ILogger<ManageController> _logger;
		private readonly IWebHostEnvironment _hostingEnvironment;
		private readonly PointNPromotionSerVice _pointNPromotionSerVice;
        private readonly IProductService _productService;
        private readonly IReturnOrderService _returnOrderService;

		public ManageController(
		UserManager<Userr> userManager,
		IPointTranHistoryService historyService,
		IPromotionService promotionService,
		IUserPromotionService userPromotionService,
		SignInManager<Userr> signInManager,
		IEmailSender emailSender,
		ILogger<ManageController> logger,
		IWebHostEnvironment hostingEnvironment,
		IOrderService iorder,
		IProductService productService,
		IReturnOrderService returnOrderService,
		IOrderService orderService,
		IOrderDetailService orderDetailService,
		IOrderPaymentService orderPaymentService,
		IOrderPromotionService orderPromotionService)
		{
			_userManager = userManager;
			_historyService = historyService;
			_promotionService = promotionService;
			_userPromotionService = userPromotionService;
			_signInManager = signInManager;
			_emailSender = emailSender;
			_logger = logger;
			_hostingEnvironment = hostingEnvironment;
			_Iorder = iorder;
			_pointNPromotionSerVice = new PointNPromotionSerVice();
			_productService = productService;
			_returnOrderService = returnOrderService;
			_orderService = orderService;
			_orderDetailService = orderDetailService;
			_orderPaymentService = orderPaymentService;
			_orderPromotionService = orderPromotionService;
		}
		[TempData]
		public string StatusMessage { get; set; }

		private async Task<Userr> GetUser()
		{
			return await _userManager.GetUserAsync(HttpContext.User);
		}

		//private async Task<dynamic> GetBills()
		//{
		//	var list = _userManager.Users.Include(x => x.Orders).ThenInclude(x => x.OrderDetails).ToListAsync();
		//	return list;
  //      }
		private async Task<List<OrderViewModel>> GetBill()
		{
			var user = await GetUser();
			var billOfUser = (await _Iorder.GetAll()).Where(x=>x.Id_User== user.Id).ToList();
			return billOfUser;
		}
        public async Task<IActionResult> DetailsBill(int id)
        {
			var  _order = await _orderService.GetById(id);
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
            _order.Width = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(_order.Width / 10)));
            _order.Length = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(_order.Length / 10)));
            _order.Height = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(_order.Height / 10)));
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
        // tất cả bill
        public async Task<IActionResult> ViewAllBill([FromQuery(Name = "p")] int currentPages)
		{
			var listbill = await GetBill();
			int pagesize = 10;
			if (pagesize <= 0)
			{
				pagesize = 10;
			}
			int countPages = (int)Math.Ceiling((double)listbill.Count() / pagesize);
			if (currentPages > countPages)
			{
				currentPages = countPages;
			}
			if (currentPages < 1)
			{
				currentPages = 1;
			}

			var pagingmodel = new PagingModel()
			{
				currentpage = currentPages,
				countpages = countPages,
				generateUrl = (int? p) => Url.Action("ViewAllBill", "Manage", new { areas = "Identity", p = p, pagesize = pagesize })
			};
			ViewBag.pagingmodel = pagingmodel;
			listbill = listbill.Skip((pagingmodel.currentpage - 1) * pagesize).Take(pagesize).ToList();
			return View(listbill);
		}
		// bill đã xác nhận
		public async Task<IActionResult> ViewBillAwaitConfirm([FromQuery(Name = "p")] int currentPages)
		{

			var listbill = (await GetBill()).Where(x => x.Status == 1).ToList();
			int pagesize = 10;
			if (pagesize <= 0)
			{
				pagesize = 10;
			}
			int countPages = (int)Math.Ceiling((double)listbill.Count() / pagesize);
			if (currentPages > countPages)
			{
				currentPages = countPages;
			}
			if (currentPages < 1)
			{
				currentPages = 1;
			}

			var pagingmodel = new PagingModel()
			{
				currentpage = currentPages,
				countpages = countPages,
				generateUrl = (int? p) => Url.Action("ViewBillAwaitConfirm", "Manage", new { areas = "Identity", p = p, pagesize = pagesize })
			};
			ViewBag.pagingmodel = pagingmodel;
			listbill = listbill.Skip((pagingmodel.currentpage - 1) * pagesize).Take(pagesize).ToList();
			return View(listbill);
		
		}
		// bill đang giao
		public async Task<IActionResult> ViewBillShipping([FromQuery(Name = "p")] int currentPages)
		{
			var listbill = (await GetBill()).Where(x => x.Status == 3).ToList();
			int pagesize = 10;
			if (pagesize <= 0)
			{
				pagesize = 10;
			}
			int countPages = (int)Math.Ceiling((double)listbill.Count() / pagesize);
			if (currentPages > countPages)
			{
				currentPages = countPages;
			}
			if (currentPages < 1)
			{
				currentPages = 1;
			}

			var pagingmodel = new PagingModel()
			{
				currentpage = currentPages,
				countpages = countPages,
				generateUrl = (int? p) => Url.Action("ViewBillAwaitConfirm", "Manage", new { areas = "Identity", p = p, pagesize = pagesize })
			};
			ViewBag.pagingmodel = pagingmodel;
			listbill = listbill.Skip((pagingmodel.currentpage - 1) * pagesize).Take(pagesize).ToList();
			return View(listbill);
	
		}
		// bill đã giao
		public async Task<IActionResult> ViewBillSuccess([FromQuery(Name = "p")] int currentPages)
		{
			var listbill = (await GetBill()).Where(x => x.Status == 4).ToList();
			int pagesize = 10;
			if (pagesize <= 0)
			{
				pagesize = 10;
			}
			int countPages = (int)Math.Ceiling((double)listbill.Count() / pagesize);
			if (currentPages > countPages)
			{
				currentPages = countPages;
			}
			if (currentPages < 1)
			{
				currentPages = 1;
			}

			var pagingmodel = new PagingModel()
			{
				currentpage = currentPages,
				countpages = countPages,
				generateUrl = (int? p) => Url.Action("ViewBillSuccess", "Manage", new { areas = "Identity", p = p, pagesize = pagesize })
			};
			ViewBag.pagingmodel = pagingmodel;
			listbill = listbill.Skip((pagingmodel.currentpage - 1) * pagesize).Take(pagesize).ToList();
			return View(listbill);
			
		}
		// bill huỷ
		public async Task<IActionResult> ViewBillCancel([FromQuery(Name = "p")] int currentPages)
		{
			var listbill = (await GetBill()).Where(x => x.Status == 8).ToList();
			int pagesize = 10;
			if (pagesize <= 0)
			{
				pagesize = 10;
			}
			int countPages = (int)Math.Ceiling((double)listbill.Count() / pagesize);
			if (currentPages > countPages)
			{
				currentPages = countPages;
			}
			if (currentPages < 1)
			{
				currentPages = 1;
			}

			var pagingmodel = new PagingModel()
			{
				currentpage = currentPages,
				countpages = countPages,
				generateUrl = (int? p) => Url.Action("ViewBillCancel", "Manage", new { areas = "Identity", p = p, pagesize = pagesize })
			};
			ViewBag.pagingmodel = pagingmodel;
			listbill = listbill.Skip((pagingmodel.currentpage - 1) * pagesize).Take(pagesize).ToList();
			return View(listbill);
		}

		// vi diem và khuyen mai
		public async Task<IActionResult> ViewWallet()
		{
			var user = await GetUser();
			return View(user);
		}

		public async Task<IActionResult> GetHistory(int idUser, int page)
		{
			var historys = (await _historyService.GetByUser(idUser)).OrderBy(x => x.CreatedDate).ToList();
			int pageSize = 10;
			double totalPage = historys.Count / (double)pageSize;
			int next = totalPage > page ? 1 : 0;
			historys = historys.Skip((page - 1) * pageSize).Take(pageSize).ToList();
			return Json(new { data = historys, next = next });
		}

		public async Task<IActionResult> ViewUserPromotion()
		{
			var user = await GetUser();
			return View(user);
		}
		
		public async Task<IActionResult> GetUserPromotion(int id, int page, int statusFilter, int dateFilter)
		{
			//HttpContext.Session.SetString("code", code ?? string.Empty);
			var userPromotions = await _userPromotionService.GetByUser(id);
			List<UserPromotionViewModel> dataList = new List<UserPromotionViewModel>();

			if (statusFilter != 2 && dateFilter == 0) // hết hạn
			{
				userPromotions = userPromotions.Where(x => x.Status == statusFilter).ToList();
				foreach (var item in userPromotions)
				{
					DateTime endTime = Convert.ToDateTime(item.EndDate);
					int result = endTime.CompareTo(DateTime.Now);
					if (result < 0)
					{
						dataList.Add(item);
					}
				}
			}
			else if (statusFilter == 2 && dateFilter == 0) // tất cả - hết hạn
			{
				foreach (var item in userPromotions)
				{
					DateTime endTime = Convert.ToDateTime(item.EndDate);
					int result = endTime.CompareTo(DateTime.Now);
					if (result < 0)
					{
						dataList.Add(item);
					}
				}
			}
			else if (statusFilter != 2 && dateFilter == 1) // còn hạn
			{
				userPromotions = userPromotions.Where(x => x.Status == statusFilter).ToList();
				foreach (var item in userPromotions)
				{
					DateTime endTime = Convert.ToDateTime(item.EndDate);
					int result = endTime.CompareTo(DateTime.Now);
					if (result >= 0)
					{
						dataList.Add(item);
					}
				}
			}
			else if (statusFilter == 2 && dateFilter == 1) // tất cả - còn hạn
			{
				foreach (var item in userPromotions)
				{
					DateTime endTime = Convert.ToDateTime(item.EndDate);
					int result = endTime.CompareTo(DateTime.Now);
					if (result >= 0)
					{
						dataList.Add(item);
					}
				}
			}
			else if (statusFilter != 2 && dateFilter == 2) dataList = userPromotions.Where(x => x.Status == statusFilter).ToList(); // tất cả
			else if (statusFilter == 2 && dateFilter == 2) dataList = userPromotions.Where(x => x.Status == 1).ToList(); // tất cả - tất cả
			else dataList = userPromotions; // tất cả - tất cả

			dataList = dataList.OrderByDescending(x => x.CreatedDate).ToList();
			int pagesize = 10;
			double totalPage = (double)dataList.Count / pagesize;
			dataList = dataList.Skip((page - 1) * pagesize).Take(pagesize).ToList();
			return Json(new { data = dataList, page = page, max = Math.Ceiling(totalPage) });
		}

		public async Task<IActionResult> GetPromotion(int Id,string code)
		{
			//if (code == null)
			//{
			//	string codese = HttpContext.Session.GetString("code");
			//	code = codese;
			//}
			if (Id == 0)
			{

				var user = await GetUser();
				if(user == null) return Json(new { success = false, redirect = true });

				Id = user.Id;
			}
			
			var promotion = await _promotionService.GetByCode(code);
			if(promotion == null) return Json(new {success =  false, message = "Mã khuyến mãi không tồn tại, vui lòng thử lại!" });
			if (promotion.Quantity > 0)
			{
				DateTime endTime = Convert.ToDateTime(promotion.EndDate);
				if (endTime.CompareTo(DateTime.Now) < 0) return Json(new { success = false, message = "Mã khuyến mãi đã hết hạn" });
				if (promotion.Status == 0) return Json(new { success = false, message = "Mã khuyến mãi đã bị đóng" });
				var userPromotion = await _userPromotionService.GetById(Id, promotion.Id);
				if (userPromotion == null)
				{
					try
					{
						var endDate = promotion.StorageTerm == null ? DateTime.Now.AddYears(100) : DateTime.Now.AddDays(Convert.ToInt32(promotion.StorageTerm));
						var result1 = _userPromotionService.Add(new CreateUserPromotionModel
						{
							Id_User = Id,
							Id_Promotion = promotion.Id,
							Status = 1,
							EndDate = endDate,
						});
						var result2 = _promotionService.ChangeQuantity(promotion.Id, -1);
						await Task.WhenAll(result1, result2);
						return Json(new { success = true, message = "Bạn đã nhận được khuyến mãi " + promotion.Name + " sử dụng đến hết " + endDate });
					}
					catch (Exception ex)
					{
						return Json(new { success = false, redirect = false, message = "Có lỗi xảy ra\n" + ex.Message });
					}

				}
				return Json(new { success = false, redirect = false, message = "Bạn đã hết lượt nhận mã khuyến mãi này!" });
			}
			return Json(new { success = false, redirect = false, message = "Mã khuyến mãi này đã hết lượt nhận!" });
		}

		[HttpPost]
		public async Task<IActionResult> ExchangePromotion(int userId, int promotionId)
		{
			try
			{
				var promotion = await _promotionService.GetById(promotionId);
				if (promotion != null && promotion.Quantity > 0)
				{
					var task1 = _userPromotionService.Add(new CreateUserPromotionModel
					{
						Id_User = userId,
						Id_Promotion = promotion.Id,
						EndDate = DateTime.Now.AddDays(Convert.ToInt32(promotion.StorageTerm)),
						Status = 1,
					});
					var history = new PointTranHistoryViewModel()
					{
						PointUserd = - Convert.ToInt32(promotion.ConversionPoint),
						Id_User = userId,
						Id_Promotion = promotionId,
					};
					var task2 = _pointNPromotionSerVice.Accumulate(userId, history.PointUserd, history);
					var obj = new UpdatePromotionModel()
					{
						Name = promotion.Name,
						Code = promotion.Code,
						Condition = promotion.Condition,
						StorageTerm = promotion.StorageTerm,
						ConversionPoint = promotion.ConversionPoint,
						PercentReduct = promotion.PercentReduct,
						AmountReduct = promotion.AmountReduct,
						ReductMax = promotion.ReductMax,
						Quantity = promotion.Quantity - 1,
						StartDate = promotion.StartDate,
						EndDate = promotion.EndDate,
						Description = promotion.Description,
						Status = promotion.Status,
						Id_Type = promotion.Id_Type,
					};
					var task3 = _promotionService.Update(promotion.Id, obj);
					await Task.WhenAll(task1, task2, task3);
					return Json(new { success = true });
				}
				return Json(new { success = false });
			}
			catch
			{
				return Json(new { success = false });
			}
		}

		[HttpPost]
		public async Task<IActionResult> UpLoadAvata(IndexViewModel indexViewModel)
		{
			
			var user = await GetCurrentUserAsync();
			if (user == null)
			{
				return NotFound();
			}
			if (indexViewModel._file != null)
			{
				StatusMessage = "Tải lên ảnh đại điện thành công";
				var fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + Path.GetExtension(indexViewModel._file.FileName);
				var uploadPath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
				string filePath = Path.Combine(uploadPath, "users", fileName);
				using (var filestream = new FileStream(filePath, FileMode.Create))
				{
					await indexViewModel._file.CopyToAsync(filestream);
				}
				if (user.Img == null)
				{
					user.Img = fileName;
					await _userManager.UpdateAsync(user);
					return RedirectToAction(nameof(Index));
				}
				else
				{
					var uploadPath1 = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
					var productPath = Path.Combine(uploadPath1, "users");
					var filename = Path.Combine(productPath, user.Img);
					if (System.IO.File.Exists(filename))
					{
						System.IO.File.Delete(filename);
					}
					user.Img = fileName;
					await _userManager.UpdateAsync(user);
					return RedirectToAction(nameof(Index));
				}



			}
			return RedirectToAction("Index","Manage",new {area="Identity"});

		}

		// GET: /Manage/Index
		[HttpGet]
		public async Task<IActionResult> Index(ManageMessageId? message = null)
		{
			ViewData["StatusMessage"] =
				message == ManageMessageId.ChangePasswordSuccess ? "Đã thay đổi mật khẩu."
				: message == ManageMessageId.SetPasswordSuccess ? "Đã đặt lại mật khẩu."
				: message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
				: message == ManageMessageId.Error ? "Có lỗi."
				: message == ManageMessageId.AddPhoneSuccess ? "Đã thêm số điện thoại."
				: message == ManageMessageId.RemovePhoneSuccess ? "Đã bỏ số điện thoại."
				: "";

			var user = await GetCurrentUserAsync();
			var model = new IndexViewModel
			{
				HasPassword = await _userManager.HasPasswordAsync(user),
				PhoneNumber = await _userManager.GetPhoneNumberAsync(user),
				TwoFactor = await _userManager.GetTwoFactorEnabledAsync(user),
				Logins = await _userManager.GetLoginsAsync(user),
				BrowserRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user),
				//AuthenticatorKey = await _userManager.GetAuthenticatorKeyAsync(user),
				profile = new EditExtraProfileModel()
				{

					UserName = user.UserName,
					UserEmail = user.Email,
					PhoneNumber = user.PhoneNumber,
					Birth = user.Birth,
					Code = user.Code,
					Gender = user.Gender,
					Img = user.Img,
					Name = user.Name,
				}
			};
			return View(model);
		}
		public enum ManageMessageId
		{
			AddPhoneSuccess,
			AddLoginSuccess,
			ChangePasswordSuccess,
			SetTwoFactorSuccess,
			SetPasswordSuccess,
			RemoveLoginSuccess,
			RemovePhoneSuccess,
			Error
		}
		private Task<Userr> GetCurrentUserAsync()
		{
			return _userManager.GetUserAsync(HttpContext.User);
		}

		//
		// GET: /Manage/ChangePassword
		[HttpGet]
		public IActionResult ChangePassword()
		{
			return View();
		}

		//
		// POST: /Manage/ChangePassword
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			var user = await GetCurrentUserAsync();
			if (user != null)
			{
				var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
				if (result.Succeeded)
				{
					await _signInManager.SignInAsync(user, isPersistent: false);
					_logger.LogInformation(3, "User changed their password successfully.");
					StatusMessage = "Đổi mật khẩu thành công";
					return RedirectToAction(nameof(Index), new { Message = ManageMessageId.ChangePasswordSuccess });
				}
				ModelState.AddModelError(result);
				return View(model);
			}
			return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
		}
		//
		// GET: /Manage/SetPassword
		[HttpGet]
		public IActionResult SetPassword()
		{
			return View();
		}

		//
		// POST: /Manage/SetPassword
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var user = await GetCurrentUserAsync();
			if (user != null)
			{
				var result = await _userManager.AddPasswordAsync(user, model.NewPassword);
				if (result.Succeeded)
				{
					await _signInManager.SignInAsync(user, isPersistent: false);
					return RedirectToAction(nameof(Index), new { Message = ManageMessageId.SetPasswordSuccess });
				}
				ModelState.AddModelError(result);
				return View(model);
			}
			return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
		}

		//GET: /Manage/ManageLogins
		[HttpGet]
		public async Task<IActionResult> ManageLogins(ManageMessageId? message = null)
		{
			ViewData["StatusMessage"] =
				message == ManageMessageId.RemoveLoginSuccess ? "Đã loại bỏ liên kết tài khoản."
				: message == ManageMessageId.AddLoginSuccess ? "Đã thêm liên kết tài khoản"
				: message == ManageMessageId.Error ? "Có lỗi."
				: "";
			var user = await GetCurrentUserAsync();
			if (user == null)
			{
				return View("Error");
			}
			var userLogins = await _userManager.GetLoginsAsync(user);
			var schemes = await _signInManager.GetExternalAuthenticationSchemesAsync();
			var otherLogins = schemes.Where(auth => userLogins.All(ul => auth.Name != ul.LoginProvider)).ToList();
			ViewData["ShowRemoveButton"] = user.PasswordHash != null || userLogins.Count > 1;
			return View(new ManageLoginsViewModel
			{
				CurrentLogins = userLogins,
				OtherLogins = otherLogins
			});
		}


		//
		// POST: /Manage/LinkLogin
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult LinkLogin(string provider)
		{
			// Request a redirect to the external login provider to link a login for the current user
			var redirectUrl = Url.Action("LinkLoginCallback", "Manage");
			var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, _userManager.GetUserId(User));
			return Challenge(properties, provider);
		}

		//
		// GET: /Manage/LinkLoginCallback
		[HttpGet]
		public async Task<ActionResult> LinkLoginCallback()
		{
			var user = await GetCurrentUserAsync();
			if (user == null)
			{
				return View("Error");
			}
			var info = await _signInManager.GetExternalLoginInfoAsync(await _userManager.GetUserIdAsync(user));
			if (info == null)
			{
				return RedirectToAction(nameof(ManageLogins), new { Message = ManageMessageId.Error });
			}
			var result = await _userManager.AddLoginAsync(user, info);
			var message = result.Succeeded ? ManageMessageId.AddLoginSuccess : ManageMessageId.Error;
			return RedirectToAction(nameof(ManageLogins), new { Message = message });
		}


		//
		// POST: /Manage/RemoveLogin
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> RemoveLogin(RemoveLoginViewModel account)
		{
			ManageMessageId? message = ManageMessageId.Error;
			var user = await GetCurrentUserAsync();
			if (user != null)
			{
				var result = await _userManager.RemoveLoginAsync(user, account.LoginProvider, account.ProviderKey);
				if (result.Succeeded)
				{
					await _signInManager.SignInAsync(user, isPersistent: false);
					message = ManageMessageId.RemoveLoginSuccess;
				}
			}
			return RedirectToAction(nameof(ManageLogins), new { Message = message });
		}
		//
		// GET: /Manage/AddPhoneNumber
		public IActionResult AddPhoneNumber()
		{
			return View();
		}

		//
		// POST: /Manage/AddPhoneNumber
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			// Generate the token and send it
			var user = await GetCurrentUserAsync();
			var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, model.PhoneNumber);
			await _emailSender.SendEmailAsync(user.Email, "Sdt", "Mã xác thực là: " + code);
			StatusMessage = "Hãy check Email của mình để nhận code";
			return RedirectToAction(nameof(VerifyPhoneNumber), new { PhoneNumber = model.PhoneNumber });
		}
		//
		// GET: /Manage/VerifyPhoneNumber
		[HttpGet]
		public async Task<IActionResult> VerifyPhoneNumber(string phoneNumber)
		{
			var code = await _userManager.GenerateChangePhoneNumberTokenAsync(await GetCurrentUserAsync(), phoneNumber);
			// Send an SMS to verify the phone number
			return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
		}

		//
		// POST: /Manage/VerifyPhoneNumber
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			var user = await GetCurrentUserAsync();
			if (user != null)
			{
				var result = await _userManager.ChangePhoneNumberAsync(user, model.PhoneNumber, model.Code);
				if (result.Succeeded)
				{
					await _signInManager.SignInAsync(user, isPersistent: false);
					return RedirectToAction(nameof(Index), new { Message = ManageMessageId.AddPhoneSuccess });
				}
			}
			// If we got this far, something failed, redisplay the form
			ModelState.AddModelError(string.Empty, "Lỗi thêm số điện thoại");
			return View(model);
		}
		//
		// GET: /Manage/RemovePhoneNumber
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> RemovePhoneNumber()
		{
			var user = await GetCurrentUserAsync();
			if (user != null)
			{
				var result = await _userManager.SetPhoneNumberAsync(user, null);
				if (result.Succeeded)
				{
					await _signInManager.SignInAsync(user, isPersistent: false);
					return RedirectToAction(nameof(Index), new { Message = ManageMessageId.RemovePhoneSuccess });
				}
			}
			return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
		}


		//
		// POST: /Manage/EnableTwoFactorAuthentication
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> EnableTwoFactorAuthentication()
		{
			var user = await GetCurrentUserAsync();
			if (user != null)
			{
				await _userManager.SetTwoFactorEnabledAsync(user, true);
				await _signInManager.SignInAsync(user, isPersistent: false);
			}
			return RedirectToAction(nameof(Index), "Manage");
		}

		//
		// POST: /Manage/DisableTwoFactorAuthentication
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DisableTwoFactorAuthentication()
		{
			var user = await GetCurrentUserAsync();
			if (user != null)
			{
				await _userManager.SetTwoFactorEnabledAsync(user, false);
				await _signInManager.SignInAsync(user, isPersistent: false);
				_logger.LogInformation(2, "User disabled two-factor authentication.");
			}
			return RedirectToAction(nameof(Index), "Manage");
		}
		//
		// POST: /Manage/ResetAuthenticatorKey
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ResetAuthenticatorKey()
		{
			var user = await GetCurrentUserAsync();
			if (user != null)
			{
				await _userManager.ResetAuthenticatorKeyAsync(user);
				_logger.LogInformation(1, "User reset authenticator key.");
			}
			return RedirectToAction(nameof(Index), "Manage");
		}

		//
		// POST: /Manage/GenerateRecoveryCode
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> GenerateRecoveryCode()
		{
			var user = await GetCurrentUserAsync();
			if (user != null)
			{
				var codes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 5);
				_logger.LogInformation(1, "User generated new recovery code.");
				return View("DisplayRecoveryCodes", new DisplayRecoveryCodesViewModel { Codes = codes });
			}
			return View("Error");
		}

		[HttpGet]
		public async Task<IActionResult> EditProfile()
		{
			var user = await GetCurrentUserAsync();

			var model = new EditExtraProfileModel()
			{

				UserName = user.UserName,
				UserEmail = user.Email,
				PhoneNumber = user.PhoneNumber,
				Birth = user.Birth,
				Code = user.Code,
				Gender = user.Gender,
				Img = user.Img,
				Name = user.Name,
			};
			return View(model);
		}
		[HttpPost, ActionName("EditProfile"), ValidateAntiForgeryToken]
		public async Task<IActionResult> EditProfileConfirm(IndexViewModel model)
		{
			var user = await GetCurrentUserAsync();

			user.PhoneNumber = model.profile.PhoneNumber;
			user.Gender = model.profile.Gender;
			user.Birth = model.profile.Birth;

			await _userManager.UpdateAsync(user);
			StatusMessage = "Cập nhập thông tin thành công";
			await _signInManager.RefreshSignInAsync(user);
			return RedirectToAction(nameof(Index), "Manage");

		}


	}
}
