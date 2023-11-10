using BookShop.BLL.ConfigurationModel.OrderDetailModel;
using BookShop.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.IService
{
    public interface IOrderDetailService
    {
        public Task<List<OrderDetailViewModel>> GetByOrder(int orderId);
        public Task<OrderDetailViewModel> GetById(int id);
        public Task<bool> Add(OrderDetailViewModel model);
        public Task<bool> Update(int id, OrderDetailViewModel model);
        public Task<bool> Delete(int id);
    }
}
