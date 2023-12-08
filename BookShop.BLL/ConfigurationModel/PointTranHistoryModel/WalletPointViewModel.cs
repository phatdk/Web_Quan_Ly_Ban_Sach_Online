using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.ConfigurationModel.PointTranHistoryModel
{
	public class WalletPointViewModel
	{
		public int Id_User { get; set; }
		public int Point { get; set; }
		public DateTime CreatedDate { get; set; }
		public int Status { get; set; }

		public List<PointTranHistoryViewModel> PointTranHistorys { get; set; }
	}
}
