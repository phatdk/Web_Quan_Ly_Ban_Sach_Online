using BookShop.BLL.ConfigurationModel.ProductModel;
using BookShop.BLL.IService;
using BookShop.Web.Client.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BookShop.Web.Client.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		private List<ProductViewModel> _products;
		private ProductViewModel _product;
		private readonly IProductService _productService;
		public HomeController(ILogger<HomeController> logger, IProductService productService)
		{
			_logger = logger;
			_products = new List<ProductViewModel>();
			_product = new ProductViewModel();
			_productService = productService;
		}

		public async Task<IActionResult> Index()
		{
			ViewBag.Products = await _productService.GetAll();
			return View();
		}

		public IActionResult GioHang()
		{
			return View();
		}

        public async Task<IActionResult> ChiTietSanPham(int id)
        {
			_product = await _productService.GetById(id);
			ViewBag.Product = _product;
            return View(_product);
        }

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}