using BookShop.BLL.ConfigurationModel.ImageModel;
using BookShop.BLL.IService;
using BookShop.DAL.Entities;
using BookShop.DAL.Repositopy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.Service
{
	public class ImageService : IImageService
	{
		private readonly IRepository<Image> _imageRepository;
        public ImageService()
        {
            _imageRepository = new Repository<Image>();
        }
        public async Task<bool> Add(CreateImageModel model)
		{
			try
			{
				var obj = new Image()
				{
					ImageUrl = model.ImageUrl,
					Index = model.Index,
					CreatedDate = DateTime.Now,
					Status = model.Status,
					Id_Product = model.Id_Product,
				};
				await _imageRepository.CreateAsync(obj);
				return true;
			}catch (Exception ex) { return false; }
		}

		public async Task<bool> Delete(int id)
		{
			try
			{
				await _imageRepository.RemoveAsync(id);
				return true;
			}
			catch (Exception ex) { return false; }
		}

		public async Task<List<Image>> GetByProduct(int productId)
		{
			return (await _imageRepository.GetAllAsync()).Where(c=>c.Id_Product ==productId).ToList();
		}

		public async Task<bool> Update(int id, UpdateImageModel model)
		{
			try
			{
				var obj = await _imageRepository.GetByIdAsync(id);
				obj.ImageUrl = model.ImageUrl;
				await _imageRepository.UpdateAsync(id, obj);
				return true;
			}catch (Exception ex) { return false; }
		}
	}
}
