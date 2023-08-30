using BookShop.BLL.ConfigurationModel.UerPromotionModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.IService.IUserPromotionService
{
	public interface IUserPromotionService
	{
		public Task<List<UserPromotionViewModel>> GetByUser(int userId);
		public Task<bool> Add(CreateUserPromotionModel model);
		public Task<bool> Update(int id);
	}
}
