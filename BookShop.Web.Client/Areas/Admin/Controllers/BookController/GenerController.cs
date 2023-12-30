using BookShop.BLL.ConfigurationModel.CategoryModel;
using BookShop.BLL.ConfigurationModel.GenreModel;
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
    public class GenerController : Controller
    {
        private readonly IGenreService _genreService;
        List<CategoryModel> _listCategory;
        GenreModel _genre;
        List<GenreModel> _genreList;
        ICategoryService _categoryService;


        public GenerController(IGenreService genreService, ICategoryService categoryService)
        {
            _genreList = new List<GenreModel>();
            _genreService = genreService;
            _genre = new GenreModel();
            _listCategory = new List<CategoryModel>();
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View();
        }

		public async Task<IActionResult> Getdata(int page, int? status, string? keyWord)
		{

			_genreList = await _genreService.GetAll();
			if (keyWord != null)
			{
				_genreList = _genreList.Where(c => c.Name.Contains(keyWord)).ToList();
			}
			if (status != null)
			{
				_genreList = _genreList.Where(c => c.Status == Convert.ToInt32(status)).ToList();
			}
			var genres = _genreList.OrderByDescending(c => c.CreatedDate).ToList();
			int pageSize = 10;
			double totalPage = (double)genres.Count / pageSize;
			genres = genres.Skip((page - 1) * pageSize).Take(pageSize).ToList();
			return Json(new { data = genres, page = page, max = Math.Ceiling(totalPage) });
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

        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            var categorys = await LoadCategory(1);
            ViewBag.Categorys = categorys;
            return View();
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create(CreateGenreModel genre)
        {
            try
            {
                var createGenerModel = new CreateGenreModel()
                {
                    Name = genre.Name,
                    Index = genre.Index,
                    CreatedDate = DateTime.Now,
                    Status = genre.Status,
                    Id_Category = genre.Id_Category == 0 ? 1 : genre.Id_Category
                };
                var result = await _genreService.Add(createGenerModel);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return View();
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            _genre = await _genreService.GetById(id);
            var _updateGenreModel = new updateGenreModel
            {
                Name = _genre.Name,
                Index = _genre.Index ?? 0,
                Status = _genre.Status ?? 0,
                Id_Category = _genre.Id_Category ?? 0
            };

            var categorys = await LoadCategory(1);
            ViewBag.Categorys = categorys;
            return View(_updateGenreModel); // Truyền ViewModel chứa cả danh sách danh mục và thông tin thể loại
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, updateGenreModel gener)
        {
            try
            {
                var updatGenerModel = new updateGenreModel()
                {
                    Name = gener.Name,
                    Index = gener.Index,
                    Status = gener.Status,
                    Id_Category = gener.Id_Category
                };
                await _genreService.Update(id, gener);
                return RedirectToAction("Index");

            }
            catch (Exception)
            {
                return View();
            }
        }

   
        public async Task<IActionResult> Details(int id)
        {
            var gener = await _genreService.GetById(id);
            return View(gener);
        }

    }

}
