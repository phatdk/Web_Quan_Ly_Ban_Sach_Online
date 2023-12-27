using BookShop.BLL.ConfigurationModel.ProductModel;
using BookShop.BLL.IService;
using BookShop.DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Web.Client.Controllers
{
	public class SearchAndFilterController : Controller
	{
		protected readonly IBookService _bookService;
		protected readonly IProductBookService _productBookService;
		protected readonly IProductService _productService;
		protected readonly IGenreService _genreService;
		public SearchAndFilterController(
			IGenreService genreService,
			IBookService bookService,
			IProductBookService productBookService,
			IProductService productService)
		{
			_genreService = genreService;
			_bookService = bookService;
			_productBookService = productBookService;
			_productService = productService;

		}
		public async Task<IActionResult> Index()
		{
			var product = await _productService.GetAll();
			return View(product);
		}

		[HttpGet]
		public async Task<IActionResult> Genre()
		{
			var genre = await _genreService.GetAll();
			return Json(genre);
		}

	}
}
