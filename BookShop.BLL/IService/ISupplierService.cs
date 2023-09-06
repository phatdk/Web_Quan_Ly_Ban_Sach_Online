using BookShop.BLL.ConfigurationModel.SupplierModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.IService
{
	public interface ISupplierService
	{
		public Task<List<SupplierViewModel>> GetAll();
		public Task<SupplierViewModel> GetById(int id);
		public Task<bool> Add(CreateSupplierModel model);
		public Task<bool> Update(int id, UpdateSuplierModel model);
		public Task<bool> Delete(int id);
	}
}
