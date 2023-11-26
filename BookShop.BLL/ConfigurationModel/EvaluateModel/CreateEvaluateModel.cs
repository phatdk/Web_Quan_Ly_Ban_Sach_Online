using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.ConfigurationModel.EvaluateModel
{
	public class CreateEvaluateModel
	{
		public int? Point { get; set; }
		public string? Content { get; set; }

		//foreign key
		public int? Id_Parents { get; set; }
		public int Id_User { get; set; }
		public int Id_Product { get; set; }
	}
}
