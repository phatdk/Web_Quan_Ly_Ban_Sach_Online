using BookShop.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.ConfigurationModel.OrderDetailModel
{
	public class CreateOrderDetailModel
	{
		public int Quantity { get; set; }
		public int Price { get; set; }

		//foreign key
		public int Id_Order { get; set; }
		public int Id_Product { get; set; }

		
	}
}
