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
		public string Name { get; set; }
		public string Code { get; set; }
		public int? Condition { get; set; } // số tiền tối thiểu theo đơn
		public int? ConversionPoint { get; set; } // số điểm cần đổi
		public int? StorageTerm { get; set; } // thời hạn lưu trữ	
        public int? AmountReduct { get; set; } // số tiền giảm
		public int? PercentReduct { get; set; } // % giảm
		public int ReductMax { get; set; } // giảm tối da theo %
		public int Quantity { get; set; } // số lượng
		public DateTime StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public string? Description { get; set; }
		public int Status { get; set; }
		public bool StatusCheckbox { get; set; }
		public bool ReduceByAmount { get; set; }
		public bool ReduceByPercentage { get; set; }


		//foreign key
		public int Id_Type { get; set; }
	}
}
