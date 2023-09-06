using BookShop.BLL.ConfigurationModel.ProductModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.IService
{
	public interface IProductService
	{
		public Task<List<ProductViewModel>> GetAll();
		public Task<ProductViewModel> GetById(int id);
		public Task<bool> Add(CreateProductModel model);
		public Task<bool> Update(int id, UpdateProductModel model);
		public Task<bool> Delete(int id);
	}
}
