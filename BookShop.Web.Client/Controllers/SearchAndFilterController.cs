using BookShop.BLL.ConfigurationModel.ProductModel;
using BookShop.BLL.IService;
using BookShop.BLL.Service;
using BookShop.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Web.Client.Controllers
{
	public class SearchAndFilterController : Controller
	{
		protected readonly IBookService _bookService;
		protected readonly IProductBookService _productBookService;
		protected readonly IProductService _productService;
		protected readonly IGenreService _genreService;
		private readonly UserManager<Userr> _userManager;
		private List<ProductViewModel> _products;
		public SearchAndFilterController(
			UserManager<Userr> userManager,
			IGenreService genreService,
			IBookService bookService,
			IProductBookService productBookService,
			IProductService productService)
		{
			_products = new List<ProductViewModel>();
			_userManager = userManager;
			_genreService = genreService;
			_bookService = bookService;
			_productBookService = productBookService;
			_productService = productService;

		}
		public async Task<IActionResult> Index()
		{
			return View();
		}
		[HttpGet]
		public async Task<IActionResult> Getdata(int page, string? keyWord, int? gennerId, int? categoriId, int? colectionId, int? authorId,int min )
		{
			var user = await GetCurrentUserAsync();
			_products = await _productService.Search(gennerId, categoriId, colectionId, authorId,min);

			if (keyWord != null)
			{
				_products = _products.Where(c => c.Name.Contains(keyWord)).ToList();
			}

			var productListSearch = _products.OrderByDescending(c => c.CreatedDate).ToList();
			int pageSize = 10;
			double totalPage = (double)productListSearch.Count / pageSize;
			productListSearch = productListSearch.Skip((page - 1) * pageSize).Take(pageSize).ToList();
			return Json(new { data = productListSearch, page = page, max = Math.Ceiling(totalPage) });
		}

		private Task<Userr> GetCurrentUserAsync()
		{
			return _userManager.GetUserAsync(HttpContext.User);
		}



		[HttpGet]
		public async Task<IActionResult> Genre()
		{
			var genre = await _genreService.GetAll();
			return Json(genre);
		}

	}
}
