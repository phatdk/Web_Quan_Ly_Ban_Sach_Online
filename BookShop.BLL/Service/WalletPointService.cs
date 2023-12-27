using BookShop.BLL.ConfigurationModel.PointTranHistoryModel;
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
		private readonly IRepository<PointTransactionsHistory> _historyRepository;
		public WalletPointService()
		{
			_repository = new Repository<WalletPoint>();
			_historyRepository = new Repository<PointTransactionsHistory>();
		}

		public async Task<bool> Add(WalletPointViewModel model)
		{
			try
			{
				var obj = new WalletPoint()
				{
					Id_User = model.Id_User,
					Point = model.Point,
					Status = model.Status,
				};
				await _repository.CreateAsync(obj);
				return true;
			}
			catch (Exception ex) { return false; }
		}

		public async Task<WalletPointViewModel> GetById(int userId)
		{
			var obj = await _repository.GetByIdAsync(userId);
			var objvm = new WalletPointViewModel();
			if (obj != null)
			{
				objvm.Id_User = obj.Id_User;
				objvm.Point = obj.Point;
				obj.CreatedDate = obj.CreatedDate;
				objvm.Status = obj.Status;
			}
			return objvm;
		}

		public async Task<bool> Update(int userId, WalletPointViewModel model)
		{
			try
			{
				var obj = await _repository.GetByIdAsync(userId);
				obj.Point = model.Point;
				var result = await _repository.UpdateAsync(obj.Id_User, obj);
				//if (result != null)
				//{
				//	foreach (var item in model.PointTranHistorys)
				//	{
				//		await _historyRepository.CreateAsync(new PointTransactionsHistory()
				//		{
				//			Id_User = result.Id_User,
				//			Id_Order = item.Id_Order,
				//			Id_Promotion = item.Id_Promotion,
				//			Point_Amount_Userd = item.PointUserd,
				//			Remaining = item.Remaining,
				//			CreatedDate = DateTime.Now,
				//		});
				//	}
				//}
				return true;
			}
			catch (Exception ex) { return false; }
		}
	}
}
