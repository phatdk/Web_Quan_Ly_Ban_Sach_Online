using BookShop.BLL.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Web.Client.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class AddPromotionsToProductsController : Controller
	{
		private readonly IProductService _productService;
        public AddPromotionsToProductsController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
		{
			var iDPromotion =  HttpContext.Session.GetString("PromotionId");
			return View();
		}
		[HttpGet]
		public async Task<IActionResult> GetProduct()
		{
			var Product = await _productService.GetAll();
			return Json(Product);
		}
	}
}
