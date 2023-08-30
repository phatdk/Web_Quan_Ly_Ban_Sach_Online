using BookShop.BLL.IService.IWalletPointService;
using BookShop.DAL.Entities;
using BookShop.DAL.Repositopy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.Service.WalletPointService
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
			}catch (Exception ex) { return  false; }
		}

		public async Task<bool> Update(int userId, WalletPoint model)
		{
			try
			{
				await _repository.UpdateAsync(userId, model);
				return true;
			}catch (Exception ex) { return false; }
		}
	}
}
