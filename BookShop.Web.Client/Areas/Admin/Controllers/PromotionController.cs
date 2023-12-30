using BookShop.BLL.ConfigurationModel.AuthorModel;
using BookShop.BLL.ConfigurationModel.PromotionModel;
using BookShop.BLL.IService;
using BookShop.BLL.Service;
using BookShop.DAL.ApplicationDbContext;
using BookShop.DAL.Entities;
using BookShop.Web.Client.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Principal;

namespace BookShop.Web.Client.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PromotionController : Controller
    {
        IPromotionService _promotionService;
        IPromotionTypeService _promotionTypeService;
        private readonly ApplicationDbcontext _dbcontext;
        public PromotionController(IPromotionService promotionService, ApplicationDbcontext dbcontext, IPromotionTypeService promotionTypeService)
        {
            _promotionTypeService = promotionTypeService;
            _promotionService = promotionService;
            _dbcontext = dbcontext;
        }
        public async Task<IActionResult> Index([FromQuery(Name = "p")] int currentPages)
        {
            var lstPromotion = await _promotionService.GetAll();
          
            int pagesize = 10;
            if (pagesize <= 0)
            {
                pagesize = 10;
            }
            int countPages = (int)Math.Ceiling((double)lstPromotion.Count() / pagesize);
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
                generateUrl = (int? p) => Url.Action("Index", "Promotion", new { areas = "Admin", p = p, pagesize = pagesize })
            };
            ViewBag.pagingmodel = pagingmodel;
            lstPromotion = lstPromotion.Skip((pagingmodel.currentpage - 1) * pagesize).Take(pagesize).ToList();
            return View(lstPromotion);
        }
        [HttpGet("Details")]
        public async Task<IActionResult> Details(int id)
        {
            var promotion = await _promotionService.GetById(id);
            if (promotion == null)
            {
                return NotFound();
            }
            return View(promotion);
        }
        public IActionResult Create()
        {
            ViewData["LoaiKhuyenMai"] = new SelectList(_dbcontext.PromotionTypes, "Id", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Promotion model)
        {
            if (!ModelState.IsValid)
            {
                var createPromotion = new CreatePromotionModel
                {
                    Name = model.Name,
                    Code = model.Code,
                    Condition = model.Condition,
                    StorageTerm = model.StorageTerm,
                    AmountReduct = model.AmountReduct,
                    PercentReduct = model.PercentReduct,
                    ReductMax = model.ReductMax,
                    Quantity = model.Quantity,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    Description = model.Description,
                    Status = model.Status,
                    Id_Type = model.Id_Type,
                };
                if (await _promotionService.Add(createPromotion))
                {
                    var typePromotion = await _promotionTypeService.GetById(model.Id_Type);

                    if (typePromotion.Name == "Sản phẩm")
                    {
                        HttpContext.Session.SetString("PromotionId", model.Id.ToString());
                        TempData["SuccessMessage"] = "Promotion created successfully!";
                        return RedirectToAction("Index", "AddPromotionsToProducts");
                    }
                    else
                    {
                        TempData["SuccessMessage"] = "Promotion created successfully!";
                        return RedirectToAction("Index");
                    }
                }
            }

            TempData["ErrorMessage"] = "Failed to create promotion.";
            return RedirectToAction("Index");

        }
        public async Task<IActionResult> Edit(int id)
        {
            var promotion = await _promotionService.GetById(id);
            if (promotion == null)
            {
                return NotFound();
            }
            ViewData["PromotionTypeId"] = new SelectList(_dbcontext.PromotionTypes, "Id", "Name", promotion.Id_Type);
            return View(promotion);
        }
        [HttpPost]

        public async Task<IActionResult> Edit(int id, UpdatePromotionModel model)
        {
            if (ModelState.IsValid)
            {
                await _promotionService.Update(id, model);
                return RedirectToAction(nameof(Index));
            }
            ViewData["PromotionTypeId"] = new SelectList(_dbcontext.PromotionTypes, "Id", "Name", model.Id_Type);
            return View(model);
        }
		//public async Task<IActionResult> Delete(int id)
		//{


		//    var promotion = await _promotionService.GetById(id);
		//    if (promotion == null)
		//    {
		//        return NotFound();
		//    }
		//    ViewData["PromotionTypeId"] = new SelectList(_dbcontext.PromotionTypes, "Id", "Name", promotion.Id_Type);
		//    return View(promotion);
		//}

		public async Task<IActionResult> Delete(List<int> ids)
		{
			if (ids != null)
			{
				foreach (var id in ids)
				{
					await _promotionService.Delete(id);
				}
			}
			return RedirectToAction(nameof(Index));
		}
	}
}
