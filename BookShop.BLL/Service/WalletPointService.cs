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
    public class WalletPointService : IWalletpointService
    {
        private readonly IRepository<WalletPoint> _repository;
        public WalletPointService()
        {
            _repository = new Repository<WalletPoint>();
        }

        public async Task<bool> Add(WalletPoint model)
        {
            try
            {
                await _repository.CreateAsync(model);
                return true;
            }
            catch (Exception ex) { return false; }
        }

		public async Task<WalletPoint> GetById(int userId)
		{
            return await _repository.GetByIdAsync(userId);
		}

		public async Task<bool> Update(int userId, WalletPoint model)
        {
            try
            {
                var obj = await _repository.GetByIdAsync(userId);
                obj.Point = model.Point;
                await _repository.UpdateAsync(userId, obj);
                return true;
            }
            catch (Exception ex) { return false; }
        }
    }
}
