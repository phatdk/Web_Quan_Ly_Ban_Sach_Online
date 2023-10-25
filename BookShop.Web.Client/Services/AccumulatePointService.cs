using BookShop.BLL.IService;
using BookShop.BLL.Service;
using BookShop.DAL.Entities;

namespace BookShop.Web.Client.Services
{
	public class AccumulatePointService
	{
		private readonly IUserService _userService;
		private readonly IWalletpointService _walletpointService;
		private readonly IPointTranHistoryService _pointTranHistoryService;

		public AccumulatePointService()
		{
			_userService = new UserService();
			_walletpointService = new WalletPointService();
			_pointTranHistoryService = new PointTranHistoryService();
		}

		public async Task<bool> Accumulate(int userId, int pointAmount)
		{
			if (userId != 0)
			{
				var obj = new WalletPoint()
				{
					Point = pointAmount,
				};
				var result = await _walletpointService.Update(userId, obj);
				return result;
			}
			return false;
		}
		
	}
}
