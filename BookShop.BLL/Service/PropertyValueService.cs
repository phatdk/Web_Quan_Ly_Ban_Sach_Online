using BookShop.BLL.ConfigurationModel.PropertyValue;
using BookShop.BLL.IService;
using BookShop.DAL.Entities;
using BookShop.DAL.Repositopy;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.Service
{
	public class PropertyValueService : IPropertyValueService
	{
		private readonly IRepository<PropertyValue> _repository;
        public PropertyValueService()
        {
            _repository = new Repository<PropertyValue>();
        }
        public async Task<bool> Add(CreateValueModel model)
		{
			try
			{
				var obj = new PropertyValue()
				{
					Value = model.Value,
					Status = model.Status,
					Id_Property = model.Id_Prperty,
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

		public async Task<List<ValueViewModel>> GetByProperty(int propertyId)
		{
			var list = (await _repository.GetAllAsync()).Where(c=>c.Id_Property == propertyId);
			var objlist = new List<ValueViewModel>();
			foreach (var item in list)
			{
				var obj = new ValueViewModel()
				{
					Value = item.Value,
					Status = item.Status,
					Id = item.Id,
					Id_Property = item.Id_Property,
				};
				objlist.Add(obj);
			}
			return objlist;
		}

		public async Task<bool> Update(int id, UpdateValueModel model)
		{
			try
			{
				var obj = await _repository.GetByIdAsync(id);
				obj.Value = model.Value;
				obj.Status = model.Status;
				await _repository.UpdateAsync(id, obj);
				return true;
			}catch (Exception ex) { return false; }
		}
	}
}
