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
        private readonly INewsService _NewService;
        private readonly IEvaluateService _EvaluateService;
        private readonly ProductPreviewService _productPreviewService;
        private readonly IPromotionService _promotionService;
        private readonly IUserPromotionService _userPromotionService;

        public HomeController(ILogger<HomeController> logger, IProductService productService, IUserPromotionService userPromotionService, IWishListService wishListService, ICategoryService categoryService, UserManager<Userr> userManager, IPromotionService promotionService, INewsService newService = null, IEvaluateService evaluateService = null)
        {
            _logger = logger;
            _wishList = new List<WishListViewModel>();
            _products = new List<ProductViewModel>();
            _product = new ProductViewModel();
            _categoryService = categoryService;
            _productService = productService;
            _WishListService = wishListService;
            _userManager = userManager;
            _userPromotionService = userPromotionService;
            _pointNPromotionSerVice = new PointNPromotionSerVice();
            _NewService = newService;
            _productPreviewService = new ProductPreviewService();
            _EvaluateService = evaluateService;
            _promotionService = promotionService;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }
        public async Task<IActionResult> News([FromQuery(Name = "p")] int currentPages)
        {
            var ListBlog = (await _NewService.GetAll()).ToList();
            int pagesize = 8;
            if (pagesize <= 0)
            {
                pagesize = 8;
            }
            int countPages = (int)Math.Ceiling((double)ListBlog.Count() / pagesize);
            if (currentPages > countPages)
            {
                currentPages = countPages;
            }
            if (currentPages < 1)
            {
                currentPages = 1;
            }

            var pagingmodel = new PagingModel()
            {
                currentpage = currentPages,
                countpages = countPages,
                generateUrl = (int? p) => Url.Action("News", "Home", new { p = p, pagesize = pagesize })
            };
            ViewBag.pagingmodel = pagingmodel;
            ListBlog = ListBlog.Skip((pagingmodel.currentpage - 1) * pagesize).Take(pagesize).ToList();
            return View(ListBlog);

		}
		public async Task<IActionResult> DetailsNew(int id)
		{
			var blog = await _NewService.GetById(id);
			if (blog==null)
			{
				return NotFound();
			}
			return View(blog);
			
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
			var Products = (await _productService.GetAll()).OrderByDescending(c => c.CreatedDate).ToList();
			var top10Products = Products.Take(10);

			return Json(new { data = top10Products });
		}
		public async Task<IActionResult> DanhSachSanPhamCombo()
		{
			var Products = (await _productService.GetAll()).Where(c => c.Type == 2).ToList();
			var top10Products = Products.Take(10).GroupBy(c => c.Id).Select(group => group.First()).ToList();

			return Json(new { data = top10Products });
		}
		public async Task<IActionResult> NuoiDayCon()
		{
			_products = (await _productService.GetDanhMuc("Nuôi dạy con")).Where(c=>c.Type ==1).ToList();

			var product1 = _products.OrderByDescending(c => c.CreatedDate).ToList();
			var top10Products1 = product1.Take(12).GroupBy(c => c.Id).Select(group => group.First()).ToList();
			return Json(new { data = top10Products1 });
		}
		public async Task<IActionResult> Manga()
		{
			_products = await _productService.GetDanhMuc("Manga");

			var product1 = _products.OrderByDescending(c => c.CreatedDate).ToList();
			var top10Products1 = product1.Take(12).GroupBy(c => c.Id).Select(group => group.First()).ToList();
			return Json(new { data = top10Products1 });
		}
		public async Task<IActionResult> SanPhamSale()

        {
            _products = (await _productService.GetAll()).Where(c => c.Saleoff > 0).ToList();

            var product1 = _products.OrderByDescending(c => c.CreatedDate).ToList();
            var top10Products1 = product1.Take(12).GroupBy(c => c.Id).Select(group => group.First()).ToList();
            return Json(new { data = top10Products1 });
        }


		public async Task<IActionResult> PhieuGiamGia()
		{
			var user = await GetCurrentUserAsync();
			var pro = (await _promotionService.GetAll()).Where(c => c.NameType == "Phiếu khuyến mãi phát hành mã").ToList();
            if (user != null)
            {
                var userPromotions = await _userPromotionService.GetByUser(user.Id);
                foreach (var item in userPromotions)
                {
                    var existingItem = pro.FirstOrDefault(c => c.Id == item.Id_Promotion);
                    if (existingItem != null)
                    {
                        pro.Remove(existingItem);
                    }
                }
            }
			var product1 = pro.OrderByDescending(c => c.CreatedDate).ToList();
			var top4 = product1.Take(4).GroupBy(c => c.Id).Select(group => group.First()).ToList();

			return Json(new { data = top4 });
		}

		public async Task<IActionResult> ChiTietSanPham(int id)
		{
			var product = await _productService.GetByIdAndCommnet(id);
			product.Comment = await _EvaluateService.GetComments(id); 
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