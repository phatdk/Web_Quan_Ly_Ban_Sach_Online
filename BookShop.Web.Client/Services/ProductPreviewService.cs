using BookShop.BLL.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BookShop.Web.Client.Services
{
	public class ProductPreviewService
	{
		private readonly IProductService _productService;
		private readonly IOrderDetailService _orderDetailService;
		private readonly IOrderService _orderService;

        public ProductPreviewService(IProductService productService, IOrderDetailService orderDetailService, IOrderService orderService)
        {
            _orderDetailService = orderDetailService;
			_productService = productService;
			_orderService = orderService;
        }
        public async Task<bool> ChangeQuantity(int id, int i)
		{
			var order = await _orderService.GetById(id);
			if (order != null)
			{
				var details = await _orderDetailService.GetByOrder(id);
				foreach (var item in details)
				{
					await _productService.ChangeQuantity(item.Id_Product, i*item.Quantity);
				}
				return true;
			}
			return false;
		}
	}
}
