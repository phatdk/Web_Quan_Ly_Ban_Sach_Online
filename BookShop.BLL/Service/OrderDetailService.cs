using BookShop.BLL.ConfigurationModel.OrderDetailModel;
using BookShop.BLL.IService;
using BookShop.DAL.Entities;
using BookShop.DAL.Repositopy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.Service
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IRepository<OrderDetail> _orderDetailRepository;
        private readonly IRepository<Product> _productRepository;

        public OrderDetailService()
        {
            _orderDetailRepository = new Repository<OrderDetail>();
            _productRepository = new Repository<Product>();
        }

        public async Task<bool> Add(CreateOrderDetailModel model)
        {
            try
            {
                var obj = new OrderDetail()
                {
                    Id_Order = model.Id_Order,
                    Id_Product = model.Id_Product,
                    Quantity = model.Quantity,
                    Price = model.Price,
                };
                await _orderDetailRepository.CreateAsync(obj);
                return true;
            }
            catch (Exception ex) { return true; }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                await _orderDetailRepository.RemoveAsync(id);
                return true;
            }
            catch (Exception ex) { return false; }
        }

        public async Task<List<OrderDetailViewModel>> GetByOrder(int orderId)
        {
            var products = await _productRepository.GetAllAsync();
            var orderdetails = (await _orderDetailRepository.GetAllAsync()).Where(c => c.Id_Order == orderId);
            var objlist = (from a in orderdetails
                           join b in products on a.Id_Product equals b.Id into t
                           from b in t.DefaultIfEmpty()
                           select new OrderDetailViewModel()
                           {
                               Id = a.Id,
                               Id_Product = a.Id_Product,
                               Id_Order = a.Id_Order,
                               NameProduct = b.Name,
                               Quantity = a.Quantity,
                               Price = a.Price,
                           }).ToList();
            return objlist;
        }

        public async Task<bool> Update(int id, UpdateOrderDetailModel model)
        {
            try
            {
                var obj = await _orderDetailRepository.GetByIdAsync(id);
                obj.Quantity = model.Quantity;
                obj.Price = model.Price;
                await _orderDetailRepository.UpdateAsync(id, obj);
                return true;
            }
            catch (Exception ex) { return false; }
        }
    }
}
