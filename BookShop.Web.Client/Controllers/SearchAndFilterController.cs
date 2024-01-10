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
		protected readonly IAuthorService _authorService;
		protected readonly ISupplierService _supplierService;
		
		public SearchAndFilterController(
			ISupplierService supplierService,
			IAuthorService authorService,
			UserManager<Userr> userManager,
			IGenreService genreService,
			IBookService bookService,
			IProductBookService productBookService,
			IProductService productService)
		{
			_supplierService = supplierService;
			_authorService = authorService;

			_products = new List<ProductViewModel>();
			_userManager = userManager;
			_genreService = genreService;
			_bookService = bookService;
			_productBookService = productBookService;
			_productService = productService;

		}
		public async Task<IActionResult> Index(int? id)
		{
			var author = (await _authorService.Getall()).Where(x => x.Status == 1).ToList();
			if (id != null)
			{
				var genre = (await _genreService.GetAll()).Where(x => x.Status == 1 && x.Id_Category == id).ToList();
				ViewBag.Genre = genre;
			}
			else
			{
				var genreNotIf = (await _genreService.GetAll()).Where(x => x.Status == 1).ToList();
                ViewBag.Genre = genreNotIf;
            }
			
			var supplier = (await _supplierService.GetAll()).Where(c=>c.Status ==1).ToList();
			ViewBag.Author = author;
		
			ViewBag.Supplier = supplier;
			return View();
		}
		[HttpGet("SearchAndFilter/Index")]
		public async Task<IActionResult> Getdata(int page, string? keyWord, List<int>? gennerId, int? categoriId, int? colectionId, List<int>? authorId ,int? min)
		{
			
			if (!authorId.Contains(0) && authorId.Count > 0)
			{
				foreach (var item in authorId)
				{
					_products = await _productService.Search(0, 0, 0, item);
				}
			}
			if (!gennerId.Contains(0) && gennerId.Count > 0)
			{
				foreach (var item in gennerId)
				{
					_products = await _productService.Search(item, 0, 0, 0);
				}
			}
			else
			{
				_products = await _productService.GetAll();
			}
			if (keyWord != null)
			{
				_products = _products.Where(c => c.Name.Contains(keyWord)).ToList();
			}
			if (min != null )
			{
				_products =  _products.Where(c => c.Price > min).ToList();
			}

			var productListSearch = _products.OrderByDescending(c => c.CreatedDate).ToList();
			int pageSize = 10;
			double totalPage = (double)productListSearch.Count / pageSize;
			productListSearch = productListSearch.Skip((page - 1) * pageSize).Take(pageSize).ToList();
			return Json(new { data = productListSearch, page = page, max = Math.Ceiling(totalPage) });
		}

		[HttpGet]
		public async Task<IActionResult> GenreList()
		{
			var genre = await _genreService.GetAll();
			return Json( new { data = genre });
		}




		private Task<Userr> GetCurrentUserAsync()
		{
			return _userManager.GetUserAsync(HttpContext.User);
		}



		
	

	}
}
