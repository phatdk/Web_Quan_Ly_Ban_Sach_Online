using BookShop.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.ConfigurationModel.PromotionModel
{
	public class CreatePromotionModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
		public int? Condition { get; set; }
		public int? ConversionPoint { get; set; }
		public int? StorageTerm { get; set; }
        public int? AmountReduct { get; set; }
		public int? PercentReduct { get; set; }
		public int ReductMax { get; set; }
		public int Quantity { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public string? Description { get; set; }
		public int Status { get; set; }

		//foreign key
		public int Id_Type { get; set; }
	}
}
