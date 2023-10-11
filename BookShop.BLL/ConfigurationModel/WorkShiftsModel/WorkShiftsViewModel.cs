using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.ConfigurationModel.WorkShiftsModel
{
	public class WorkShiftsViewModel
	{
		public int Id { get; set; }
		public string Shift { get; set; }
		public string Time { get; set; }
		public DateTime CreatedDate { get; set; }
		public int Status { get; set; }
	}
}
