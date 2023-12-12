using BookShop.BLL.ConfigurationModel.OrderPromotionModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.IService
{
	public interface IOrderPromotionService
	{
		public Task<List<OrderPromotionViewModel>> GetAll();
		public Task<List<OrderPromotionViewModel>> GetByOrder(int orderId);
		public Task<OrderPromotionViewModel> GetById(int id);
		public Task<bool> Add(OrderPromotionViewModel model);
		public Task<bool> Update(OrderPromotionViewModel model);
		public Task<bool> Delete(int id);

	}
}
