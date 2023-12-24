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

namespace App.Areas.Identity.Controllers
{

	// [Authorize]
	[Area("Identity")]
	[Route("/Member/[action]")]
	public class ManageController : Controller
	{
		private readonly UserManager<Userr> _userManager;
		private readonly IPointTranHistoryService _historyService;
		private readonly IPromotionService _promotionService;
		private readonly IUserPromotionService _userPromotionService;
		private readonly IOrderService _Iorder;
		private readonly SignInManager<Userr> _signInManager;
		private readonly IEmailSender _emailSender;
		private readonly ILogger<ManageController> _logger;
		private readonly IWebHostEnvironment _hostingEnvironment;
		public ManageController(
		UserManager<Userr> userManager,
		IPointTranHistoryService historyService,
		IPromotionService promotionService,
		IUserPromotionService userPromotionService,
		SignInManager<Userr> signInManager,
		IEmailSender emailSender,
		ILogger<ManageController> logger,
		IWebHostEnvironment hostingEnvironment,
		IOrderService iorder)
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
		}
		[TempData]
		public string StatusMessage { get; set; }

		private async Task<Userr> GetUser()
		{
			return await _userManager.GetUserAsync(HttpContext.User);
		}

		private async Task<List<ViewOrder>> GetBill()
		{
			var user = await GetUser();
			var billOfUser = await _Iorder.GetOrderByUser(user.Id);
			return billOfUser;
		}
		// tất cả bill
		public async Task<IActionResult> ViewAllBill()
		{
			return View(await GetBill());
		}
		// bill đã xác nhận
		public async Task<IActionResult> ViewBillAwaitConfirm()
		{
			return View((await GetBill()).Where(x => x.Status == 1).ToList());
		}
		// bill đang giao
		public async Task<IActionResult> ViewBillShipping()
		{
			return View((await GetBill()).Where(x => x.Status == 3).ToList());
		}
		// bill đã giao
		public async Task<IActionResult> ViewBillSuccess()
		{
			return View((await GetBill()).Where(x => x.Status == 4).ToList());
		}
		// bill huỷ
		public async Task<IActionResult> ViewBillCancel()
		{
			return View((await GetBill()).Where(x => x.Status == 8).ToList());
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
			return Json(new {data = dataList, page = page, max = Math.Ceiling(totalPage)});
		}

		public async Task<IActionResult> GetPromotion(int id, string code)
		{
			var promotion = await _promotionService.GetByCode(code);
			if (promotion != null && promotion.Quantity > 0)
			{
				DateTime endTime = Convert.ToDateTime(promotion.EndDate);
				if (endTime.CompareTo(DateTime.Now) < 0) return Json(new { success = false, message = "Mã khuyến mãi đã hết hạn" });
				if (promotion.Status == 0) return Json(new { success = false, message = "Mã khuyến mãi đã bị đóng" });
				var userPromotion = await _userPromotionService.GetById(id, promotion.Id);
				if (userPromotion == null)
				{
					try
					{
						var endDate = promotion.StorageTerm == null ? DateTime.Now.AddYears(100) : DateTime.Now.AddDays(Convert.ToInt32(promotion.StorageTerm));
						var result1 = _userPromotionService.Add(new CreateUserPromotionModel
						{
							Id_User = id,
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
						return Json(new { success = false, message = "Có lỗi xảy ra\n" + ex.Message });
					}

				}
				return Json(new { success = false, message = "Bạn đã hết lượt nhận mã khuyến mãi này!" });
			}
			return Json(new { success = false, message = "Mã khuyến mãi này đã hết lượt nhận!" });
		}

		[HttpPost]
		public async Task<IActionResult> UpLoadAvata(IndexViewModel indexViewModel)
		{
			StatusMessage = "Tải lên ảnh đại điện thành công";
			var user = await GetCurrentUserAsync();
			if (user == null)
			{
				return NotFound();
			}
			if (indexViewModel._file != null)
			{
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
			return View();

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
