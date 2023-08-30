using BookShop.BLL.ConfigurationModel.OrderDetailModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.IService.IOrderDetailService
{
	public interface IOrderDetailService
	{
		public Task<List<OrderDetailViewModel>> GetByOrder(int orderId);
		public Task<bool> Add(CreateOrderDetailModel model);
		public Task<bool> Update(int id, UpdateOrderDetailModel model);
		public Task<bool> Delete(int id);
	}
}
