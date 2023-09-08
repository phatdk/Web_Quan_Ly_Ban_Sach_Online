using BookShop.BLL.ConfigurationModel.PropertyValueModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.IService
{
	public interface IPropertyValueService
	{
		public Task<List<ValueViewModel>> GetByProperty(int propertyId);
		public Task<bool> Add(CreateValueModel model);
		public Task<bool> Update(int id, UpdateValueModel model);
		public Task<bool> Delete(int id);
	}
}
