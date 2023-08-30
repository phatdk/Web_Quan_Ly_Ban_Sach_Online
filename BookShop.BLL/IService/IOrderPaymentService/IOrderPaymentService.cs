using BookShop.BLL.ConfigurationModel.OrderPaymentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.IService.IOrderPaymentService
{
	public interface IOrderPaymentService
	{
		public Task<List<OrderPaymentViewModel>> GetByOrder(int orderId);
		public Task<OrderPaymentViewModel> GetById(int id);
		public Task<bool> Add(CreateOrderPaymentModel model);
		public Task<bool> Update(int id, UpdateOrderPaymentModel model);
		public Task<bool> Delete(int id);
	}
}
