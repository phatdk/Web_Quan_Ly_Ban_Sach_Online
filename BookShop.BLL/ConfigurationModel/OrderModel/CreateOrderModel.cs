using BookShop.BLL.ConfigurationModel.PaymentFormModel;
using BookShop.BLL.ConfigurationModel.ProductModel;
using BookShop.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.ConfigurationModel.OrderModel
{
	public class CreateOrderModel
	{
		public int Id { get; set; }
		public string? Receiver { get; set; }
		public string? Phone { get; set; }
		public string? Email { get; set; }
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
		public string? Address { get; set; }
		public int? Shipfee { get; set; }
		public int Total { get; set; }

        //foreign key
		public string? NameUser { get; set; }
        public int Id_User { get; set; }
        public int Id_Staff { get; set; }
		public int Id_Promotion { get; set; }
		public int Id_StatusOrder { get; set; }

        //Order
        public bool IsUsePoint { get; set; }
        public int PointUsed { get; set; }
        public int PointAmount { get; set; }

		// tinh toan
		public int Weight { get; set; }
		public int Length { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }

		public List<int> productsId { get; set; }
		public List<int> paymentsId { get; set; }

		public List<ProductViewModel> productViewModels { get; set; }
		public List<PaymentFormViewModel> paymentFormViewModels { get; set; }
    }
}
