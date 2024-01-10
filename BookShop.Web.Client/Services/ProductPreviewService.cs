using BookShop.BLL.ConfigurationModel.ProductModel;
using BookShop.BLL.IService;
using BookShop.BLL.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BookShop.Web.Client.Services
{
	public class ProductPreviewService
	{
		private readonly IProductService _productService;
		private readonly IOrderDetailService _orderDetailService;
		private readonly IOrderService _orderService;

		public ProductPreviewService()
		{
			_orderDetailService = new OrderDetailService();
			_productService = new ProductService();
			_orderService = new OrderService();
		}
		public async Task<bool> ChangeQuantity(int id, int quantity)
		{
			var result = await _productService.ChangeQuantity(id, quantity);
			return result;
		}

		public async Task<List<ProductViewModel>> Getproducts(int id)
		{
			return new List<ProductViewModel>();
		}

		public async Task<List<ProductViewModel>> FindProducts(string keyword)
		{
			// var result = new List<ProductViewModel>();
			var list = await _productService.GetAll(); // nang cap tim kiem
			var list1 = list.Where(x => x.Name.ToLower().Contains(keyword.ToLower())).ToList();
			return list1;
		}

		public async Task<List<ProductViewModel>> FilterProducts(int authorId, int genreId, int collectionId)
		{
			var list1 = new List<ProductViewModel>(); // bo loc kep
			if (genreId > 0 && authorId > 0)
			{
				var genreFilter = await _productService.GetByGenre(genreId);
				var authorFilter = await _productService.GetByAuthor(authorId);
				list1 = genreFilter.Intersect(authorFilter).ToList();
			}
			else if (authorId > 0) list1 = await _productService.GetByAuthor(collectionId);
			else if (genreId > 0) list1 = await _productService.GetByGenre(genreId);
			if (collectionId > 0) list1 = list1.Where(x=>x.CollectionId == collectionId).ToList();
			return list1;
		}
	}
}
