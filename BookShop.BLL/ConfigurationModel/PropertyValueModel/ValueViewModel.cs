using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.ConfigurationModel.PropertyValueModel
{
	public class ValueViewModel
	{
		public int Id { get; set; }
		public string Value1 { get; set; }
		public string? Value2 { get; set; }
		public int Status { get; set; }
		public int Id_Property { get; set; }
	}
}
