﻿using BookShop.BLL.ConfigurationModel.AuthorModel;
using BookShop.BLL.ConfigurationModel.PromotionModel;
using BookShop.BLL.IService;
using BookShop.BLL.Service;
using BookShop.DAL.ApplicationDbContext;
using BookShop.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace BookShop.Web.Client.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PromotionController : Controller
    {
        IPromotionService _promotionService;
        private readonly ApplicationDbcontext _dbcontext;
        public PromotionController(IPromotionService promotionService, ApplicationDbcontext dbcontext)
        {
            _promotionService = promotionService;
            _dbcontext = dbcontext;
        }
        public async Task<IActionResult> Index()
        {
            var lstPromotion = await _promotionService.GetAll();
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

                await _promotionService.Add(createPromotion);
                return RedirectToAction("Index");
            }
            return View(model);
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
        public async Task<IActionResult> Delete(int id)
        {


            var promotion = await _promotionService.GetById(id);
            if (promotion == null)
            {
                return NotFound();
            }
            ViewData["PromotionTypeId"] = new SelectList(_dbcontext.PromotionTypes, "Id", "Name", promotion.Id_Type);
            return View(promotion);
        }
        [HttpPost, ActionName("Delete")]
 
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var promotion = await _promotionService.GetById(id);

            await _promotionService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
