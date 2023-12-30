using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.ConfigurationModel.ReturnOrderModel
{
	public class ReturnOrderViewModel
	{
		public int Id { get; set; }
		public string Notes { get; set; }
		public DateTime CreatedDate { get; set; }
		public int Status { get; set; }

		public int Id_Order { get; set; }
		public int? Id_OrderDetail { get; set; }
	}
}
