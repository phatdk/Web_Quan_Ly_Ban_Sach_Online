using BookShop.BLL.ConfigurationModel.PromotionModel;
using BookShop.BLL.IService;
using BookShop.BLL.Service;
using BookShop.DAL.Entities;

namespace BookShop.Web.Client.Services
{
	public class PointNPromotionSerVice
	{
		private readonly IUserService _userService;
		private readonly IWalletpointService _walletpointService;
		private readonly IPointTranHistoryService _pointTranHistoryService;
		private readonly IPromotionService _promotionService;
		private readonly IPromotionTypeService _promotionTypeService;

		public PointNPromotionSerVice()
		{
			_userService = new UserService();
			_walletpointService = new WalletPointService();
			_pointTranHistoryService = new PointTranHistoryService();
			_promotionService = new PromotionService();
			_promotionTypeService = new PromotionTypeService();
		}

		public async Task<bool> Accumulate(int userId, int pointAmount)
		{
			if (userId != 0)
			{
				var obj = new WalletPoint()
				{
					Point = pointAmount,
				};
				var result = await _walletpointService.Update(userId, obj); // tao lich su tich diêm
				return result;
			}
			return false;
		}
		
		public async Task<List<PromotionViewModel>> GetActivePromotion()
		{
			var promotions = (await _promotionService.GetAll()).Where(x => x.Status == 1);
			var activePromotions = new List<PromotionViewModel>();
			foreach (var promotion in promotions)
			{
				DateTime startTime = Convert.ToDateTime(promotion.StartDate);
				DateTime endTime = Convert.ToDateTime(promotion.EndDate);
				int result = DateTime.Now.CompareTo(startTime);
				if (result >= 0 && result < endTime.CompareTo(DateTime.Now))
				{
					activePromotions.Add(promotion);
				}
			}
			return activePromotions;
		}
	}
}
