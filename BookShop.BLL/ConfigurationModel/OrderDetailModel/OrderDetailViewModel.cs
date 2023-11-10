using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.ConfigurationModel.OrderDetailModel
{
	public class OrderDetailViewModel
	{
		public int Id { get; set; }
		public int Quantity { get; set; }
		public int Price { get; set; }

		// join properties
		public string? Img { get; set; }
		public string? NameProduct { get; set; }

		//foreign key
		public int Id_Order { get; set; }
		public int Id_Product { get; set; }
		public int Id_User { get; set; }
	}
}
