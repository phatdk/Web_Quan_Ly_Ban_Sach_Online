using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.ConfigurationModel.OrderPaymentModel
{
	public class UpdateOrderPaymentModel
	{
		public int paymentAmount { get; set; }
		public int Status { get; set; }

		//foreign key
		public int Id_Payment { get; set; }
	}
}
