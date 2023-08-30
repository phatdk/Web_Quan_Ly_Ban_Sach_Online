using BookShop.BLL.ConfigurationModel.PromotionTypeModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.IService.IPromotionTypeService
{
	public interface IPromotionTypeService
	{
		public Task<List<PromotionTypeViewModel>> GetAll();
		public Task<PromotionTypeViewModel> GetById(int id);
		public Task<bool> Add(CreatePromotionTypeModel model);
		public Task<bool> Update(int id, UpdatePromotiontypeModel model);
		public Task<bool> Delete(int id);
	}
}
