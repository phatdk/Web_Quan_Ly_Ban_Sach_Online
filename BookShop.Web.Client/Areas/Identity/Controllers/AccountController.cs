// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using App.Areas.Identity.Models.AccountViewModels;
using BookShop.BLL.ConfigurationModel.CartDetailModel;
using BookShop.BLL.ConfigurationModel.PointTranHistoryModel;
using BookShop.BLL.IService;
using BookShop.DAL.Entities;
using BookShop.DAL.Repositopy;
using BookShop.Web.Client.ExtendMethods;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace App.Areas.Identity.Controllers
{
    //  [Authorize]
    [Area("Identity")]
    [Route("/Account/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<Userr> _userManager;
        private readonly SignInManager<Userr> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<AccountController> _logger;
        private readonly IUserService _userService;
        private readonly ICartService _CartRepository;
        private readonly IWalletpointService _WalletPointRepository;

        public AccountController(UserManager<Userr> userManager, SignInManager<Userr> signInManager, IEmailSender emailSender, ILogger<AccountController> logger, IUserService userService, ICartService cartRepository, IWalletpointService walletPointRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _userService = userService;
            _CartRepository = cartRepository;
            _WalletPointRepository = walletPointRepository;
        }

        private Task<Userr> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }
        // GET: /Account/Login
        [HttpGet("/login/")]
        [AllowAnonymous]
        public  IActionResult Login()
        {

          // var roleuser=await _userManager.GetRolesAsync(await GetCurrentUserAsync());
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost("/login/")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            //returnUrl ??= Url.Content("~/");
            //ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                if (model.Password == null && model.UserNameOrEmail == null)
                {
                    return NotFound();
                }
                var result = await _signInManager.PasswordSignInAsync(model.UserNameOrEmail, model.Password, model.RememberMe, lockoutOnFailure: true);
                // Tìm UserName theo Email, đăng nhập lại
                if ((!result.Succeeded))
                {
                    ModelState.AddModelError("Sai mật khẩu tài khoản");
                    ViewBag.Error = "Sai tài khoản mật khẩu";
                    return View(model);
                }

                if (result.Succeeded)
                {
                    var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == model.UserNameOrEmail);
                    var roleuser = await _userManager.GetRolesAsync(user);
                    if (roleuser != null)
                    {
                        if (roleuser.Contains("Admin"))
                        {
                            return Redirect("/Admin/Home/Index");
                        }
                    }
                    _logger.LogInformation(1, "User logged in.");
                    return Redirect("Home/Index");
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToAction(nameof(SendCode), new { ReturnUrl = "~/", RememberMe = model.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    ViewBag.Error = "Tài khoản đã bị khoá";
                    _logger.LogWarning(2, "Tài khoản bị khóa");
                    return View("Lockout");
                }
                else
                {
                    ViewBag.Error = "Tài khoản đã bị khoá";
                    ModelState.AddModelError("Sai mật khẩu tài khoản");
                    return View(model);
                }
            }
            else
            {
                ViewBag.Error = "Sai tài khoản mật khẩu";
                return View(model);
            }

        }

        // POST: /Account/LogOff
        [HttpPost("/logout/")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User đăng xuất");
            return RedirectToAction("Index", "Home", new { area = "" });
        }
        //
        // GET: /Account/Register
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        //

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
            var duplicate = (await _userService.GetAll()).Where(c => c.Code.Equals(code));
            if (!duplicate.Any())
            {
                return code;
            }
            return (await GenerateCode(length)).ToString();
        }

		// POST: /Account/Register
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
		{
			returnUrl ??= Url.Content("~/");
			ViewData["ReturnUrl"] = returnUrl;
			if (ModelState.IsValid)
			{
				// kiểm cha email
				var emailCheck = (await _userService.GetAll()).Where(x=>x.Email.Equals(model.Email));
				if (emailCheck.Any())
				{
					_logger.LogInformation("Email đã được sử dụng.");
					return View(model);
				}
				var user = new Userr { Code = "KH" + await GenerateCode(7), UserName = model.UserName, Email = model.Email, Name = model.Name, Gender = 0, CreatedDate = DateTime.Now, Status = 1 };
				var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var cart = new CartViewModel()
                    {
                        Id_User = user.Id,

                    };
                    await _CartRepository.Add(cart);
                    var walletPoint = new WalletPointViewModel()
                    {
                        Id_User = user.Id,
                        CreatedDate = DateTime.Now,
                        Status = 1,
                        Point = 0,
                    };
                    await _WalletPointRepository.Add(walletPoint);
                    _logger.LogInformation("Đã tạo user mới.");
                    var users = await _userManager.FindByNameAsync(user.UserName);
                    if (users != null)
                    {
                        try
                        {
                            // Phát sinh token để xác nhận email
                            var codes = await _userManager.GenerateEmailConfirmationTokenAsync(users);
                            codes = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(codes));

                            // https://localhost:5001/confirm-email?userId=fdsfds&code=xyz&returnUrl=
                            var callbackUrl = Url.ActionLink(
                                action: nameof(ConfirmEmail),
                                values:
                                    new
                                    {
                                        area = "Identity",
                                        userId = user.Id,
                                        code = codes
                                    },
                                protocol: Request.Scheme);

                            await _emailSender.SendEmailAsync(model.Email,
                                "Xác nhận địa chỉ email",
                                @$"Bạn đã đăng ký tài khoản trên RazorWeb, 
                           hãy <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>bấm vào đây</a> 
                           để kích hoạt tài khoản.");

                            if (_userManager.Options.SignIn.RequireConfirmedAccount)
                            {
                                return LocalRedirect(Url.Action(nameof(RegisterConfirmation)));
                            }
                            else
                            {
                                await _signInManager.SignInAsync(user, isPersistent: false);
                                return LocalRedirect(returnUrl);
                            }
                        }
                        catch (Exception ex)
                        {

                            throw;
                        }

                    }


                }
                else
                {
                    ModelState.AddModelError(result);
                }


            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // GET: /Account/ConfirmEmail
        [HttpGet]
        [AllowAnonymous]
        public IActionResult RegisterConfirmation()
        {
            return View();
        }

        // GET: /Account/ConfirmEmail
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("ErrorConfirmEmail");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("ErrorConfirmEmail");
            }
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "ErrorConfirmEmail");
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }
        //
        // GET: /Account/ExternalLoginCallback
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl ??= Url.Content("~/");
            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Lỗi sử dụng dịch vụ ngoài: {remoteError}");
                return View(nameof(Login));
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (result.Succeeded)
            {
                // Cập nhật lại token
                await _signInManager.UpdateExternalAuthenticationTokensAsync(info);

                _logger.LogInformation(5, "User logged in with {Name} provider.", info.LoginProvider);
                return LocalRedirect(returnUrl);
            }
            if (result.RequiresTwoFactor)
            {
                return RedirectToAction(nameof(SendCode), new { ReturnUrl = returnUrl });
            }
            if (result.IsLockedOut)
            {
                return View("Lockout");
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["ProviderDisplayName"] = info.ProviderDisplayName;
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }

                // Input.Email
                var registeredUser = await _userManager.FindByEmailAsync(model.Email);
                string externalEmail = null;
                Userr externalEmailUser = null;

                // Claim ~ Dac tinh mo ta mot doi tuong 
                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                {
                    externalEmail = info.Principal.FindFirstValue(ClaimTypes.Email);
                }

                if (externalEmail != null)
                {
                    externalEmailUser = await _userManager.FindByEmailAsync(externalEmail);
                }

                if ((registeredUser != null) && (externalEmailUser != null))
                {
                    // externalEmail  == Input.Email
                    if (registeredUser.Id == externalEmailUser.Id)
                    {
                        // Lien ket tai khoan, dang nhap
                        var resultLink = await _userManager.AddLoginAsync(registeredUser, info);
                        if (resultLink.Succeeded)
                        {
                            await _signInManager.SignInAsync(registeredUser, isPersistent: false);
                            return LocalRedirect(returnUrl);
                        }
                    }
                    else
                    {
                        // registeredUser = externalEmailUser (externalEmail != Input.Email)
                        /*
                            info => user1 (mail1@abc.com)
                                 => user2 (mail2@abc.com)
                        */
                        ModelState.AddModelError(string.Empty, "Không liên kết được tài khoản, hãy sử dụng email khác");
                        return View();
                    }
                }


                if ((externalEmailUser != null) && (registeredUser == null))
                {
                    ModelState.AddModelError(string.Empty, "Không hỗ trợ tạo tài khoản mới - có email khác email từ dịch vụ ngoài");
                    return View();
                }

                if ((externalEmailUser == null) && (externalEmail == model.Email))
                {
                    // Chua co Account -> Tao Account, lien ket, dang nhap
                    var newUser = new Userr()
                    {
                        UserName = externalEmail,
                        Email = externalEmail
                    };

                    var resultNewUser = await _userManager.CreateAsync(newUser);
                    if (resultNewUser.Succeeded)
                    {
                        await _userManager.AddLoginAsync(newUser, info);
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                        await _userManager.ConfirmEmailAsync(newUser, code);

                        await _signInManager.SignInAsync(newUser, isPersistent: false);

                        return LocalRedirect(returnUrl);

                    }
                    else
                    {
                        ModelState.AddModelError("Không tạo được tài khoản mới");
                        return View();
                    }
                }


                var user = new Userr { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        _logger.LogInformation(6, "User created an account using {Name} provider.", info.LoginProvider);

                        // Update any authentication tokens as well
                        await _signInManager.UpdateExternalAuthenticationTokensAsync(info);

                        return LocalRedirect(returnUrl);
                    }
                }
                ModelState.AddModelError(result);
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
        }

        //
        // GET: /Account/ForgotPassword
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.ActionLink(
                    action: nameof(ResetPassword),
                    values: new { area = "Identity", code },
                    protocol: Request.Scheme);


                await _emailSender.SendEmailAsync(
                    model.Email,
                    "Reset Password",
                    $"Hãy bấm <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>vào đây</a> để đặt lại mật khẩu.");

                return RedirectToAction(nameof(ForgotPasswordConfirmation));



            }
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return RedirectToAction(nameof(AccountController.ResetPasswordConfirmation), "Account");
            }
            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Code));

            var result = await _userManager.ResetPasswordAsync(user, code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(AccountController.ResetPasswordConfirmation), "Account");
            }
            ModelState.AddModelError(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/SendCode
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl = null, bool rememberMe = false)
        {
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            var userFactors = await _userManager.GetValidTwoFactorProvidersAsync(user);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }
        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            // Dùng mã Authenticator
            if (model.SelectedProvider == "Authenticator")
            {
                return RedirectToAction(nameof(VerifyAuthenticatorCode), new { ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
            }

            // Generate the token and send it
            var code = await _userManager.GenerateTwoFactorTokenAsync(user, model.SelectedProvider);
            if (string.IsNullOrWhiteSpace(code))
            {
                return View("Error");
            }

            var message = "Your security code is: " + code;
            if (model.SelectedProvider == "Email")
            {
                await _emailSender.SendEmailAsync(await _userManager.GetEmailAsync(user), "Security Code", message);
            }
            else if (model.SelectedProvider == "Phone")
            {
                await _emailSender.SendSmsAsync(await _userManager.GetPhoneNumberAsync(user), message);
            }

            return RedirectToAction(nameof(VerifyCode), new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }
        //
        // GET: /Account/VerifyCode
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyCode(string provider, bool rememberMe, string returnUrl = null)
        {
            // Require that the user has already logged in via username/password or external login
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            model.ReturnUrl ??= Url.Content("~/");
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes.
            // If a user enters incorrect codes for a specified amount of time then the user account
            // will be locked out for a specified amount of time.
            var result = await _signInManager.TwoFactorSignInAsync(model.Provider, model.Code, model.RememberMe, model.RememberBrowser);
            if (result.Succeeded)
            {
                return LocalRedirect(model.ReturnUrl);
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning(7, "User account locked out.");
                return View("Lockout");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid code.");
                return View(model);
            }
        }

        //
        // GET: /Account/VerifyAuthenticatorCode
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyAuthenticatorCode(bool rememberMe, string returnUrl = null)
        {
            // Require that the user has already logged in via username/password or external login
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            return View(new VerifyAuthenticatorCodeViewModel { ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyAuthenticatorCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyAuthenticatorCode(VerifyAuthenticatorCodeViewModel model)
        {
            model.ReturnUrl ??= Url.Content("~/");
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes.
            // If a user enters incorrect codes for a specified amount of time then the user account
            // will be locked out for a specified amount of time.
            var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(model.Code, model.RememberMe, model.RememberBrowser);
            if (result.Succeeded)
            {
                return LocalRedirect(model.ReturnUrl);
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning(7, "User account locked out.");
                return View("Lockout");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Mã sai.");
                return View(model);
            }
        }
        //
        // GET: /Account/UseRecoveryCode
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> UseRecoveryCode(string returnUrl = null)
        {
            // Require that the user has already logged in via username/password or external login
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            return View(new UseRecoveryCodeViewModel { ReturnUrl = returnUrl });
        }

        //
        // POST: /Account/UseRecoveryCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UseRecoveryCode(UseRecoveryCodeViewModel model)
        {
            model.ReturnUrl ??= Url.Content("~/");
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _signInManager.TwoFactorRecoveryCodeSignInAsync(model.Code);
            if (result.Succeeded)
            {
                return LocalRedirect(model.ReturnUrl);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Sai mã phục hồi.");
                return View(model);
            }
        }

        [Route("/accessdenied")]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }




    }
}
