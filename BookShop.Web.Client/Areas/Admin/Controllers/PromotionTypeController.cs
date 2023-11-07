using BookShop.BLL.ConfigurationModel.PromotionTypeModel;
using BookShop.BLL.IService;
using BookShop.DAL.Entities;
using BookShop.DAL.Entities.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Web.Client.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class PromotionTypeController : Controller
	{
		IPromotionTypeService _promotionTypeService;
		public PromotionTypeController(IPromotionTypeService promotionTypeService)
		{
			_promotionTypeService = promotionTypeService;
		}
		public async Task<IActionResult> Index()
		{
			var lstPromotionType = await _promotionTypeService.GetAll();
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
