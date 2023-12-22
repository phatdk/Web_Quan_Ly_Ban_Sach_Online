using BookShop.BLL.ConfigurationModel.CategoryModel;
using BookShop.BLL.IService;
using BookShop.BLL.Service;
using BookShop.DAL.Entities;
using BookShop.Web.Client.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Web.Client.Areas.Admin.Controllers.BookController
{
    [Area("Admin")]
    [Route("admin/Category")]
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet]
        [Route("listcategory")]
        public async Task<IActionResult> Index([FromQuery(Name = "p")] int currentPages)
        {
            var category = await _categoryService.GetAll();
               
            int pagesize = 10;
            if (pagesize <= 0) 
            {
                pagesize = 10;
            }
            int countPages = (int)Math.Ceiling((double)category.Count() / pagesize);
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
                generateUrl = (int? p) => Url.Action("Index", "Category", new { areas = "Admin", p = p, pagesize = pagesize })
            };
            ViewBag.pagingmodel = pagingmodel;
            category = category.Skip((pagingmodel.currentpage - 1) * pagesize).Take(pagesize).ToList();
            return View(category);
        }

        [HttpGet("Create/category")]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost("Create/category")]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                var createcategoryModel = new CreateCategoryModel
                {
                    Name = category.Name,
                    CreatedDate = category.CreatedDate,
                    Status = category.Status
                };

                await _categoryService.Add(createcategoryModel);
                return RedirectToAction("Index");
            }
            return View(category);
        }
        [HttpGet("Edit/category/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryService.GetById(id);
            return View(category);
        }
        [HttpPost("Edit/category/{id}")]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (!ModelState.IsValid)
            {
                var updateCategoryModel = new UpdateCategoryModel
                {
                    Name = category.Name,
                    Status = category.Status
                };

                await _categoryService.Update(id, updateCategoryModel);
                return RedirectToAction("Index");
            }
            return View(category);

        }

        [HttpGet("Detail/category/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var category = await _categoryService.GetById(id);
            return View(category);
        }

        //[HttpDelete("Delete/category/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _categoryService.Delete(id))
            {
                return RedirectToAction("Index");
            }
            else return BadRequest();
        }
    }
}
