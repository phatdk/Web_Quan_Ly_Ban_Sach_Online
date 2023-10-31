using BookShop.BLL.ConfigurationModel.WishListModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.IService
{
	public interface IWishListService
	{
        public Task<List<WishListViewModel>> GetByUser(int userId);
        public Task<List<WishListViewModel>> Timkiem(int userId ,string keyword);
        public Task<WishListViewModel> GetByUserId(int userId, int bookId);
		public Task<bool> Add(CreateWishListModel model);
		public Task<bool> Delete(int id);
	}
}
