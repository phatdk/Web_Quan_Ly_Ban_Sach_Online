using BookShop.BLL.ConfigurationModel.OrderDetailModel;
using BookShop.BLL.IService;
using BookShop.DAL.Entities;
using BookShop.DAL.Repositopy;
using MailKit.Search;
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

		public async Task<bool> Add(OrderDetailViewModel model)
		{
			try
			{
				var obj = new OrderDetail()
				{
					Id_Order = model.Id_Order,
					Id_Product = model.Id_Product,
					UserId = model.Id_User,
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

        public async Task<List<OrderDetailViewModel>> GetAll()
        {
			var listOrderDetails = await _orderDetailRepository.GetAllAsync();
			var list = from x in listOrderDetails
					   select new OrderDetailViewModel()
					   {
						   Id = x.Id,
						   Id_Product = x.Id_Product,
						   Id_Order = x.Id_Order,
						   Id_User = x.UserId,
						   Quantity = x.Quantity,
						   Price = x.Price,
					   };


            return list.ToList();
        }

        public async Task<OrderDetailViewModel> GetById(int id)
		{
			var details = await _orderDetailRepository.GetByIdAsync(id);
			return new OrderDetailViewModel()
			{
				Id = details.Id,
				Id_Product = details.Id_Product,
				Id_Order = details.Id_Order,
				Id_User = details.UserId,
				Quantity = details.Quantity,
				Price = details.Price,
			};
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

		public async Task<bool> Update(int id, OrderDetailViewModel model)
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
