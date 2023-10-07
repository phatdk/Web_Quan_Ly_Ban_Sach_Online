using BookShop.BLL.ConfigurationModel.BookModel;
using BookShop.BLL.ConfigurationModel.ImageModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.ConfigurationModel.ProductModel
{
	public class CreateProductModel
	{
		public string Name { get; set; }
		public int Quantity { get; set; }
		public int Price { get; set; }
		public string? Description { get; set; }
		public int Status { get; set; }
		public int Type { get; set; }
		public bool IsType { get; set; }
		public List<BookViewModel> bookViewModels { get; set; }
		public List<ImageViewModel> imageViewModels { get; set; }
	}
}
