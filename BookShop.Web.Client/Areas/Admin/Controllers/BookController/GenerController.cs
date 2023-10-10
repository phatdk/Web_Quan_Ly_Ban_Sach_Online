using BookShop.BLL.ConfigurationModel.CategoryModel;
using BookShop.BLL.ConfigurationModel.GenreModel;
using BookShop.BLL.IService;
using BookShop.DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Web.Client.Areas.Admin.Controllers.BookController
{
    [Area("Admin")] 
    [Route("admin/Gener")] 
    public class GenerController : Controller
    {
        private readonly IGenreService _genreService;
        List<CategoryModel> _listCategory;
        ICategoryService _categoryService;


        public GenerController(IGenreService genreService, List<CategoryModel> listCategory, ICategoryService categoryService)
        {
            _genreService = genreService;
            _listCategory = new List<CategoryModel>();
            _categoryService = categoryService;
        }

        [HttpGet]
        [Route("listgener")]
        public async Task<IActionResult> Index()
        {
            var gener = await _genreService.GetAll();
            return View(gener);
        }

        public async Task<List<CategoryModel>> LoadCategory(int status)
        {
            var list = await _categoryService.GetAll();
            if (status == 1)
            {
                return list.Where(p => p.Status == 1).ToList();
            }
            else
            {
                return list;
            }
        }

        [HttpGet("Create/gener")]
        public async Task<IActionResult> Create()
        {
            var listcategory = await LoadCategory(1);
            return View();
        }
        [HttpPost("Create/gener")]
        public async Task<IActionResult> Create(Genre genre)
        {
            if (!ModelState.IsValid)
            {
                var createGenerModel = new CreateGenreModel
                {
                    Name = genre.Name,
                    Index = genre.Index,
                    CreatedDate = genre.CreatedDate,
                    Id_Category = genre.Id_Category == 0 ? 0 : genre.Id_Category
                };

                await _genreService.Add(createGenerModel);
                return RedirectToAction("Index");
            }
            return View(genre);
        }
        [HttpGet("Edit/gener/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var gener = await _genreService.GetById(id);
            return View(gener);
        }
        [HttpPost("Edit/gener/{id}")]
        public async Task<IActionResult> Edit(int id, Genre gener)
        {
            if (!ModelState.IsValid)
            {
                var updatGenerModel = new updateGenreModel
                {
                    Name = gener.Name,
                    Index = gener.Index,

                };

                await _genreService.Update(id, updatGenerModel);
                return RedirectToAction("Index");
            }
            return View(gener);
        }

        [HttpGet("Detail/gener/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var gener = await _genreService.GetById(id);
            return View(gener);
        }

        //[HttpDelete("Delete/author/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _genreService.Delete(id))
            {
                return RedirectToAction("Index");
            }
            else return BadRequest();
        }
    }
}
