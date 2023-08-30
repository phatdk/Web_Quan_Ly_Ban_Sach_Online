using BookShop.BLL.ConfigurationModel.OrderPaymentModel;
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
    public class OrderPaymentService : IOrderPaymentService
    {
        private readonly IRepository<OrderPayment> _orderPaymentRepository;
        private readonly IRepository<PaymentForm> _paymentFormRepository;

        public OrderPaymentService()
        {
            _orderPaymentRepository = new Repository<OrderPayment>();
            _paymentFormRepository = new Repository<PaymentForm>();
        }

        public async Task<bool> Add(CreateOrderPaymentModel model)
        {
            try
            {
                var obj = new OrderPayment()
                {
                    Id_Order = model.Id_Order,
                    Id_Payment = model.Id_Payment,
                    paymentAmount = model.paymentAmount,
                    CreatedDate = DateTime.Now,
                    Status = model.Status,
                };
                await _orderPaymentRepository.CreateAsync(obj);
                return true;
            }
            catch (Exception ex) { return false; }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                await _orderPaymentRepository.RemoveAsync(id);
                return true;
            }
            catch (Exception ex) { return false; }
        }

        public async Task<OrderPaymentViewModel> GetById(int id)
        {
            var orderpayments = (await _orderPaymentRepository.GetAllAsync()).Where(c => c.Id == id);
            var payments = await _paymentFormRepository.GetAllAsync();
            var objlist = (from a in orderpayments
                           join b in payments on a.Id_Payment equals b.Id into t
                           from b in t.DefaultIfEmpty()
                           select new OrderPaymentViewModel()
                           {
                               Id = a.Id,
                               Id_Order = a.Id_Order,
                               Id_Payment = a.Id_Payment,
                               paymentAmount = a.paymentAmount,
                               CreatedDate = a.CreatedDate,
                               Status = a.Status,
                               NamePaymentForm = b.Name,
                           }).FirstOrDefault();
            return objlist;
        }

        public async Task<List<OrderPaymentViewModel>> GetByOrder(int orderId)
        {
            var orderpayments = (await _orderPaymentRepository.GetAllAsync()).Where(c => c.Id_Order == orderId);
            var payments = await _paymentFormRepository.GetAllAsync();
            var objlist = (from a in orderpayments
                           join b in payments on a.Id_Payment equals b.Id into t
                           from b in t.DefaultIfEmpty()
                           select new OrderPaymentViewModel()
                           {
                               Id = a.Id,
                               Id_Order = a.Id_Order,
                               Id_Payment = a.Id_Payment,
                               paymentAmount = a.paymentAmount,
                               CreatedDate = a.CreatedDate,
                               Status = a.Status,
                               NamePaymentForm = b.Name,
                           }).ToList();
            return objlist;
        }

        public async Task<bool> Update(int id, UpdateOrderPaymentModel model)
        {
            try
            {
                var obj = await _orderPaymentRepository.GetByIdAsync(id);
                obj.Id_Payment = model.Id_Payment;
                obj.Status = model.Status;
                obj.paymentAmount = model.paymentAmount;
                await _orderPaymentRepository.UpdateAsync(id, obj);
                return true;
            }
            catch (Exception ex) { return false; }
        }
    }
}
