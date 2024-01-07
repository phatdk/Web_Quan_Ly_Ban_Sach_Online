using BookShop.BLL.ConfigurationModel.OrderModel;
using BookShop.BLL.IService;
using BookShop.DAL.Entities;
using BookShop.DAL.Repositopy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace BookShop.BLL.Service
{
	public class OrderService : IOrderService
	{
		private readonly IRepository<Order> _orderRepository;
		private readonly IRepository<Userr> _userRepository;
		private readonly IRepository<Promotion> _promotionRepository;
		private readonly IRepository<OrderDetail> _orderDetailRepository;
		private readonly IRepository<StatusOrder> _statusRepository;
		private readonly IRepository<Product> _ProductRepository;

		public OrderService()
		{
			_orderRepository = new Repository<Order>();
			_userRepository = new Repository<Userr>();
			_promotionRepository = new Repository<Promotion>();
			_orderDetailRepository = new Repository<OrderDetail>();
			_statusRepository = new Repository<StatusOrder>();
		}
		public async Task<List<ViewOrder>> GetOrderByUser(int userId)
		{
			var orders = (await _orderRepository.GetAllAsync()).Where(c => c.Id_User == userId);
			var users = (await _userRepository.GetAllAsync());
			var promotions = await _promotionRepository.GetAllAsync();
			var status = await _statusRepository.GetAllAsync();
			var OrderDetails = await _orderDetailRepository.GetAllAsync();
			//   var Products = await _ProductRepository.GetAllAsync();

			var objlist = (from a in orders
						   join od in OrderDetails on a.Id equals od.Id_Order
						   join b in users on a.Id_User equals b.Id into t
						   from b1 in t.DefaultIfEmpty()
						   join d in status on a.Id_StatusOrder equals d.Id
						   join e in users on a.Id_Staff equals e.Id into j
						   from e1 in j.DefaultIfEmpty()
						   select new ViewOrder()
						   {
							   Id = a.Id,
							   Code = a.Code,
							   Phone = a.Phone,
							   Email = a.Email,
							   Receiver = a.Receiver,
							   Address = a.Address,
							   Description = a.Description,
							   CreatedDate = a.CreatedDate,
							   AcceptDate = a.AcceptDate,
							   DeliveryDate = a.DeliveryDate,
							   PaymentDate = a.PaymentDate,
							   ModifiDate = a.ModifiDate,
							   ReceiveDate = a.ReceiveDate,
							   CompleteDate = a.CompleteDate,
							   Shipfee = a.Shipfee,
							   Id_Status = a.Id_StatusOrder,
							   Status = d.Status,
							   StatusName = d.StatusName,
							   Id_User = a.Id_User,
							   UserCode = b1.Code,
							   NameUser = b1.Name,
							   Id_Staff = a.Id_Staff,
							   StaffCode = e1 == null ? "Trống" : e1.Code,
							   NameStaff = e1 == null ? "Trống" : e1.Name,
						   }).ToList();
			return objlist;
		}
		public async Task<string> GenerateCode(int length)
		{
			// Khởi tạo đối tượng Random
			Random random = new Random();

			// Tạo một chuỗi các ký tự ngẫu nhiên
			string characters = "0123456789";
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
					model.PointAmount = pointToAmount;
				}
				else
				{
					model.PointUsed = 0;
				}
				if (string.IsNullOrEmpty(model.Code))
				{
					model.Code = "OL" + await GenerateCode(8);
				}
				var obj = new Order()
				{
					Code = model.Code,
					Receiver = model.Receiver,
					Phone = model.Phone,
					Email = model.Email,
					AcceptDate = model.AcceptDate,
					DeliveryDate = model.DeliveryDate,
					ReceiveDate = model.ReceiveDate,
					PaymentDate = model.PaymentDate,
					CompleteDate = model.CompleteDate,
					ModifiDate = model.ModifiDate,
					ModifiNotes = model.ModifiNotes == null ? "" : model.ModifiNotes,
					CreatedDate = DateTime.Now,
					Description = model.Description,
					City = model.City,
					District = model.District,
					Commune = model.Commune,
					Address = model.Address,
					Shipfee = model.Shipfee,
					Id_StatusOrder = model.Id_Status,
					Id_User = model.Id_User,
					Id_Staff = model.Id_Staff,
					//thêm
					IsOnlineOrder = model.IsOnlineOrder,
					IsUsePoint = model.IsUsePoint,
					PointUsed = model.PointUsed,
					PointAmount = model.PointAmount,
				};
				var ObjStatus = await _orderRepository.CreateAsync(obj);
				if (ObjStatus != null && model.orderDetails != null)
				{
					foreach (var item in model.orderDetails)
					{
						await _orderDetailRepository.CreateAsync(new OrderDetail()
						{
							Id_Order = ObjStatus.Id,
							UserId = ObjStatus.Id_User,
							Id_Product = item.Id_Product,
							Price = item.Price,
							Quantity = item.Quantity,
						});
					}
				}
				model.Id = ObjStatus.Id;
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
			var status = await _statusRepository.GetAllAsync();
			var objlist = (from a in orders
						   join b in users on a.Id_User equals b.Id into t
						   from b1 in t.DefaultIfEmpty()
						   join d in status on a.Id_StatusOrder equals d.Id
						   join e in users on a.Id_Staff equals e.Id into j
						   from e1 in j.DefaultIfEmpty()
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
							   AcceptDate = a.AcceptDate,
							   DeliveryDate = a.DeliveryDate,
							   PaymentDate = a.PaymentDate,
							   ModifiDate = a.ModifiDate,
							   ReceiveDate = a.ReceiveDate,
							   CompleteDate = a.CompleteDate,
							   Shipfee = a.Shipfee,
							   Id_Status = a.Id_StatusOrder,
							   Status = d.Status,
							   StatusName = d.StatusName,
							   Id_User = a.Id_User,
							   UserCode = b1.Code,
							   NameUser = b1.Name,
							   Id_Staff = a.Id_Staff,
							   StaffCode = e1 == null ? "Trống" : e1.Code,
							   NameStaff = e1 == null ? "Trống" : e1.Name,
							   IsOnlineOrder = a.IsOnlineOrder,
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
			var status = await _statusRepository.GetAllAsync();
			var objlist = (from a in orders
						   join b in users on a.Id_User equals b.Id into t
						   from b1 in t.DefaultIfEmpty()
						   join d in status on a.Id_StatusOrder equals d.Id
						   join e in users on a.Id_Staff equals e.Id into j
						   from e1 in j.DefaultIfEmpty()
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
							   AcceptDate = a.AcceptDate,
							   DeliveryDate = a.DeliveryDate,
							   PaymentDate = a.PaymentDate,
							   ModifiDate = a.ModifiDate,
							   ReceiveDate = a.ReceiveDate,
							   CompleteDate = a.CompleteDate,
							   Shipfee = a.Shipfee,
							   Id_Status = a.Id_StatusOrder,
							   Status = d.Status,
							   StatusName = d.StatusName,
							   Id_User = a.Id_User,
							   UserCode = b1.Code,
							   NameUser = b1.Name,
							   Id_Staff = a.Id_Staff,
							   StaffCode = e1 == null ? "Trống" : e1.Code,
							   NameStaff = e1 == null ? "Trống" : e1.Name,
						   }).ToList();
			return objlist;
		}

		public async Task<OrderViewModel> GetById(int id)
		{
			var orders = (await _orderRepository.GetAllAsync()).Where(c => c.Id == id);
			var users = await _userRepository.GetAllAsync();
			var status = await _statusRepository.GetAllAsync();
			var objlist = (from a in orders
						   join b in users on a.Id_User equals b.Id into t
						   from b1 in t.DefaultIfEmpty()
						   join d in status on a.Id_StatusOrder equals d.Id
						   join e in users on a.Id_Staff equals e.Id into j
						   from e1 in j.DefaultIfEmpty()
						   select new OrderViewModel()
						   {
							   Id = a.Id,
							   Code = a.Code,
							   Phone = a.Phone,
							   Email = a.Email,
							   Receiver = a.Receiver,
							   Address = a.Address,
							   Description = a.Description == null ? "Trống" : a.Description,
							   CreatedDate = a.CreatedDate,
							   AcceptDate = a.AcceptDate,
							   DeliveryDate = a.DeliveryDate,
							   PaymentDate = a.PaymentDate,
							   ModifiDate = a.ModifiDate,
							   ReceiveDate = a.ReceiveDate,
							   CompleteDate = a.CompleteDate,
							   ModifiNotes = a.ModifiNotes == null ? "" : a.ModifiNotes,
							   Shipfee = a.Shipfee,
							   City = a.City,
							   District = a.District,
							   Commune = a.Commune,
							   Id_Status = a.Id_StatusOrder,
							   Status = d.Status,
							   StatusName = d.StatusName,
							   Id_User = a.Id_User,
							   UserCode = b1.Code,
							   NameUser = b1.Name,
							   Id_Staff = a.Id_Staff,
							   StaffCode = e1 == null ? "Trống" : e1.Code,
							   NameStaff = e1 == null ? "Trống" : e1.Name,
							   IsOnlineOrder = a.IsOnlineOrder,
							   IsUsePoint = a.IsUsePoint,
							   PointUsed = a.PointUsed,
							   PointAmount = a.PointAmount,
						   }).FirstOrDefault();
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
					model.PointAmount = pointToAmount;
				}
				else
				{
					model.PointUsed = 0;
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
				obj.Shipfee = model.Shipfee;
				obj.Id_Staff = model.Id_Staff;
				obj.Id_StatusOrder = model.Id_Status;
				obj.Id_User = model.Id_User;
				//thêm
				obj.IsOnlineOrder = model.IsOnlineOrder;
				obj.IsUsePoint = model.IsUsePoint;
				obj.PointUsed = model.PointUsed;
				obj.PointAmount = model.PointAmount;
				await _orderRepository.UpdateAsync(obj.Id, obj);
				return true;
			}
			catch (Exception ex) { return false; }
		}
	}
}
