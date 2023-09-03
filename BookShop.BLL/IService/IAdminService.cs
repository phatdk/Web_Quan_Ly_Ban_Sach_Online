using BookShop.BLL.ConfigurationModel.AdminModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.IService
{
	public interface IAdminService
	{
		public Task<List<AdminViewModel>> GetAll();
		public Task<AdminViewModel> GetById(int id);
		public Task<bool> Add(CreateAdminModel model);
		public Task<bool> Update(int id, UpdateAdminModel model);
		public Task<bool> Delete(int id);
	}
}
