using BookShop.BLL.ConfigurationModel.ProductModel;
using BookShop.BLL.ConfigurationModel.PromotionModel;
using BookShop.BLL.ConfigurationModel.WishListModel;
using BookShop.BLL.IService;
using BookShop.BLL.Service;
using BookShop.DAL.Entities;
using BookShop.Web.Client.Models;
using BookShop.Web.Client.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace BookShop.Web.Client.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		private List<ProductViewModel> _products;
		private List<WishListViewModel> _wishList;
		private ProductViewModel _product;
		private readonly IProductService _productService;
		private readonly IWishListService _WishListService;
		private readonly UserManager<Userr> _userManager;
		private readonly PointNPromotionSerVice _pointNPromotionSerVice;

		public HomeController(ILogger<HomeController> logger, IProductService productService, IWishListService wishListService, UserManager<Userr> userManager)
		{
			_logger = logger;
			_wishList = new List<WishListViewModel>();
			_products = new List<ProductViewModel>();
			_product = new ProductViewModel();
			_productService = productService;
			_WishListService = wishListService;
			_userManager = userManager;
			_pointNPromotionSerVice = new PointNPromotionSerVice();
		}

		public async Task<IActionResult> Index()
		{

			return View();
		}
		public async Task<IActionResult> SachMoi()
		{
			_products = await _productService.GetDanhMuc("Cổ điển");

			var product = _products.OrderByDescending(c => c.CreatedDate).ToList();
			var top10Products = product.Take(10).ToList();
			return Json(new { data = top10Products });
		}
		public async Task<IActionResult> DanhSachSanPham()
		{
			var Products = await _productService.GetAll();
			var top10Products = Products.Take(10).ToList();

			return Json(new { data = top10Products });
		}
		public async Task<IActionResult> NuoiDayCon()
		{
			_products = await _productService.GetDanhMuc("Nuôi dạy con");

			var product = _products.OrderBy(c => c.CreatedDate).ToList();
			var top10Products = product.Take(7).ToList();
			return Json(new { data = top10Products });
		}
		public async Task<IActionResult> ChiTietSanPham(int id)
		{
			var product = await _productService.GetByIdAndCommnet(id);
			//ViewBag.Product = _product;
			if (product != null)
			{
				return View(product);
			}
			else
			{
				return NotFound();
			}
		}
		private Task<Userr> GetCurrentUserAsync()
		{
			return _userManager.GetUserAsync(HttpContext.User);
		}


		public async Task<IActionResult> ThemVaoYeuThich(int id)
		{
			var user = await GetCurrentUserAsync();

			if (user == null)
			{
				return View();
			}
			else
			{
				var wishlists = await _WishListService.GetByUserId(user.Id, id);
				if (wishlists != true)
				{
					var wishlist = new CreateWishListModel()
					{
						Id_Product = id,
						Id_User = user.Id,
					};

					try
					{
						await _WishListService.Add(wishlist);
						return Json(new { success = true });
					}
					catch (Exception ex)
					{
						return Json(new { success = false });
					}
				}
				return Json(new { success = false });
			}
		}

		
		public async Task<IActionResult> XoaYeuThich(int id)
		{
			var user = await GetCurrentUserAsync();
			var delete = await _WishListService.Delete(user.Id, id);
			if (delete)
			{
				return Json(new { success = true });
			}
			return Json(new { success = false });
		}


		[HttpPost]
		public async Task<ActionResult> TimKiemYeuThich(string keyword)
		{
			var user = await GetCurrentUserAsync();
			var filteredWishlist = _WishListService.Timkiem(user.Id, keyword);
			return PartialView("Danhsachyeuthich", filteredWishlist);
		}

		public async Task<IActionResult> Danhsachyeuthich()
		{
			return View();
		}

		public async Task<IActionResult> ExchangePromotion()
		{
			var user = await GetCurrentUserAsync();
			var promotionList = (await _pointNPromotionSerVice.GetActivePromotion()).Where(x => x.NameType.Equals("Phiếu khuyến mãi điểm đổi")).ToList();
			ViewBag.Promotion = promotionList != null ? promotionList : new List<PromotionViewModel>() ;
			return View();
		}


		[HttpGet]
		public async Task<IActionResult> Getdata(int page, string? keyWord)
		{
            var user = await GetCurrentUserAsync();
            _wishList = await _WishListService.GetByUser(user.Id);
			if (keyWord != null)
			{
				_wishList = _wishList.Where(c => c.Name.Contains(keyWord)).ToList();
			}

			var listwish = _wishList.OrderByDescending(c => c.CreatedDate).ToList();
			int pageSize = 10;
			double totalPage = (double)listwish.Count / pageSize;
			listwish = listwish.Skip((page - 1) * pageSize).Take(pageSize).ToList();
			return Json(new { data = listwish, page = page, max = Math.Ceiling(totalPage) });
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}