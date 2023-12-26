using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.ConfigurationModel.OrderPromotionModel
{
	public class OrderPromotionViewModel
	{
		public int Id { get; set; }
		public int Id_Order {  get; set; }
		public int Id_Promotion { get; set; }
		// join
		public string NamePromotion { get; set; }
		public int? AmountReduct { get; set; }
		public int? PercentReduct { get; set; }
		public int ReductMax { get; set; }
		public int TotalReduct {  get; set; }
	}
}
