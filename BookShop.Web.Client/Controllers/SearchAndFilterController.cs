using BookShop.BLL.ConfigurationModel.ProductModel;
using BookShop.BLL.IService;
using BookShop.BLL.Service;
using BookShop.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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
        public async Task<IActionResult> Index(int? id,string? keyWord)
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

            var supplier = (await _supplierService.GetAll()).Where(c => c.Status == 1).ToList();
            ViewBag.Author = author;
            ViewBag.Supplier = supplier;

			HttpContext.Session.SetString("Keyword", keyWord ?? string.Empty);
			return View();
        }



        [HttpPost]
        public async Task<IActionResult> Getdata(int page, string? keyword, List<int>? gennerId, List<int>? colectionId, List<int>? authorId, List<int>? publishersId, int? min)
        {
            var list = new List<ProductViewModel>();
			// lấy tất cả
			string savedKeyword = HttpContext.Session.GetString("Keyword");
			if (gennerId.Count() == 0 && authorId.Count() == 0 && colectionId.Count() == 0 && publishersId.Count() == 0)
            {
				if (savedKeyword != null)
				{
					_products = (await _productService.GetAll()).GroupBy(c => c.Id).Select(group => group.First()).ToList();
					_products = _products.Where(c => c.Name.Contains(savedKeyword)).GroupBy(c => c.Id).Select(group => group.First()).ToList();
					list.AddRange(_products);
				}
                else
                {
                    _products = (await _productService.GetAll()).GroupBy(c => c.Id).Select(group => group.First()).ToList();
                list.AddRange(_products);

                }
				
            }
            else
            {
               
                if (authorId.Count > 0)
                {
                    foreach (var item in authorId)
                    {
						 _products = await _productService.Search(null, null, item);
                        list.AddRange(_products);
                    }
                }
                if (gennerId.Count > 0)
                {
                    foreach (var item in gennerId)
                    {
                        _products = await _productService.Search(item, null, null);
						list.AddRange(_products);
					}
                }
				if (publishersId.Count > 0)
				{
					foreach (var item in publishersId)
					{
						_products = await _productService.Search( null,item, null);
						list.AddRange(_products);
					}
				}
			}


            if (keyword != null)
            {
				var listnew = new List<ProductViewModel>();

				_products = _products.Where(c => c.Name.Contains(keyword)).GroupBy(c => c.Id).Select(group => group.First()).ToList();
				listnew.AddRange(_products);
				list = listnew;
			}
			
			
            if (min > 0)
            {
				var listnew = new List<ProductViewModel>();
				_products = _products.Where(c => c.Price > min).GroupBy(c => c.Id).Select(group => group.First()).ToList();
				listnew.AddRange(_products);
                list = listnew;
			}
            

			var productListSearch = list.OrderByDescending(c => c.CreatedDate).GroupBy(c => c.Id).Select(group => group.First()).ToList();
            int pageSize = 10;
            double totalPage = (double)productListSearch.Count / pageSize;
            productListSearch = productListSearch.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return Json(new { data = productListSearch, page = page, max = Math.Ceiling(totalPage) });
        }
	
		[HttpGet]
        public async Task<IActionResult> GenreList()
        {
            var genre = await _genreService.GetAll();
            return Json(new { data = genre });
        }




        private Task<Userr> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }






    }
}
