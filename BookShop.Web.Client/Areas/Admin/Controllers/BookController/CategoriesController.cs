using BookShop.BLL.ConfigurationModel.CategoryModel;
using BookShop.BLL.ConfigurationModel.OrderModel;
using BookShop.BLL.IService;
using BookShop.BLL.Service;
using BookShop.DAL.Entities;
using BookShop.Web.Client.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Web.Client.Areas.Admin.Controllers.BookController
{
    [Area("Admin")]
   
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;
		private List<CategoryModel> _categories;
		public CategoriesController(ICategoryService categoryService)
        {
            _categories = new List<CategoryModel>();
            _categoryService = categoryService;
        }
       
        public async Task<IActionResult> Index()
        {
            return View();
        }

		public async Task<IActionResult> Getdata(int page,int? status, string? keyWord)
		{

            _categories = await _categoryService.GetAll();
			if (keyWord != null)
			{
				_categories =  _categories.Where(c=>c.Name.Contains(keyWord)).ToList();
			}
			if (status != null)
			{
				_categories = _categories.Where(c => c.Status == Convert.ToInt32(status)).ToList();
			}
			var category =  _categories.OrderByDescending(c=>c.CreatedDate).ToList();
			int pageSize = 10;
			double totalPage = (double)category.Count / pageSize;
            category = category.Skip((page - 1) * pageSize).Take(pageSize).ToList();
			return Json(new { data = category, page = page, max = Math.Ceiling(totalPage) });
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
                    CreatedDate = DateTime.Now,
                    Status = category.Status
                };

                await _categoryService.Add(createcategoryModel);
                return RedirectToAction("Index");
            }
            return View(category);
        }
       
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryService.GetById(id);
            return View(category);
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Category ?category)
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
