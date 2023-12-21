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
		public Task<List<ProductViewModel>> filter(int ct, int gr, int sls, int cbk, int au);
        public Task<List<ProductViewModel>> GetAll();
		public Task<List<ProductViewModel>> GetByAuthor(int  authorId);
		public Task<List<ProductViewModel>> GetByGenre(int genreId);
		public Task<ProductViewModel> GetById(int id);
		Task<ProductViewModel> GetByIdAndCommnet(int id);

        public Task<List<ProductViewModel>> GetByCollection(int collectionId);
		public Task<CreateProductModel> Add(CreateProductModel model);
		public Task<bool> Update(UpdateProductModel model);
		public Task<bool> ChangeQuantity(int id, int changeAmount);
		public Task<bool> Delete(int id);
	}
}
