using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.ConfigurationModel.OrderModel
{
	public class UpdateOrderModel
	{
		public string Receiver { get; set; }
		public string Phone { get; set; }
		public DateTime? AcceptDate { get; set; }
		public DateTime? DeliveryDate { get; set; }
		public DateTime? ReceiveDate { get; set; }
		public DateTime? PaymentDate { get; set; }
		public DateTime? CompleteDate { get; set; }
		public DateTime? ModifiDate { get; set; }
		public string? ModifiNotes { get; set; }
		public string? Description { get; set; }
		public int Status { get; set; }
		public int City { get; set; }
		public int District { get; set; }
		public int Commune { get; set; }
		public int Id_Promotion { get; set; }
        //Order
        public bool IsUsePoint { get; set; }
        public int PointUsed { get; set; }
        public int PointAmount { get; set; }
    }
}
