using BookShop.BLL.ConfigurationModel.PaymentFormModel;
using BookShop.BLL.IService.IPaymentFormService;
using BookShop.DAL.Entities;
using BookShop.DAL.Repositopy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.Service.PaymentFormService
{
	public class PaymentFormService : IPaymentFormService
	{
		private readonly IRepository<PaymentForm> _repository;

		public PaymentFormService()
		{
			_repository = new Repository<PaymentForm>();
		}

		public async Task<bool> Add(CreatePaymentFormModel model)
		{
			try
			{
				var obj = new PaymentForm()
				{
					Name = model.Name,
					CreatedDate = DateTime.Now,
					Status = model.Status,
				};
				await _repository.CreateAsync(obj);
				return true;
			}
			catch (Exception ex) { return false; }
		}

		public async Task<bool> Delete(int id)
		{
			try
			{
				await _repository.RemoveAsync(id);
				return true;
			}
			catch (Exception ex) { return false; }
		}

		public async Task<List<PaymentFormViewModel>> GetAll()
		{
			var list = await _repository.GetAllAsync();
			var objlist = new List<PaymentFormViewModel>();
			foreach (var model in list)
			{
				var obj = new PaymentFormViewModel()
				{
					Id = model.Id,
					Name = model.Name,
					CreatedDate = model.CreatedDate,
					Status = model.Status,
				};
				objlist.Add(obj);
			}
			return objlist;
		}

		public async Task<PaymentFormViewModel> GetById(int id)
		{
			var obj = await _repository.GetByIdAsync(id);
			return new PaymentFormViewModel()
			{
				Id = obj.Id,
				Name = obj.Name,
				CreatedDate = obj.CreatedDate,
				Status = obj.Status,
			};
		}

		public async Task<bool> Update(int id, UpdatePaymentFormModel model)
		{
			try
			{
				var obj = await _repository.GetByIdAsync(id);
				obj.Name = model.Name;
				obj.Status = model.Status;
				await _repository.UpdateAsync(id, obj);
				return true;
			}
			catch (Exception ex) { return false; }
		}
	}
}
