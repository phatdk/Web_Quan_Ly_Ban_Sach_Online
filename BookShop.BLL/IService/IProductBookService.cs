using BookShop.BLL.ConfigurationModel.ProductBooklModel;
using BookShop.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.IService
{
	public interface IProductBookService
	{
		public Task<List<ProductBookViewModel>> GetByProduct(int productId);
		public Task<List<ProductBookViewModel>> GetByBook(int bookId);
		public Task<bool> Add(ProductBook model);
		public Task<bool> Update(int id, ProductBook model);
		public Task<bool> Delete(int id);
	}
}
