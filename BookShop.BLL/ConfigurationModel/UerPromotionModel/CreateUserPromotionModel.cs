using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.ConfigurationModel.UerPromotionModel
{
	public class CreateUserPromotionModel
	{
		public DateTime? EndDate { get; set; }
		public int Status { get; set; }
		public int Id_User { get; set; }
		public int Id_Promotion { get; set; }
	}
}
