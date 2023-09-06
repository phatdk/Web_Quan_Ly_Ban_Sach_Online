using BookShop.BLL.ConfigurationModel.SupplierModel;
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
	public class SupplierService : ISupplierService
	{
		private readonly IRepository<Supplier> _repository;
		public SupplierService()
		{
			_repository = new Repository<Supplier>();
		}
		public async Task<bool> Add(CreateSupplierModel model)
		{
			try
			{
				var obj = new Supplier()
				{
					Name = model.Name,
					Index = model.Index,
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

		public async Task<List<SupplierViewModel>> GetAll()
		{
			var list = await _repository.GetAllAsync();
			var objlist = new List<SupplierViewModel>();
			foreach (var item in list)
			{
				var obj = new SupplierViewModel()
				{
					Id = item.Id,
					Name = item.Name,
					Index = item.Index,
					CreatedDate = item.CreatedDate,
					Status = item.Status,
				};
				objlist.Add(obj);
			}
			return objlist;
		}

		public async Task<SupplierViewModel> GetById(int id)
		{
			var obj = await _repository.GetByIdAsync(id);
			return new SupplierViewModel()
			{
				Id = obj.Id,
				Name = obj.Name,
				Index = obj.Index,
				CreatedDate = obj.CreatedDate,
				Status = obj.Status,
			};
		}

		public async Task<bool> Update(int id, UpdateSuplierModel model)
		{
			try
			{
				var obj = await _repository.GetByIdAsync(id);
				obj.Name = model.Name;
				obj.Index = model.Index;
				obj.Status = model.Status;
				await _repository.UpdateAsync(id, obj);
				return true;
			}
			catch (Exception ex) { return false; }
		}
	}
}
