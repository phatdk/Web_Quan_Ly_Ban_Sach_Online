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
		private readonly ICategoryService _categoryService;
		private readonly UserManager<Userr> _userManager;
		private readonly PointNPromotionSerVice _pointNPromotionSerVice;
		private readonly ProductPreviewService _productPreviewService;

		public HomeController(ILogger<HomeController> logger, IProductService productService, IWishListService wishListService, ICategoryService categoryService, UserManager<Userr> userManager)
		{
			_logger = logger;
			_wishList = new List<WishListViewModel>();
			_products = new List<ProductViewModel>();
			_product = new ProductViewModel();
			_categoryService = categoryService;
			_productService = productService;
			_WishListService = wishListService;
			_userManager = userManager;
			_pointNPromotionSerVice = new PointNPromotionSerVice();
			_productPreviewService = new ProductPreviewService();
		}

		public async Task<IActionResult> Index()
		{
			return View();
		}
		public async Task<IActionResult> SachMoi()
		{
			_products = await _productService.GetDanhMuc("Cổ điển");

			var product = _products.OrderByDescending(c => c.CreatedDate).ToList();
			var top10Products = product.Take(10);
			return Json(new { data = top10Products });
		}
		public async Task<IActionResult> DanhSachSanPham()
		{
			var Products = await _productService.GetAll();
			var top10Products = Products.Take(10);

			return Json(new { data = top10Products });
		}
		public async Task<IActionResult> NuoiDayCon()
		{
			_products = await _productService.GetDanhMuc("Nuôi dạy con");

			var product1 = _products.OrderByDescending(c => c.CreatedDate).ToList();
			var top10Products1 = product1.Take(12).GroupBy(c => c.Id).Select(group => group.First()).ToList();
			return Json(new { data = top10Products1 });
		}
		public async Task<IActionResult> ChiTietSanPham(int id)
		{
			var product = await _productService.GetByIdAndCommnet(id);
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
				return Json(new { success = false, errorMessage = "Bạn cần đăng nhập để thêm sẩn phẩm vào danh sách yêu thích" });
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
						return Json(new { success = true, errorMessage = "Đã thêm sản phẩm vào danh sách yêu thích" });
					}
					catch (Exception ex)
					{
						return Json(new { success = false, });
					}
				}
				return Json(new { success = false, errorMessage = "Sản phẩm đã có trong danh sách yêu thích" });
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
			var promotionList = (await _pointNPromotionSerVice.GetActivePromotion()).Where(x => x.NameType.Equals("Phiếu khuyến mãi điểm đổi"));
			var activePromotions = new List<PromotionViewModel>();
			foreach (var item in promotionList)
			{
				DateTime startTime = Convert.ToDateTime(item.StartDate);
				DateTime endTime = Convert.ToDateTime(item.EndDate);
				int result = DateTime.Now.CompareTo(startTime);
				if (result >= 0 && endTime.CompareTo(DateTime.Now) >= 0)
				{
					activePromotions.Add(item);
				}
			}

			ViewBag.Promotion = activePromotions;
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
		public async Task<IActionResult> ListDanhMuc()
		{

			var cate = (await _categoryService.GetAll()).Where(c => c.Status == 1);
			var cateList = cate.OrderByDescending(c => c.CreatedDate).ToList();

			return Json(new { data = cateList });
		}
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}