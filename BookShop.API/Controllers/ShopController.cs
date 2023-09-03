using BookShop.BLL.ConfigurationModel.ShopModel;
using BookShop.BLL.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ShopController : ControllerBase
	{
		private readonly IShopService _shopService;

		public ShopController(IShopService shopService)
		{
			_shopService = shopService;
		}
		[HttpGet]
		public async Task<ShopViewModel> GetShop()
		{
			var result = (await _shopService.GetShop()).First();
			
			return result;
		}
	}
}
