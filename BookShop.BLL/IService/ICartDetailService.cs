using BookShop.BLL.ConfigurationModel.CartDetailModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.IService
{
	public interface ICartDetailService
	{
		public Task<List<CartDetailViewModel>> GetByCart(int userId);
		public Task<CartDetailViewModel> GetById(int id);
		public Task<bool> Add(CreateCartDetailModel model);
		public Task<bool> Update(int id, UpdateCartDetailModel model);
		public Task<bool> Delete(int id);
		public Task<bool> DeleteByCart(int userId);
	}
}
