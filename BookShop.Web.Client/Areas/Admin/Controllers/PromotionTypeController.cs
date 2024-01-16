using BookShop.BLL.ConfigurationModel.PromotionTypeModel;
using BookShop.BLL.IService;
using BookShop.DAL.Entities;
using BookShop.DAL.Entities.Identity;
using BookShop.Web.Client.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Web.Client.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Authorize(Roles = "Admin,Staff")]
    public class PromotionTypeController : Controller
	{
		IPromotionTypeService _promotionTypeService;
		public PromotionTypeController(IPromotionTypeService promotionTypeService)
		{
			_promotionTypeService = promotionTypeService;
		}
		public async Task<IActionResult> Index([FromQuery(Name = "p")] int currentPages)
		{
			var lstPromotionType = await _promotionTypeService.GetAll();
            int pagesize = 10;
            if (pagesize <= 0)
            {
                pagesize = 10;
            }
            int countPages = (int)Math.Ceiling((double)lstPromotionType.Count() / pagesize);
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
            lstPromotionType = lstPromotionType.Skip((pagingmodel.currentpage - 1) * pagesize).Take(pagesize).ToList();
            return View(lstPromotionType);
		}
		public async Task<IActionResult> Details(int id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var promotionType = await _promotionTypeService.GetById(id);
			if (promotionType == null)
			{
				return NotFound();
			}
			return View(promotionType);
		}
		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Create(CreatePromotionTypeModel model)
		{
			if (ModelState.IsValid)
			{
				await _promotionTypeService.Add(model);
				return RedirectToAction("Index");
			}
			return View(model);
		}
		public async Task<IActionResult> Edit(int id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var promotionType = await _promotionTypeService.GetById(id);
			if (promotionType == null)
			{
				return NotFound();
			}
			return View(promotionType);
		}
		[HttpPost]
		public async Task<IActionResult> Edit(int id, UpdatePromotiontypeModel model)
		{
			if (ModelState.IsValid)
			{
				await _promotionTypeService.Update(id, model);
				return RedirectToAction("Index");
			}
			return View(model);
		}
		public async Task<IActionResult> Delete(int id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var promotionType = await _promotionTypeService.GetById(id);
			if (promotionType == null)
			{
				return NotFound();
			}

			return View(promotionType);
		}
		[HttpPost, ActionName("Delete")]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var role = await _promotionTypeService.GetById(id);
			await _promotionTypeService.Delete(id);
			return RedirectToAction("Index");
		}
	}
}
