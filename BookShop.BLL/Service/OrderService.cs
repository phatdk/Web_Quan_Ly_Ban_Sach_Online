using BookShop.BLL.ConfigurationModel.OrderDetailModel;
using BookShop.BLL.ConfigurationModel.OrderModel;
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
	public class OrderService : IOrderService
	{
		private readonly IRepository<Order> _orderRepository;
		private readonly IRepository<Userr> _userRepository;
		private readonly IRepository<Promotion> _promotionRepository;
		private readonly IRepository<OrderDetail> _orderDetailRepository;
		private readonly IRepository<CartDetail> _cartDetailRepository;
		private readonly IRepository<Product> _productRepository;
		private readonly IRepository<StatusOrder> _statusRepository;

		public OrderService()
		{
			_orderRepository = new Repository<Order>();
			_userRepository = new Repository<Userr>();
			_promotionRepository = new Repository<Promotion>();
			_orderDetailRepository = new Repository<OrderDetail>();
			_cartDetailRepository = new Repository<CartDetail>();
			_productRepository = new Repository<Product>();
			_statusRepository = new Repository<StatusOrder>();
		}

		public async Task<string> GenerateCode(int length)
		{
			// Khởi tạo đối tượng Random
			Random random = new Random();

			// Tạo một chuỗi các ký tự ngẫu nhiên
			string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			string code = "";
			for (int i = 0; i < length; i++)
			{
				code += characters[random.Next(characters.Length)];
			}
			var duplicate = (await _orderRepository.GetAllAsync()).Where(c => c.Code.Equals(code));
			if (!duplicate.Any())
			{
				return code;
			}
			return (await GenerateCode(length)).ToString();
		}

		public async Task<OrderViewModel> Add(OrderViewModel model)
		{
			try
			{

				if (model.IsUsePoint)
				{
					// nếu sử dụng dụng điểm thì sẽ tiến hành đổi điểm VD: 1 điểm = 1.000đ
					int pointToAmount = Convert.ToInt32(model.PointUsed * 1000);

					//
					model.PointAmount = pointToAmount;

					//Lưu lại vào Notes
					model.ModifiNotes = $"Đã sử dụng {model.PointUsed} điểm để giảm {pointToAmount}đ";
				}
				else
				{
					model.PointUsed = 0;
					model.ModifiNotes = String.Empty;
				}

				var code = await GenerateCode(10);
				var obj = new Order()
				{
					Code = code.ToString(),
					Receiver = model.Receiver,
					Phone = model.Phone,
					Email = model.Email,
					AcceptDate = model.AcceptDate,
					DeliveryDate = model.DeliveryDate,
					ReceiveDate = model.ReceiveDate,
					PaymentDate = model.PaymentDate,
					CompleteDate = model.CompleteDate,
					ModifiDate = model.ModifiDate,
					ModifiNotes = model.ModifiNotes,
					CreatedDate = DateTime.Now,
					Description = model.Description,
					City = model.City,
					District = model.District,
					Commune = model.Commune,
					Address = model.Address,
					Shipfee = model.Shipfee,
					Id_StatusOrder = model.Id_Status,
					Id_User = model.Id_User,
					Id_Promotion = model.Id_Promotion,

					//thêm
					IsOnlineOrder = model.IsOnlineOrder,
					IsUsePoint = model.IsUsePoint,
					PointUsed = model.PointUsed,
					PointAmount = model.PointAmount,
				};
				var ObjStatus = await _orderRepository.CreateAsync(obj);
				if (ObjStatus != null)
				{

					foreach (var item in model.orderDetails)
					{
						await _orderDetailRepository.CreateAsync(new OrderDetail()
						{
							Id_Order = ObjStatus.Id,
							Id_Product = item.Id_Product,
							Price = item.Price,
							Quantity = item.Quantity,
						});
					}
					model.Id = ObjStatus.Id;
				}
				return model;
			}
			catch (Exception ex) { return model; }
		}

		public async Task<bool> Delete(int id)
		{
			try
			{
				await _orderRepository.RemoveAsync(id);
				return true;
			}
			catch (Exception ex) { return false; }
		}

		public async Task<List<OrderViewModel>> GetAll()
		{
			var orders = await _orderRepository.GetAllAsync();
			var users = await _userRepository.GetAllAsync();
			var promotions = await _promotionRepository.GetAllAsync();
			var status = await _statusRepository.GetAllAsync();
			var objlist = (from a in orders
						   join b in users on a.Id_User equals b.Id into t
						   from b1 in t.DefaultIfEmpty()
						   join c in promotions on a.Id_Promotion equals c.Id into i
						   from c1 in i.DefaultIfEmpty()
						   join d in status on a.Id_StatusOrder equals d.Id
						   select new OrderViewModel()
						   {
							   Id = a.Id,
							   Code = a.Code,
							   Phone = a.Phone,
							   Email = a.Email,
							   Receiver = a.Receiver,
							   Address = a.Address,
							   Description = a.Description,
							   CreatedDate = a.CreatedDate,
							   Shipfee = a.Shipfee,
							   Id_User = a.Id_User,
							   Id_Promotion = a.Id_Promotion,
							   Id_Status = a.Id_StatusOrder,
							   NameUser = b1.Name,
							   NamePromotion = (c1 == null) ? "không sử dụng khuyến mãi" : c1.Name,
							   Status = d.Status,
						   }).ToList();
			foreach (var item in objlist)
			{
				var details = (await _orderDetailRepository.GetAllAsync()).Where(x => x.Id_Order == item.Id).ToList();
				foreach (var prod in details)
				{
					item.Total += (prod.Quantity * prod.Price);
				}
			}
			return objlist;
		}

		public async Task<List<OrderViewModel>> GetByUser(int userId)
		{
			var orders = (await _orderRepository.GetAllAsync()).Where(c => c.Id_User == userId);
			var users = await _userRepository.GetAllAsync();
			var promotions = await _promotionRepository.GetAllAsync();
			var status = await _statusRepository.GetAllAsync();
			var objlist = (from a in orders
						   join b in users on a.Id_User equals b.Id into t
						   from b1 in t.DefaultIfEmpty()
						   join c in promotions on a.Id_Promotion equals c.Id into i
						   from c1 in i.DefaultIfEmpty()
						   join d in status on a.Id_StatusOrder equals d.Id
						   select new OrderViewModel()
						   {
							   Id = a.Id,
							   Code = a.Code,
							   Phone = a.Phone,
							   Receiver = a.Receiver,
							   Email = a.Email,
							   CreatedDate = a.CreatedDate,
							   Description = a.Description,
							   City = a.City,
							   District = a.District,
							   Commune = a.Commune,
							   Id_User = a.Id_User,
							   Id_Promotion = a.Id_Promotion,
							   Id_Status = d.Id,
							   Status = d.Status,
							   NameUser = b1.Name,
							   NamePromotion = (c1 == null) ? "không sử dụng khuyến mãi" : c1.Name,
						   }).ToList();
			return objlist;
		}

		public async Task<bool> Update(OrderViewModel model)
		{
			try
			{
				if (model.IsUsePoint)
				{
					// nếu sử dụng dụng điểm thì sẽ tiến hành đổi điểm VD: 1 điểm = 1.000đ
					int pointToAmount = Convert.ToInt32(model.PointUsed * 1000);

					//
					model.PointAmount = pointToAmount;

					//Lưu lại vào Notes
					model.ModifiNotes = $"Đã sử dụng {model.PointUsed} điểm để giảm {pointToAmount}đ";
				}
				else
				{
					model.PointUsed = 0;
					model.ModifiNotes = String.Empty;
				}

				var obj = await _orderRepository.GetByIdAsync(model.Id);
				obj.Receiver = model.Receiver;
				obj.Phone = model.Phone;
				obj.Email = model.Email;
				obj.Address = model.Address;
				obj.AcceptDate = model.AcceptDate;
				obj.DeliveryDate = model.DeliveryDate;
				obj.ReceiveDate = model.ReceiveDate;
				obj.PaymentDate = model.PaymentDate;
				obj.CompleteDate = model.CompleteDate;
				obj.ModifiDate = model.ModifiDate;
				obj.ModifiNotes = model.ModifiNotes;
				obj.Description = model.Description;
				obj.City = model.City;
				obj.District = model.District;
				obj.Commune = model.Commune;
				obj.Id_Promotion = model.Id_Promotion;
				//thêm
				obj.IsUsePoint = model.IsUsePoint;
				obj.PointUsed = model.PointUsed;
				obj.PointAmount = model.PointAmount;
				await _orderRepository.UpdateAsync(model.Id, obj);
				return true;
			}
			catch (Exception ex) { return false; }
		}

		public async Task<OrderViewModel> GetById(int id)
		{
			var orders = (await _orderRepository.GetAllAsync()).Where(c => c.Id == id);
			var users = await _userRepository.GetAllAsync();
			var promotions = await _promotionRepository.GetAllAsync();
			var status = await _statusRepository.GetAllAsync();
			var objlist = (from a in orders
						   join b in users on a.Id_User equals b.Id into t
						   from b1 in t.DefaultIfEmpty()
						   join c in promotions on a.Id_Promotion equals c.Id into i
						   from c1 in i.DefaultIfEmpty()
						   join d in status on a.Id_StatusOrder equals d.Id
						   select new OrderViewModel()
						   {
							   Id = a.Id,
							   Code = a.Code,
							   Phone = a.Phone,
							   Receiver = a.Receiver,
							   Email = a.Email,
							   Address = a.Address,
							   Shipfee = a.Shipfee,
							   AcceptDate = a.AcceptDate,
							   CreatedDate = a.CreatedDate,
							   DeliveryDate = a.DeliveryDate,
							   ReceiveDate = a.ReceiveDate,
							   PaymentDate = a.PaymentDate,
							   CompleteDate = a.CompleteDate,
							   ModifiDate = a.ModifiDate,
							   ModifiNotes = a.ModifiNotes,
							   Description = a.Description,
							   Id_User = a.Id_User,
							   Id_Promotion = a.Id_Promotion,
							   Id_Status = d.Id,
							   Status =  d.Status,
							   NameUser = b1.Name,
							   IsOnlineOrder = a.IsOnlineOrder,
							   IsUsePoint = a.IsUsePoint,
							   PointUsed = a.PointUsed,
							   PointAmount = a.PointAmount,
							   NamePromotion = (c1 == null) ? "không sử dụng khuyến mãi" : c1.Name,
						   }).FirstOrDefault();
			return objlist;
		}

		public async Task<bool> ChangeStatus(int id, int statusId)
		{
			try
			{
				var order = await _orderRepository.GetByIdAsync(id);
				if(order != null)
				{
					order.Id_StatusOrder = statusId;
					await _orderRepository.UpdateAsync(id, order);
					return true;
				}
				return false;
			}catch { return false; }
		}
	}
}
