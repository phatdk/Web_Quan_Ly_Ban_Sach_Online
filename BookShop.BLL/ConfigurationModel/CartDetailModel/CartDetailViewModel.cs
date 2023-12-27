using BookShop.BLL.ConfigurationModel.ImageModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.ConfigurationModel.CartDetailModel
{
	public class CartDetailViewModel
	{
		public int Id { get; set; }
		public int Quantity { get; set; }
		public DateTime CreatedDate { get; set; }

		// join properties
		public string ProductName { get; set; }
		public int ProductPrice { get; set; }
		public int TotalPrice { get; set; }
		public int Status { get; set; }
		public string ImgProductCartDetail { get; set; }
        public bool IsSelected { get; set; }
        public bool IsCanceled { get; set; }
		public int SoLuongKho { get; set; }
        //foreign key
        public int Id_User { get; set; }
		public int Id_Product { get; set; }
    }
}
