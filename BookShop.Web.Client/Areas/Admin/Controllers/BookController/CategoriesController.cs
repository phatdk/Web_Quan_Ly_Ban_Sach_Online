using BookShop.BLL.ConfigurationModel.CategoryModel;
using BookShop.BLL.IService;
using BookShop.DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Web.Client.Areas.Admin.Controllers.BookController
{
    [Area("Admin")]
    [Route("admin/Category")]
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet]
        [Route("listcategory")]
        public async Task<IActionResult> Index()
        {
            var category = await _categoryService.GetAll();
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
