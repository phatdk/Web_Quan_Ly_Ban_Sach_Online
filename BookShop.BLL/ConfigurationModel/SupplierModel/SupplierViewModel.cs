using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.ConfigurationModel.SupplierModel
{
	public class SupplierViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int Index { get; set; }
		public DateTime CreatedDate { get; set; }
		public int Status { get; set; }
	}
}
