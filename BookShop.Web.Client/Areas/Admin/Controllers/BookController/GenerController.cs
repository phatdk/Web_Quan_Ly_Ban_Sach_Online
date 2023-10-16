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
        GenreModel _genre;
        ICategoryService _categoryService;


        public GenerController(IGenreService genreService, ICategoryService categoryService)
        {
            _genreService = genreService;
            _genre = new GenreModel();
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
                    CreatedDate = genre.CreatedDate,
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

        [HttpGet("Edit/genre/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            _genre = await _genreService.GetById(id);
            var _updateGenreModel = new updateGenreModel
            {
                Name = _genre.Name,
                Index = _genre.Index ?? 0,
                Id_Category = _genre.Id_Category ?? 0
            };

            var categorys = await LoadCategory(1);
            ViewBag.Categorys = categorys;
            return View(_updateGenreModel); // Truyền ViewModel chứa cả danh sách danh mục và thông tin thể loại
        }


        [HttpPost("Edit/genre/{id}")]
        public async Task<IActionResult> Edit(int id, updateGenreModel gener)
        {
            try
            {
                var updatGenerModel = new updateGenreModel()
                {
                    Name = gener.Name,
                    Index = gener.Index,
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

        [HttpGet("Detail/gener/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var gener = await _genreService.GetById(id);
            return View(gener);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var genre = await _genreService.GetById(id);
            if (genre == null)
            {
                return NotFound();
            }

            try
            {
                // Xóa thể loại theo id
                await _genreService.Delete(id);
                return RedirectToAction("Index"); // Chuyển hướng sau khi xóa thành công
            }
            catch (Exception)
            {
                return RedirectToAction("Index"); // Xử lý lỗi xóa
            }
        }
    }

}
