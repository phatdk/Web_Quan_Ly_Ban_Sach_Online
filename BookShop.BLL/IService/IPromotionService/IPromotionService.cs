using BookShop.BLL.ConfigurationModel.PromotionModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.IService.IPromotionService
{
	public interface IPromotionService
	{
		public Task<List<PromotionViewModel>> GetAll();
		public Task<List<PromotionViewModel>> GetByStatus(int status);
		public Task<List<PromotionViewModel>> GetByType(int typeId);
		public Task<PromotionViewModel> GetById(int id);
		public Task<PromotionViewModel> GetByCode(string code);
		public Task<bool> Add(CreatePromotionModel model);
		public Task<bool> Update(int id, UpdatePromotionModel model);
		public Task<bool> Delete(int id);
	}
}
