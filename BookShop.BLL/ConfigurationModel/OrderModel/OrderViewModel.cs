using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.ConfigurationModel.OrderModel
{
	public class OrderViewModel
	{
		public int Id { get; set; }
		public string Code { get; set; }
		public string Receiver { get; set; }
		public string Phone { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime? AcceptDate { get; set; }
		public DateTime? DeliveryDate { get; set; }
		public DateTime? ReceiveDate { get; set; }
		public DateTime? PaymentDate { get; set; }
		public DateTime? CompleteDate { get; set; }
		public DateTime? ModifiDate { get; set; }
		public string? ModifiNotes { get; set; }
		public string? Description { get; set; }
		public string? City { get; set; }
		public string? District { get; set; }
		public string? Commune { get; set; }

		// join properties
		public string NameUser { get; set; }
		public string NamePromotion { get; set; }

		//foreign key
		public int Id_User { get; set; }
		public int? Id_Promotion { get; set; }
	}
}
