using BookShop.BLL.ConfigurationModel.ImageModel;
using BookShop.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.IService
{
	public interface IImageService
	{
		public Task<List<Image>> GetByProduct(int productId);
		public Task<bool> Add(CreateImageModel model);
		public Task<bool> Update(UpdateImageModel model);
		public Task<bool> Delete(int id);
	}
}
