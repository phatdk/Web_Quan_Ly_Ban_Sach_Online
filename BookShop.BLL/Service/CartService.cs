using BookShop.BLL.ConfigurationModel.CartDetailModel;
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
	public class CartService : ICartService
	{
		private readonly IRepository<Cart> _repository;
        public CartService()
        {
            _repository = new Repository<Cart>();
        }
        public async Task<bool> Add(CartViewModel model)
		{
			try
			{
				var obj = new Cart()
				{
					Id_User = model.Id_User,
					CreatedDate = DateTime.Now,
					Status = 1,
				};
				await _repository.CreateAsync(obj);
				return true;
			}catch (Exception ex) { return false; }
		}

		public async Task<bool> Delete(int userId)
		{
			try
			{
				await _repository.RemoveAsync(userId);
				return true;
			}catch (Exception ex) { return false; }
		}

		public async Task<CartViewModel> GetByUser(int userId)
		{
			var obj = await _repository.GetByIdAsync(userId);
			return new CartViewModel
			{
				Id_User = obj.Id_User,
			};
		}
	}
}
