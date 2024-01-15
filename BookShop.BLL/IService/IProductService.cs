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
		public Task<List<ProductViewModel>> GetDanhMuc(string danhmuc);
		public Task<List<ProductViewModel>> GetByAuthor(int authorId);
		public Task<List<ProductViewModel>> GetByGenre(int genreId);
		public Task<ProductViewModel> GetById(int id);
		public Task<ProductViewModel> GetByIdAndCommnet(int id);
		public Task<CreateProductModel> Add(CreateProductModel model);
		public Task<bool> Update(UpdateProductModel model);
		public Task<bool> ChangeQuantity(int id, int changeAmount);
		public Task<bool> Delete(int id);
		public Task<List<ProductViewModel>> Search(int? gennerId, int? supplierId, int? authorId);
	}
}
