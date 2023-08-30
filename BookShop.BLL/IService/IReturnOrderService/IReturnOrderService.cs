using BookShop.BLL.ConfigurationModel.ReturnOrderModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.IService.IReturnOrderService
{
	public interface IReturnOrderService
	{
		public Task<List<ReturnOrderViewModel>> GetByOrder(int orderId);
		public Task<ReturnOrderViewModel> GetById(int id);
		public Task<bool> Add(CreateReturnOrderModel model);
		public Task<bool> Update(int id, UpdateReturnOrderModel model);
		public Task<bool> DeleteById(int id);
	}
}
