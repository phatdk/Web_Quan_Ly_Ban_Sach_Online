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
        public string Name { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public int Type { get; set; }
        public int? CollectionId { get; set; }
        public string? CollectionName { get; set; }
        public string ImgUrl { get; set; }

    }
}
