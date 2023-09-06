using BookShop.BLL.ConfigurationModel.WishListModel;
using BookShop.BLL.IService;
using BookShop.DAL.Entities;
using BookShop.DAL.Repositopy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.Service
{
	public class WishListService : IWishListService
	{
		private readonly IRepository<WishList> _repository;
		public WishListService()
		{
			_repository = new Repository<WishList>();
		}
		public async Task<bool> Add(CreateWishListModel model)
		{
			try
			{
				var obj = new WishList()
				{
					Id_Product = model.Id_Product,
					Id_User = model.Id_User,
					CreatedDate = DateTime.Now,
				};
				await _repository.CreateAsync(obj);
				return true;
			}
			catch (Exception ex) { return false; }
		}

		public async Task<bool> Delete(int id)
		{
			try
			{
				await _repository.RemoveAsync(id);
				return true;
			}
			catch (Exception ex) { return false; }
		}

		public async Task<List<WishListViewModel>> GetByUser(int userId)
		{
			var list = (await _repository.GetAllAsync()).Where(c => c.Id_User == userId);
			var objlist = new List<WishListViewModel>();
			foreach (var item in objlist)
			{
				var obj = new WishListViewModel()
				{
					Id_Product = item.Id_Product,
					Id_User = item.Id_User,
					CreatedDate = item.CreatedDate,
				};
				objlist.Add(obj);
			}
			return objlist;
		}

		public async Task<WishListViewModel> GetByUserId(int userId, int productId)
		{
			var obj = (await _repository.GetAllAsync()).Where(c => c.Id_Product == productId && c.Id_User == userId).FirstOrDefault();
			return new WishListViewModel()
			{
				Id_Product = obj.Id_Product,
				Id_User = obj.Id_User,
				CreatedDate = obj.CreatedDate,
			};
		}
	}
}
