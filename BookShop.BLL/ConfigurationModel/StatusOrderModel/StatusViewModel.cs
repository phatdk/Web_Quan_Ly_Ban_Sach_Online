using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.ConfigurationModel.StatusOrderModel
{
	public class StatusViewModel
	{
		public int Id { get; set; }
		public string StatusName { get; set; }
		public DateTime CreatedDate { get; set; }
		public int Status { get; set; }
	}
}
