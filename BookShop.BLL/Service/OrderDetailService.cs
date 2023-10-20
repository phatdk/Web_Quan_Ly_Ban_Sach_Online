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
        private readonly IRepository<Image> _imageRepository;

        public OrderDetailService()
        {
            _orderDetailRepository = new Repository<OrderDetail>();
            _productRepository = new Repository<Product>();
            _imageRepository = new Repository<Image>();
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
        public async Task<bool> AddRange(CreateOrderDetailModel model, List<CartDetail>ListItem)
        {
            try
            {
                foreach (var item in ListItem)
                {
                    var obj = new OrderDetail()
                    {
                        Id_Order = model.Id_Order,
                        Id_Product = item.Id_Product,
                        Quantity = item.Quantity,
                        Price =(await _productRepository.GetByIdAsync(model.Id_Product)).Price,
                    };
                    await _orderDetailRepository.CreateAsync(obj);
                }
                
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
            List<OrderDetail> details = (await _orderDetailRepository.GetAllAsync()).Where(x => x.Id_Order == orderId).ToList();
            var products = await _productRepository.GetAllAsync();
            var objlist = (from a in details
                           join b in products on a.Id_Product equals b.Id
                           select new OrderDetailViewModel()
                           {
                               Id = a.Id,
                               Quantity = a.Quantity,
                               Price = a.Price,
                               Id_Product = a.Id_Product,
                               Id_Order = a.Id_Order,
							   Id_User = a.UserId,
                               NameProduct = b.Name,
                           }).ToList();
            foreach (var item in objlist)
            {
                item.Img = (await _imageRepository.GetAllAsync()).Where(x => x.Id_Product == item.Id_Product).FirstOrDefault()?.ImageUrl;
            }
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
