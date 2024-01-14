using BookShop.BLL.ConfigurationModel.BookModel;
using BookShop.BLL.ConfigurationModel.EvaluateModel;
using BookShop.BLL.ConfigurationModel.ImageModel;
using BookShop.BLL.ConfigurationModel.PromotionModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.ConfigurationModel.ProductModel
{
	public class ProductViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int Quantity { get; set; }
		public int Price { get; set; }
		public string Description { get; set; }
		public DateTime? CreatedDate { get; set; }
		public int Status { get; set; }
		public int Type { get; set; }
		public int? CollectionId { get; set; }
		public int ?Saleoff { get;set; }
		public int NewPrice { get; set; }

		// join
		public string? CollectionName { get; set; }
		public string? ImgUrl { get; set; }
		public int BuyQuantity { get; set; }
		public List<BookViewModel> bookViewModels { get; set; }
		public List<ImageViewModel> imageViewModels { get; set; }
		public List<PromotionViewModel> promotionViewModels { get; set; }
		public List<EvaluateViewModel>? Comment { get; set; }
	}
}
