using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.ConfigurationModel.ProductBooklModel
{
	public class ProductBookViewModel
	{
		public int Id { get; set; }
		public int Status { get; set; }
		public int Id_Product { get; set; }
		public int Id_Book { get; set; }

		// join properties
		public string BookTitle { get; set; }
		public string ProductName { get; set; }
	}
}
