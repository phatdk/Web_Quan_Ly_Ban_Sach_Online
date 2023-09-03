using BookShop.BLL.ConfigurationModel.AdminModel;
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
	public class AdminService : IAdminService
	{
		private readonly IRepository<Admin> _repository;
        public AdminService()
        {
            _repository = new Repository<Admin>();
        }
        public async Task<bool> Add(CreateAdminModel model)
		{
			try
			{
				var obj = new Admin()
				{
					Name = model.Name,
					Phone = model.Phone,
					Email = model.Email,
					Password = model.Password,
					Role = model.Role,
					CreatedDate = DateTime.Now,
					Status = model.Status,
				};
				await _repository.CreateAsync(obj);
				return true;
			}catch (Exception ex) { return false; }
		}

		public async Task<bool> Delete(int id)
		{
			try
			{
				await _repository.RemoveAsync(id);
				return true;
			}catch (Exception ex) { return false; }
		}

		public async Task<List<AdminViewModel>> GetAll()
		{
			var list = await _repository.GetAllAsync();
			var objlist = new List<AdminViewModel>();
			foreach (var item in list)
			{
				var obj = new AdminViewModel()
				{
					Id = item.Id,
					Name = item.Name,
					Phone = item.Phone,
					Email = item.Email,
					Password = item.Password,
					Role = item.Role,
					CreatedDate = item.CreatedDate,
					Status = item.Status,
				};
				objlist.Add(obj);
			}
			return objlist;
		}

		public async Task<AdminViewModel> GetById(int id)
		{
			var obj = await _repository.GetByIdAsync(id);
			return new AdminViewModel()
			{
				Id = obj.Id,
				Name = obj.Name,
				Phone = obj.Phone,
				Email = obj.Email,
				Password = obj.Password,
				Role = obj.Role,
				CreatedDate = obj.CreatedDate,
				Status = obj.Status,
			};
		}

		public async Task<bool> Update(int id, UpdateAdminModel model)
		{
			try
			{
				var obj = await _repository.GetByIdAsync(id);
				obj.Name = model.Name;
				obj.Phone = model.Phone;
				obj.Email = model.Email;
				obj.Password = model.Password;
				obj.Role = model.Role;
				obj.Status = model.Status;
				await _repository.UpdateAsync(id, obj);
				return true;
			}catch(Exception ex) { return false; }
		}
	}
}
