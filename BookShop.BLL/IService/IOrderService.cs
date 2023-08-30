using BookShop.BLL.ConfigurationModel.OrderModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.IService
{
    public interface IOrderService
    {
        public Task<List<OrderViewModel>> GetAll();
        public Task<List<OrderViewModel>> GetByUser(int userId);
        public Task<List<OrderViewModel>> GetByStatus(int status);
        public Task<OrderViewModel> GetById(int id);
        public Task<bool> Add(CreateOrderModel model);
        public Task<bool> Update(int id, UpdateOrderModel model);
        public Task<bool> Delete(int id);
    }
}
