using BookShop.BLL.ConfigurationModel.OrderDetailModel;
using BookShop.BLL.ConfigurationModel.OrderModel;
using BookShop.DAL.Entities;
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
        public Task<OrderViewModel> GetById(int id);
        public Task<OrderViewModel> Add(OrderViewModel model);
        public Task<bool> Update(OrderViewModel model);
        public Task<bool> ChangeStatus(int id, int statusId);
        public Task<bool> Delete(int id);
    }
}
