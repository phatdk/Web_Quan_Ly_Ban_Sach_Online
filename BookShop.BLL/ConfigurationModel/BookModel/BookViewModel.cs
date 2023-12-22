using BookShop.BLL.ConfigurationModel.AuthorModel;
using BookShop.BLL.ConfigurationModel.GenreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.ConfigurationModel.BookModel
{
	public class BookViewModel
	{
		public int Id { get; set; }
		public string? ISBN { get; set; }
		public string? Title { get; set; }
		public string? Description { get; set; }
		public string? Reader { get; set; }
		public string? Barcode { get; set; }
		public int? Price { get; set; }
		public int? ImportPrice { get; set; }   
		public int? Quantity { get; set; }
		public string? PageSize { get; set; }
		public int? Pages { get; set; }
		public string? Cover { get; set; }
		public string? PublicationDate { get; set; }
		public int Weight { get; set; }
		public int Widght { get; set; }
		public int Length { get; set; }
		public int Height { get; set; }
		public DateTime? CreatedDate { get; set; }
		public int? Status { get; set; }

		//join properties
		public string? CollectionName { get; set; }
		public string? SupplierName { get; set; }

		//foreign key
		public int? Id_Collection { get; set; }
		public int? Id_Supplier { get; set; }

		//public List<int> authorSelected { get; set; }
		//public List<int> genreSelected { get; set; }

		public List<string> authors { get; set; }
		public List<string> genres { get; set; }
		// list
		public List<AuthorModel.AuthorModel> authorModels { get; set; }
		public List<GenreModel.GenreModel> genreModels { get; set; }
	}
}
