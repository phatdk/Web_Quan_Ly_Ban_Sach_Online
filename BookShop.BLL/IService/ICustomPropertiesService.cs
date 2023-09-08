using BookShop.BLL.ConfigurationModel.CustomPropertiesModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.IService
{
	public interface ICustomPropertiesService
	{
		public Task<List<PropertyViewModel>> GetAll();
		public Task<bool> Add(CreatePropertityModel model);
		public Task<bool> Update(int id, UpdatePropertityModel model);
		public Task<bool > Delete(int id);
	}
}
