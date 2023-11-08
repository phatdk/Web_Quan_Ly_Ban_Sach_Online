// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using App.Areas.Identity.Models.ManageViewModels;
using App.Areas.Identity.Models.RoleViewModels;
using BookShop.DAL.ApplicationDbContext;
using BookShop.DAL.Entities;
using BookShop.DAL.Entities.Identity;
using BookShop.Web.Client.ExtendMethods;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace App.Areas.Identity.Controllers
{

    [Authorize(Roles = "Admin")]
    [Area("Identity")]
    [Route("/Role/[action]")]
    public class RoleController : Controller
    {

        private readonly ILogger<RoleController> _logger;
        private readonly RoleManager<Role> _roleManager;
        //private readonly ApplicationDbcontext _context;
        private readonly UserManager<Userr> _userManager;

        public RoleController(ILogger<RoleController> logger, RoleManager<Role> roleManager, UserManager<Userr> userManager)
        {
            _logger = logger;
            _roleManager = roleManager;
            //  _context = context;
            _userManager = userManager;
        }

        [TempData]
        public string StatusMessage { get; set; }


        public Role GetRoleIdentity(int id)
        {

            return _roleManager.Roles.FirstOrDefault(x => x.Id == id);
        }
        // GET: /Role/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {

            var r = _roleManager.Roles.ToList();
            var roles = new List<RoleModel>();
            foreach (var _r in r)
            {
                var claims = await _roleManager.GetClaimsAsync(_r);
                var claimsString = claims.Select(c => c.Type + "=" + c.Value).ToArray();

                var rm = new RoleModel()
                {
                    Name = _r.Name,
                    Id = _r.Id,
                    Claims = claimsString.ToArray()
                };
                roles.Add(rm);
            }

            return View(roles);
        }

        // GET: /Role/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Role/Create
        [HttpPost, ActionName(nameof(Create))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(CreateRoleModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var newRole = new Role() { Name = model.Name };
            var result = await _roleManager.CreateAsync(newRole);
            if (result.Succeeded)
            {
                StatusMessage = $"Bạn vừa tạo role mới: {model.Name}";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError(result);
            }
            return View();
        }

        // GET: /Role/Delete/roleid
        [HttpGet("{roleid}")]
        public async Task<IActionResult> DeleteAsync(int roleid)
        {
            if (roleid == null) return NotFound("Không tìm thấy role");
            var role = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Id == roleid);
            if (role == null)
            {
                return NotFound("Không tìm thấy role");
            }
            return View(role);
        }

        // POST: /Role/Edit/1
        [HttpPost("{roleid}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmAsync(int roleid)
        {
            if (roleid == null) return NotFound("Không tìm thấy role");
            var role = GetRoleIdentity(roleid);
            if (role == null) return NotFound("Không tìm thấy role");

            var result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded)
            {
                StatusMessage = $"Bạn vừa xóa: {role.Name}";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError(result);
            }
            return View(role);
        }

        //// GET: /Role/Edit/roleid
        [HttpGet("{roleid}")]
        public async Task<IActionResult> Edit(int roleid, EditRoleModel model)
        {
            if (roleid == null) return NotFound("Không tìm thấy role");
            var role = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Id == roleid);
            if (role == null)
            {
                return NotFound("Không tìm thấy role");
            }
            model.Name = role.Name;
            //   model.Claims = _context.RoleClaims.Where(x => x.RoleId == role.Id).ToList();


            ModelState.Clear();
            return View(model);

        }

        // POST: /Role/Edit/1
        [HttpPost("{roleid}"), ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirmAsync(int roleid, [Bind("Name")] EditRoleModel model)
        {
            if (roleid == null) return NotFound("Không tìm thấy role");
            var role = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Id == roleid);
            if (role == null)
            {
                return NotFound("Không tìm thấy role");
            }
            //  model.Claims = _context.RoleClaims.Where(x => x.RoleId == role.Id).ToList();
            //   model.role = role;
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            role.Name = model.Name;

            var result = await _roleManager.UpdateAsync(role);

            if (result.Succeeded)
            {
                StatusMessage = $"Bạn vừa đổi tên: {model.Name}";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("lỗi update");
            }

            return View(model);
        }

        //// GET: /Role/AddRoleClaim/roleid
        //[HttpGet("{roleid}")]
        //public async Task<IActionResult> AddRoleClaimAsync(int roleid)
        //{
        //    if (roleid == null) return NotFound("Không tìm thấy role");
        //    var role = GetRoleIdentity(roleid);
        //    if (role == null)
        //    {
        //        return NotFound("Không tìm thấy role");
        //    }

        //    var model = new EditClaimModel()
        //    {
        //        role = role
        //    };
        //    return View(model);
        //}

        //// POST: /Role/AddRoleClaim/roleid
        //[HttpPost("{roleid}")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> AddRoleClaimAsync(int roleid, [Bind("ClaimType", "ClaimValue")] EditClaimModel model)
        //{
        //    if (roleid == null) return NotFound("Không tìm thấy role");
        //    var role = GetRoleIdentity(roleid);
        //    if (role == null)
        //    {
        //        return NotFound("Không tìm thấy role");
        //    }
        //    model.role = role;
        //    if (!ModelState.IsValid) return View(model);


        //    if ((await _roleManager.GetClaimsAsync(role)).Any(c => c.Type == model.ClaimType && c.Value == model.ClaimValue))
        //    {
        //        ModelState.AddModelError(string.Empty, "Claim này đã có trong role");
        //        return View(model);
        //    }

        //    var newClaim = new Claim(model.ClaimType, model.ClaimValue);
        //    var result = await _roleManager.AddClaimAsync(role, newClaim);

        //    if (!result.Succeeded)
        //    {
        //        ModelState.AddModelError(result);
        //        return View(model);
        //    }

        //    StatusMessage = "Vừa thêm đặc tính (claim) mới";

        //    return RedirectToAction("Edit", new { roleid = role.Id });

        //}

        //// GET: /Role/EditRoleClaim/claimid
        //[HttpGet("{claimid:Guid}")]
        //public async Task<IActionResult> EditRoleClaim(int claimid)
        //{
        //    var claim = _context.RoleClaims.Where(c => c.Id == claimid).FirstOrDefault();
        //    if (claim == null) return NotFound("Không tìm thấy role");

        //    var role = _roleManager.Roles.Where(x => x.Id == claim.RoleId).FirstOrDefault();
        //    if (role == null) return NotFound("Không tìm thấy role");
        //    ViewBag.claimid = claimid;

        //    var Input = new EditClaimModel()
        //    {
        //        ClaimType = claim.ClaimType,
        //        ClaimValue = claim.ClaimValue,
        //        role = role
        //    };


        //    return View(Input);
        //}

        //// GET: /Role/EditRoleClaim/claimid
        //[HttpPost("{claimid:guid}")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> EditRoleClaim(int claimid, [Bind("ClaimType", "ClaimValue")] EditClaimModel Input)
        //{
        //    var claim = _context.RoleClaims.Where(c => c.Id == claimid).FirstOrDefault();
        //    if (claim == null) return NotFound("Không tìm thấy role");

        //    ViewBag.claimid = claimid;

        //    var role = _roleManager.Roles.Where(x => x.Id == claim.RoleId).FirstOrDefault();
        //    if (role == null) return NotFound("Không tìm thấy role");
        //    Input.role = role;
        //    if (!ModelState.IsValid)
        //    {
        //        return View(Input);
        //    }
        //    if (_context.RoleClaims.Any(c => c.RoleId == role.Id && c.ClaimType == Input.ClaimType && c.ClaimValue == Input.ClaimValue && c.Id != claim.Id))
        //    {
        //        ModelState.AddModelError(string.Empty, "Claim này đã có trong role");
        //        return View(Input);
        //    }


        //    claim.ClaimType = Input.ClaimType;
        //    claim.ClaimValue = Input.ClaimValue;

        //    await _context.SaveChangesAsync();

        //    StatusMessage = "Vừa cập nhật claim";

        //    return RedirectToAction("Edit", new { roleid = role.Id });
        //}
        //// POST: /Role/EditRoleClaim/claimid
        //[HttpPost("{claimid:Guid}")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteClaim(int claimid, [Bind("ClaimType", "ClaimValue")] EditClaimModel Input)
        //{
        //    var claim = _context.RoleClaims.Where(c => c.Id == claimid).FirstOrDefault();
        //    if (claim == null) return NotFound("Không tìm thấy role");

        //    var role = GetRoleIdentity(claim.RoleId);
        //    if (role == null) return NotFound("Không tìm thấy role");
        //    Input.role = role;
        //    if (!ModelState.IsValid)
        //    {
        //        return View(Input);
        //    }
        //    if (_context.RoleClaims.Any(c => c.RoleId == role.Id && c.ClaimType == Input.ClaimType && c.ClaimValue == Input.ClaimValue && c.Id != claim.Id))
        //    {
        //        ModelState.AddModelError(string.Empty, "Claim này đã có trong role");
        //        return View(Input);
        //    }


        //    await _roleManager.RemoveClaimAsync(role, new Claim(claim.ClaimType, claim.ClaimValue));

        //    StatusMessage = "Vừa xóa claim";


        //    return RedirectToAction("Edit", new { roleid = role.Id });
        //}


    }
}
