using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.ConfigurationModel.CustomProperties
{
	public class CreatePropertityModel
	{
		public string propertyName { get; set; }

		//foreign key
		public int Id_Shop { get; set; }
	}
}
