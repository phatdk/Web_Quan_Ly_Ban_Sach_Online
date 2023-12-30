using BookShop.BLL.ConfigurationModel.ReturnOrderModel;
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
	public class ReturnOrderService : IReturnOrderService
	{
		private readonly IRepository<ReturnOrder> _repository;
		public ReturnOrderService()
		{
			_repository = new Repository<ReturnOrder>();
		}
		public async Task<bool> Add(CreateReturnOrderModel model)
		{
			try
			{
				var obj = new ReturnOrder()
				{
					Notes = model.Notes,
					CreatedDate = DateTime.Now,
					Status = model.Status,
					Id_Order = model.Id_Order,
					Id_OrderDetail = model.Id_OrderDetail,
				};
				await _repository.CreateAsync(obj);
				return true;
			}
			catch (Exception ex) { return false; }
		}

		public async Task<bool> DeleteById(int id)
		{
			try
			{
				await _repository.RemoveAsync(id);
				return true;
			}
			catch (Exception ex) { return false; }
		}

		public async Task<ReturnOrderViewModel> GetById(int id)
		{
			var obj = await _repository.GetByIdAsync(id);
			return new ReturnOrderViewModel()
			{
				Id = obj.Id,
				Notes = obj.Notes,
				CreatedDate = obj.CreatedDate,
				Status = obj.Status,
				Id_Order = obj.Id_Order,
				Id_OrderDetail = obj.Id_OrderDetail,
			};
		}

		public async Task<List<ReturnOrderViewModel>> GetByOrder(int orderId)
		{
			var list = (await _repository.GetAllAsync()).Where(c => c.Id_Order == orderId);
			var objlist = new List<ReturnOrderViewModel>();
			foreach (var item in list)
			{
				var obj = new ReturnOrderViewModel()
				{
					Id = item.Id,
					Notes = item.Notes,
					CreatedDate = item.CreatedDate,
					Status = item.Status,
					Id_Order = item.Id_Order,
					Id_OrderDetail = item.Id_OrderDetail,
				};
				objlist.Add(obj);
			}
			return objlist;
		}

		public async Task<bool> Update(int id, UpdateReturnOrderModel model)
		{
			try
			{
				var obj = await _repository.GetByIdAsync(id);
				obj.Notes = model.Notes;
				obj.Status = model.Status;
				await _repository.UpdateAsync(id, obj);
				return true;
			}
			catch (Exception ex) { return false; }
		}
	}
}
