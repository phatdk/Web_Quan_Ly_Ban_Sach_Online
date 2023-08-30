using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.ConfigurationModel.OrderPaymentModel
{
	public class OrderPaymentViewModel
	{
		public int Id { get; set; }
		public int paymentAmount { get; set; }
		public DateTime CreatedDate { get; set; }
		public int Status { get; set; }

		// join properties
		public string NamePaymentForm { get; set; }

		//foreign key
		public int Id_Order { get; set; }
		public int Id_Payment { get; set; }
	}
}
