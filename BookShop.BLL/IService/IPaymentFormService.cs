using BookShop.BLL.ConfigurationModel.PaymentFormModel;
using BookShop.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.IService
{
    public interface IPaymentFormService
    {
        public Task<List<PaymentFormViewModel>> GetAll();
        public Task<PaymentFormViewModel> GetById(int id);
        public Task<bool> Add(CreatePaymentFormModel model);
        public Task<bool> Update(int id, UpdatePaymentFormModel model);
        public Task<bool> Delete(int id);
    }
}
