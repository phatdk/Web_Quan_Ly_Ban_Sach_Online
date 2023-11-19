using BookShop.BLL.ConfigurationModel.PointTranHistoryModel;
using BookShop.BLL.ConfigurationModel.PromotionModel;
using BookShop.BLL.ConfigurationModel.UerPromotionModel;
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
		private readonly IUserPromotionService _userPromotionService;

		public PointNPromotionSerVice()
		{
			_userService = new UserService();
			_walletpointService = new WalletPointService();
			_pointTranHistoryService = new PointTranHistoryService();
			_promotionService = new PromotionService();
			_promotionTypeService = new PromotionTypeService();
			_userPromotionService = new UserPromotionService();
		}

		public async Task<bool> Accumulate(int userId, int pointAmount, CreatePointTranHistoryModel model)
		{
			if (userId != 0)
			{
				var wallet = await _userService.GetById(userId);
				if (wallet != null)
				{
					var obj = new WalletPoint()
					{
						Point = wallet.Point + pointAmount,
					};
					var result = await _walletpointService.Update(userId, obj); // thay doi so diem
					if (result) // tạo lịch sử tích điểm
					{
						result = await _pointTranHistoryService.Add(model);
					}
					return result;
				}
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
				if (result >= 0 && endTime.CompareTo(DateTime.Now) >= 0)
				{
					activePromotions.Add(promotion);
				}
			}
			return activePromotions;
		}

		public async Task<bool> ModifyUserPromotion(int userId, int promotionId, int status)
		{
			var promotion = (await _userPromotionService.GetByUser(userId)).Where(x=>x.Id_Promotion == promotionId).FirstOrDefault();
			if(promotion != null)
			{
				promotion.Status = status;
				return await _userPromotionService.Update(promotion);
			}
			return false;
		}
	}
}
