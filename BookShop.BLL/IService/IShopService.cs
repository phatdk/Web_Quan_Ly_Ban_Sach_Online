using BookShop.BLL.ConfigurationModel.ShopModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.IService
{
	public interface IShopService
	{
		public Task<List<ShopViewModel>> GetShop();
		public Task<bool> Update(int id, UpdateShopModel model);
	}
}
