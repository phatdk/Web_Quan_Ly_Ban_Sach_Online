using BookShop.BLL.ConfigurationModel.CustomPropertiesModel;
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
	public class CustomPropertiesService : ICustomPropertiesService
	{
		private readonly IRepository<CustomProperties> _repository;
        public CustomPropertiesService()
        {
            _repository = new Repository<CustomProperties>();
        }
        public async Task<bool> Add(CreatePropertityModel model)
		{
			try
			{
				var obj = new CustomProperties()
				{
					propertyName = model.propertyName,
					Id_Shop = model.Id_Shop,
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
			}catch(Exception ex) { return false; }
		}

		public async Task<List<PropertyViewModel>> GetAll()
		{
			var list = await _repository.GetAllAsync();
			var objlist = new List<PropertyViewModel>();
            foreach (var item in list)
            {
				var obj = new PropertyViewModel()
				{
					Id = item.Id,
					PropertyName = item.propertyName,
					Id_Shop = item.Id_Shop,
				};
				objlist.Add(obj);
            }
			return objlist;
        }

		public async Task<bool> Update(int id, UpdatePropertityModel model)
		{
			try
			{
				var obj = await _repository.GetByIdAsync(id);
				obj.propertyName = model.PropertyName;
				await _repository.UpdateAsync(id, obj);
				return true;
			}catch { return false; }
		}
	}
}
