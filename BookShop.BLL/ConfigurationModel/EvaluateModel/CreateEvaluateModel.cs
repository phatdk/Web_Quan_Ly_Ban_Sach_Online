using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.ConfigurationModel.EvaluateModel
{
	public class CreateEvaluateModel
	{
		[Required]
		public int Point { get; set; }
		[Required]
		public string Content { get; set; }
		public int Idsp { get; set; }

		//foreign key
		public int? Id_Parents { get; set; }
		public int Id_User { get; set; }
		public int Id_Product { get; set; }
	}
}
