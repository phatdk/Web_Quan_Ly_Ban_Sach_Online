using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.ConfigurationModel.PointTranHistoryModel
{
	public class PointTranHistoryViewModel
	{
		public int Id { get; set; }
		public int PointUserd { get; set; }
		public int Remaining { get; set; }
		public DateTime CreatedDate { get; set; }
		public string Notif { get; set; }
		public int Id_User { get; set; }
		public int Id_Parents { get; set; }
	}
}
