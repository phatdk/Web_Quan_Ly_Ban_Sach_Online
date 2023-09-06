using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.ConfigurationModel.WishListModel
{
	public class WishListViewModel
	{
		public DateTime CreatedDate { get; set; }

		//foreign key
		public int Id_User { get; set; }
		public int Id_Product { get; set; }
	}
}
