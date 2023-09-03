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
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Promotion> _promotionRepository;

        public OrderService()
        {
            _orderRepository = new Repository<Order>();
            _userRepository = new Repository<User>();
            _promotionRepository = new Repository<Promotion>();
        }
        public async Task<string> GenerateCode(int length)
        {
            // Khởi tạo đối tượng Random
            Random random = new Random();

            // Tạo một chuỗi các ký tự ngẫu nhiên
            string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
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
            return GenerateCode(length).ToString();
        }

        public async Task<bool> Add(CreateOrderModel model)
        {
            try
            {
                var code = GenerateCode(13);
                var obj = new Order()
                {
                    Code = code.ToString(),
                    Receiver = model.Receiver,
                    Phone = model.Phone,
                    AcceptDate = model.AcceptDate,
                    DeliveryDate = model.DeliveryDate,
                    ReceiveDate = model.ReceiveDate,
                    PaymentDate = model.PaymentDate,
                    CompleteDate = model.CompleteDate,
                    ModifiDate = model.ModifiDate,
                    ModifiNotes = model.ModifiNotes,
                    CreatedDate = DateTime.Now,
                    Description = model.Description,
                    Status = model.Status,
                    City = model.City,
                    District = model.District,
                    Commune = model.Commune,
                    Id_User = model.Id_User,
                    Id_Promotion = model.Id_Promotion,
                };
                await _orderRepository.CreateAsync(obj);
                return true;
            }
            catch (Exception ex) { return false; }
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
            var objlist = (from a in orders
                           join b in users on a.Id_User equals b.Id into t
                           from b in t.DefaultIfEmpty()
                           join c in promotions on a.Id_Promotion equals c.Id into i
                           from c in i.DefaultIfEmpty()
                           select new OrderViewModel()
                           {
                               Id = a.Id,
                               Code = a.Code,
                               Phone = a.Phone,
                               Receiver = a.Receiver,
                               AcceptDate = a.AcceptDate,
                               CreatedDate = a.CreatedDate,
                               DeliveryDate = a.DeliveryDate,
                               ReceiveDate = a.ReceiveDate,
                               PaymentDate = a.PaymentDate,
                               CompleteDate = a.CompleteDate,
                               ModifiDate = a.ModifiDate,
                               ModifiNotes = a.ModifiNotes,
                               Description = a.Description,
                               Status = a.Status,
                               City = a.City,
                               District = a.District,
                               Commune = a.Commune,
                               Id_User = a.Id_User,
                               Id_Promotion = a.Id_Promotion,
                               NameUser = b.Name,
                               NamePromotion = c.Name,
                           }).ToList();
            return objlist;
        }

        public async Task<List<OrderViewModel>> GetByStatus(int status)
        {
            var orders = (await _orderRepository.GetAllAsync()).Where(c => c.Status == status);
            var users = await _userRepository.GetAllAsync();
            var promotions = await _promotionRepository.GetAllAsync();
            var objlist = (from a in orders
                           join b in users on a.Id_User equals b.Id into t
                           from b in t.DefaultIfEmpty()
                           join c in promotions on a.Id_Promotion equals c.Id into i
                           from c in i.DefaultIfEmpty()
                           select new OrderViewModel()
                           {
                               Id = a.Id,
                               Code = a.Code,
                               Phone = a.Phone,
                               Receiver = a.Receiver,
                               AcceptDate = a.AcceptDate,
                               CreatedDate = a.CreatedDate,
                               DeliveryDate = a.DeliveryDate,
                               ReceiveDate = a.ReceiveDate,
                               PaymentDate = a.PaymentDate,
                               CompleteDate = a.CompleteDate,
                               ModifiDate = a.ModifiDate,
                               ModifiNotes = a.ModifiNotes,
                               Description = a.Description,
                               Status = a.Status,
                               City = a.City,
                               District = a.District,
                               Commune = a.Commune,
                               Id_User = a.Id_User,
                               Id_Promotion = a.Id_Promotion,
                               NameUser = b.Name,
                               NamePromotion = c.Name,
                           }).ToList();
            return objlist;
        }

        public async Task<List<OrderViewModel>> GetByUser(int userId)
        {
            var orders = (await _orderRepository.GetAllAsync()).Where(c => c.Id_User == userId);
            var users = await _userRepository.GetAllAsync();
            var promotions = await _promotionRepository.GetAllAsync();
            var objlist = (from a in orders
                           join b in users on a.Id_User equals b.Id into t
                           from b in t.DefaultIfEmpty()
                           join c in promotions on a.Id_Promotion equals c.Id into i
                           from c in i.DefaultIfEmpty()
                           select new OrderViewModel()
                           {
                               Id = a.Id,
                               Code = a.Code,
                               Phone = a.Phone,
                               Receiver = a.Receiver,
                               AcceptDate = a.AcceptDate,
                               CreatedDate = a.CreatedDate,
                               DeliveryDate = a.DeliveryDate,
                               ReceiveDate = a.ReceiveDate,
                               PaymentDate = a.PaymentDate,
                               CompleteDate = a.CompleteDate,
                               ModifiDate = a.ModifiDate,
                               ModifiNotes = a.ModifiNotes,
                               Description = a.Description,
                               Status = a.Status,
                               City = a.City,
                               District = a.District,
                               Commune = a.Commune,
                               Id_User = a.Id_User,
                               Id_Promotion = a.Id_Promotion,
                               NameUser = b.Name,
                               NamePromotion = c.Name,
                           }).ToList();
            return objlist;
        }

        public async Task<bool> Update(int id, UpdateOrderModel model)
        {
            try
            {
                var obj = await _orderRepository.GetByIdAsync(id);
                obj.Receiver = model.Receiver;
                obj.Phone = model.Phone;
                obj.AcceptDate = model.AcceptDate;
                obj.DeliveryDate = model.DeliveryDate;
                obj.ReceiveDate = model.ReceiveDate;
                obj.PaymentDate = model.PaymentDate;
                obj.CompleteDate = model.CompleteDate;
                obj.ModifiDate = model.ModifiDate;
                obj.ModifiNotes = model.ModifiNotes;
                obj.Description = model.Description;
                obj.Status = model.Status;
                obj.City = model.City;
                obj.District = model.District;
                obj.Commune = model.Commune;
                obj.Id_Promotion = model.Id_Promotion;
                await _orderRepository.UpdateAsync(id, obj);
                return true;
            }
            catch (Exception ex) { return false; }
        }

        public async Task<OrderViewModel> GetById(int id)
        {
            var orders = (await _orderRepository.GetAllAsync()).Where(c => c.Id == id);
            var users = await _userRepository.GetAllAsync();
            var promotions = await _promotionRepository.GetAllAsync();
            var objlist = (from a in orders
                           join b in users on a.Id_User equals b.Id into t
                           from b in t.DefaultIfEmpty()
                           join c in promotions on a.Id_Promotion equals c.Id into i
                           from c in i.DefaultIfEmpty()
                           select new OrderViewModel()
                           {
                               Id = a.Id,
                               Code = a.Code,
                               Phone = a.Phone,
                               Receiver = a.Receiver,
                               AcceptDate = a.AcceptDate,
                               CreatedDate = a.CreatedDate,
                               DeliveryDate = a.DeliveryDate,
                               ReceiveDate = a.ReceiveDate,
                               PaymentDate = a.PaymentDate,
                               CompleteDate = a.CompleteDate,
                               ModifiDate = a.ModifiDate,
                               ModifiNotes = a.ModifiNotes,
                               Description = a.Description,
                               Status = a.Status,
                               City = a.City,
                               District = a.District,
                               Commune = a.Commune,
                               Id_User = a.Id_User,
                               Id_Promotion = a.Id_Promotion,
                               NameUser = b.Name,
                               NamePromotion = c.Name,
                           }).FirstOrDefault();
            return objlist;
        }
    }
}
