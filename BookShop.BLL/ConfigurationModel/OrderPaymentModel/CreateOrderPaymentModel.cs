using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.ConfigurationModel.OrderPaymentModel
{
	public class CreateOrderPaymentModel
	{
		public int paymentAmount { get; set; }
		public int Status { get; set; }

		//foreign key
		public int Id_Order { get; set; }
		public int Id_Payment { get; set; }
	}
}
