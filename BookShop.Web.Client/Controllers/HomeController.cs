using BookShop.BLL.ConfigurationModel.ProductModel;
using BookShop.BLL.ConfigurationModel.WishListModel;
using BookShop.BLL.IService;
using BookShop.DAL.Entities;
using BookShop.Web.Client.Models;
using Microsoft.AspNetCore.Identity;
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
		private readonly IWishListService _WishListService;
        private readonly UserManager<Userr> _userManager;

        public HomeController(ILogger<HomeController> logger, IProductService productService, IWishListService wishListService, UserManager<Userr> userManager)
        {
            _logger = logger;
            _products = new List<ProductViewModel>();
            _product = new ProductViewModel();
            _productService = productService;
            _WishListService = wishListService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
		{
			ViewBag.Products = await _productService.GetAllConditional();
			return View();
		}

		public IActionResult GioHang()
		{
			return View();
		}
        public async Task<IActionResult> DanhGia()
        {
            ViewBag.Products = await _productService.GetAll();
            return View();
        }
        public async Task<IActionResult> ChiTietSanPham(int id)
        {
			_product = await _productService.GetById(id);
			ViewBag.Product = _product;
            return View(_product);
        }
        private Task<Userr> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        [HttpPost]
        public async Task<IActionResult> ThemVaoYeuThich(int ProductId)
        {
            var user = await GetCurrentUserAsync();
            var wishlists = await _WishListService.GetByUser(user.Id);
            var wishlistCheck = wishlists.FirstOrDefault(c=>c.Id_Product == ProductId);
            if (wishlistCheck == null)
            {
                var wishlist = new CreateWishListModel()
                {
                    Id_Product = ProductId,
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

        [HttpPost]
        public async Task<IActionResult> XoaYeuThich(int id)
        {
            var delete = await _WishListService.Delete(id);
            if (delete)
            {
                Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
                Response.Headers["Pragma"] = "no-cache";
                Response.Headers["Expires"] = "0";

                return Json(new { success = true });
            }
            return Json(new { success = false });
        }


        [HttpPost]
        public async Task<ActionResult> TimKiemYeuThich(string keyword)
        {
            var user = await GetCurrentUserAsync();
            var filteredWishlist =  _WishListService.Timkiem(user.Id,keyword);
             return PartialView("Danhsachyeuthich", filteredWishlist);
        }

        public async Task<IActionResult> Danhsachyeuthich()
		{
            var user = await GetCurrentUserAsync();
            var obj  = await _WishListService.GetByUser(user.Id);
			if (obj != null)
			{
				return View(obj);
			}
			return View();
		}


		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}