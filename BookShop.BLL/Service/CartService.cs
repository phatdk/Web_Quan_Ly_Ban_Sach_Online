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
        public async Task<bool> Add(Cart model)
		{
			try
			{
				await _repository.CreateAsync(model);
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

		public async Task<Cart> GetByUser(int userId)
		{
			return await _repository.GetByIdAsync(userId);
		}
	}
}
