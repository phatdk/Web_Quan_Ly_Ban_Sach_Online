using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.ConfigurationModel.UerPromotionModel
{
	public class UserPromotionViewModel
	{
		public int Id { get; set; }
		public DateTime? EndDate { get; set; }
		public int ReduceMax { get; set; }
		public DateTime CreatedDate { get; set; }
		public int Status { get; set; }

		//join
		public string Name { get; set; }
		public int StorageTerm { get; set; }
		public string Code { get; set; }
		public int Id_User { get; set; }
		public int Id_Promotion { get; set; }
	}
}
