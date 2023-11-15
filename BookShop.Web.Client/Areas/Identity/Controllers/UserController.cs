// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using App.Areas.Identity.Models.AccountViewModels;
using App.Areas.Identity.Models.ManageViewModels;
using App.Areas.Identity.Models.RoleViewModels;
using App.Areas.Identity.Models.UserViewModels;
using BookShop.BLL.IService;
using BookShop.DAL.ApplicationDbContext;
using BookShop.DAL.Entities;
using BookShop.DAL.Entities.Identity;
using BookShop.Web.Client.ExtendMethods;
using MessagePack;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ShopWheyProject.MVC.Areas.Identity.Controllers
{

    /// <summary>
    /// [Authorize(Roles = RoleName.Administrator)]
    /// </summary>
    [Area("Identity")]
    [Authorize(Roles = "Admin")]
    [Route("/ManageUser/[action]")]
    public class UserController : Controller
    {

        private readonly RoleManager<Role> _roleManager;
        private readonly IUserRoleService _UserRole;
        private readonly UserManager<Userr> _userManager;

        public UserController(RoleManager<Role> roleManager, IUserRoleService userRole, UserManager<Userr> userManager)
        {
            _roleManager = roleManager;
            _UserRole = userRole;
            _userManager = userManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        //
        // GET: /ManageUser/Index
        public async Task<IActionResult> Index([FromQuery(Name = "p")] int currentPage = 1)
        {
            if (currentPage < 1)
                currentPage = 1;

            var model = new UserListModel();
            model.currentPage = currentPage;

            var qr = _userManager.Users.OrderBy(u => u.UserName);

            model.totalUsers = await qr.CountAsync();
            model.countPages = (int)Math.Ceiling((double)model.totalUsers / model.ITEMS_PER_PAGE);

            if (model.currentPage > model.countPages)
                model.currentPage = model.countPages;

            var qr1 = qr.Skip((model.currentPage - 1) * model.ITEMS_PER_PAGE)
                        .Take(model.ITEMS_PER_PAGE)
                        .Select(u => new UserAndRole()
                        {
                            Id = u.Id,
                            UserName = u.UserName,
                        });
            if (qr1.Count() > 0)
            {
                model.users = qr1.ToList();

                foreach (var user in model.users)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    user.RoleNames = string.Join(",", roles);

                }

                return View(model);
            }
            else
            {
                return View();
            }

        }

        public async Task<IActionResult> ViewPromotionUser(int IdUser)
        {
            return View(await _userManager.Users.Include(x => x.UserPromotions).FirstOrDefaultAsync(x => x.Id == IdUser));
        }

        // GET: /ManageUser/AddRole/id
        [HttpGet("{id}")]
        public async Task<IActionResult> AddRole(int id)
        {
            //   await _context.RoleClaims.Where(rc => rc.RoleId == role.Id).ToListAsync();
            // public SelectList allRoles { get; set; }
            var model = new AddUserRoleModel();
            //if (Guid.Empty == id)
            //{
            //    return NotFound($"Không có user");
            //}

            model.user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (model.user == null)
            {
                return NotFound($"Không thấy user, id = {id}.");
            }

            model.RoleNames = (await _userManager.GetRolesAsync(model.user)).ToArray();


            List<string> roleNames = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            ViewBag.allRoles = new SelectList(roleNames);

            //   await GetClaims(model);

            return View(model);
        }

        // GET: /ManageUser/AddRole/id
        [HttpPost("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRoleAsync(int id, [Bind("RoleNames")] AddUserRoleModel model)
        {

            model.user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (model.user == null)
            {
                return NotFound($"Không thấy user, id = {id}.");
            }
            // await GetClaims(model);

            var OldRoleNames = (await _userManager.GetRolesAsync(model.user)).ToArray();
            if (model.RoleNames != null)
            {
                var deleteRoles = OldRoleNames.Where(r => !model.RoleNames.Contains(r));

                var addRoles = model.RoleNames.Where(r => !OldRoleNames.Contains(r));

                List<string> roleNames = await _roleManager.Roles.Select(r => r.Name).ToListAsync();

                ViewBag.allRoles = new SelectList(roleNames);
                if (addRoles != null)
                {
                    //var resultAdds = await _userManager.AddToRolesAsync(model.user, addRoles);
                    //if (!resultAdds.Succeeded)
                    //{
                    //    ModelState.AddModelError(resultAdds);
                    //    return View(model);
                    //}
                    foreach (var item in addRoles)
                    {
                        var role = await _roleManager.Roles.FirstOrDefaultAsync(x => x.NormalizedName == item.ToUpper());
                        var userole = new BookShop.BLL.ConfigurationModel.UserModel.UserRoleModel() { UserId = model.user.Id, RoleId = role.Id };
                        var statusdelete = await _UserRole.Add(userole);
                        if (statusdelete != true)
                        {
                            return View(model);
                        }

                    }
                }

                if (deleteRoles != null)
                {
                    foreach (var item in deleteRoles)
                    {
                        var role = await _roleManager.Roles.FirstOrDefaultAsync(x => x.NormalizedName == item.ToUpper());
                        var userole = (await _UserRole.GetAll()).FirstOrDefault(x => x.RoleId == role.Id && x.UserId == model.user.Id);
                        var statusdelete = await _UserRole.Delete(userole);
                        if (statusdelete != true)
                        {
                            return View(model);
                        }

                    }
                }


            }
            else
            {
                foreach (var item in OldRoleNames)
                {
                    var role = await _roleManager.Roles.FirstOrDefaultAsync(x => x.NormalizedName == item.ToUpper());
                    var userole = (await _UserRole.GetAll()).FirstOrDefault(x => x.RoleId == role.Id && x.UserId == model.user.Id);
                    var statusdelete = await _UserRole.Delete(userole);
                    if (statusdelete != true)
                    {
                        return View(model);
                    }

                }

            }
            StatusMessage = $"Vừa cập nhật role cho user: {model.user.UserName}";

            return RedirectToAction("Index");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> SetPasswordAsync(int id)
        {
            //if (Guid.Empty == id)
            //{
            //    return NotFound($"Không có user");
            //}

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
            ViewBag.user = ViewBag;

            if (user == null)
            {
                return NotFound($"Không thấy user, id = {id}.");
            }

            return View();
        }

        [HttpPost("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPasswordAsync(int id, SetUserPasswordModel model)
        {
            //if (Guid.Empty == id)
            //{
            //    return NotFound($"Không có user");
            //}

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
            ViewBag.user = ViewBag;

            if (user == null)
            {
                return NotFound($"Không thấy user, id = {id}.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _userManager.RemovePasswordAsync(user);

            var addPasswordResult = await _userManager.AddPasswordAsync(user, model.NewPassword);
            if (!addPasswordResult.Succeeded)
            {
                foreach (var error in addPasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            StatusMessage = $"Vừa cập nhật mật khẩu cho user: {user.UserName}";

            return RedirectToAction("Index");
        }


        //[HttpGet("{userid}")]
        //public async Task<ActionResult> AddClaimAsync(int userid)
        //{

        //    var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userid);
        //    if (user == null) return NotFound("Không tìm thấy user");
        //    ViewBag.user = user;
        //    return View();
        //}

        //[HttpPost("{userid}")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> AddClaimAsync(int userid, AddUserClaimModel model)
        //{

        //    var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userid);
        //    if (user == null) return NotFound("Không tìm thấy user");
        //    ViewBag.user = user;
        //    if (!ModelState.IsValid) return View(model);
        //    var claims = _context.UserClaims.Where(c => c.UserId == user.Id);

        //    if (claims.Any(c => c.ClaimType == model.ClaimType && c.ClaimValue == model.ClaimValue))
        //    {
        //        ModelState.AddModelError(string.Empty, "Đặc tính này đã có");
        //        return View(model);
        //    }

        //    await _userManager.AddClaimAsync(user, new Claim(model.ClaimType, model.ClaimValue));
        //    StatusMessage = "Đã thêm đặc tính cho user";

        //    return RedirectToAction("AddRole", new { id = user.Id });
        //}

        //[HttpGet("{claimid}")]
        //public async Task<IActionResult> EditClaim(int claimid)
        //{
        //    var userclaim = _context.UserClaims.Where(c => c.Id == claimid).FirstOrDefault();
        //    var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userclaim.UserId);

        //    if (user == null) return NotFound("Không tìm thấy user");

        //    var model = new AddUserClaimModel()
        //    {
        //        ClaimType = userclaim.ClaimType,
        //        ClaimValue = userclaim.ClaimValue

        //    };
        //    ViewBag.user = user;
        //    ViewBag.userclaim = userclaim;
        //    return View("AddClaim", model);
        //}
        //[HttpPost("{claimid}")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> EditClaim(int claimid, AddUserClaimModel model)
        //{
        //    var userclaim = _context.UserClaims.Where(c => c.Id == claimid).FirstOrDefault();
        //    var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userclaim.UserId);
        //    if (user == null) return NotFound("Không tìm thấy user");

        //    if (!ModelState.IsValid) return View("AddClaim", model);

        //    if (_context.UserClaims.Any(c => c.UserId == user.Id
        //        && c.ClaimType == model.ClaimType
        //        && c.ClaimValue == model.ClaimValue
        //        && c.Id != userclaim.Id))
        //    {
        //        ModelState.AddModelError("Claim này đã có");
        //        return View("AddClaim", model);
        //    }


        //    userclaim.ClaimType = model.ClaimType;
        //    userclaim.ClaimValue = model.ClaimValue;

        //    await _context.SaveChangesAsync();
        //    StatusMessage = "Bạn vừa cập nhật claim";


        //    ViewBag.user = user;
        //    ViewBag.userclaim = userclaim;
        //    return RedirectToAction("AddRole", new { id = user.Id });
        //}
        //[HttpPost("{claimid}")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteClaimAsync(int claimid)
        //{
        //    var userclaim = _context.UserClaims.Where(c => c.Id == claimid).FirstOrDefault();
        //    var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userclaim.UserId);
        //    if (user == null) return NotFound("Không tìm thấy user");

        //    await _userManager.RemoveClaimAsync(user, new Claim(userclaim.ClaimType, userclaim.ClaimValue));

        //    StatusMessage = "Bạn đã xóa claim";

        //    return RedirectToAction("AddRole", new { id = user.Id });
        //}

        //private async Task GetClaims(AddUserRoleModel model)
        //{
        //    var listRoles = from r in _context.Roles
        //                    join ur in _context.UserRoles on r.Id equals ur.RoleId
        //                    where ur.UserId == model.user.Id
        //                    select r;

        //    var _claimsInRole = from c in _context.RoleClaims
        //                        join r in listRoles on c.RoleId equals r.Id
        //                        select c;
        //    model.claimsInRole = await _claimsInRole.ToListAsync();


        //    model.claimsInUserClaim = await (from c in _context.UserClaims
        //                                     where c.UserId == model.user.Id
        //                                     select c).ToListAsync();

        //}
    }
}
