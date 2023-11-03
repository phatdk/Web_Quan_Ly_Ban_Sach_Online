using BookShop.BLL.ConfigurationModel.WishListModel;
using BookShop.BLL.IService;
using BookShop.DAL.Entities;
using BookShop.DAL.Repositopy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.Service
{
	public class WishListService : IWishListService
	{
		private readonly IRepository<WishList> _repository;
		private readonly IRepository<Product> _productRepository;
		private readonly IRepository<CollectionBook> _collectionRepository;
		private readonly IRepository<Image> _imageRepository;
        public WishListService()
		{
			_productRepository = new Repository<Product>();
			_repository = new Repository<WishList>();
			_collectionRepository = new Repository<CollectionBook>();
			_imageRepository = new Repository<Image>();
		}
		public async Task<bool> Add(CreateWishListModel model)
		{
			try
			{
				var obj = new WishList()
				{
					Id_Product = model.Id_Product,
					Id_User = model.Id_User,
					CreatedDate = DateTime.Now,
				};
				await _repository.CreateAsync(obj);
				return true;
			}
			catch (Exception ex) { return false; }
		}

		public async Task<bool> Delete(int id)
		{
			try
			{
				await _repository.RemoveAsync(id);
				return true;
			}
			catch (Exception ex) { return false; }
		}

        public async Task<List<WishListViewModel>> GetByUser(int userId)
		{
			var list = (await _repository.GetAllAsync()).Where(c => c.Id_User == userId);
			var objlist = new List<WishListViewModel>();
			
			foreach (var item in list)
			{
				var product =(await _productRepository.GetAllAsync()).FirstOrDefault(c=>c.Id == item.Id_Product);
				var obj = new WishListViewModel()
				{
					Id_Product = item.Id_Product,
					Id_User = item.Id_User,
					CreatedDate = item.CreatedDate,
					Name = product.Name,
					CollectionName = (await _collectionRepository.GetAllAsync()).FirstOrDefault(c => c.Id == product.Id_Collection).Name,
					ImgUrl = (await _imageRepository.GetAllAsync()).FirstOrDefault(c=>c.Id_Product == product.Id).ImageUrl

				};
				objlist.Add(obj);
			}
			return objlist;
		}
        public async Task<List<WishListViewModel>> Timkiem(int userId, string keyword)
        {
            var matchingProducts = (await _productRepository.GetAllAsync()).Where(c => c.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();

            var objlist = new List<WishListViewModel>();

            var wishListItems = (await _repository.GetAllAsync()).Where(c => c.Id_User == userId);
            foreach (var item in wishListItems)
            {
                var product = matchingProducts.FirstOrDefault(p => p.Id == item.Id_Product);
                if (product != null)
                {
                    var obj = new WishListViewModel()
                    {
                        Id_Product = item.Id_Product,
                        Id_User = item.Id_User,
                        CreatedDate = item.CreatedDate,
                        Name = product.Name,
                        CollectionName = (await _collectionRepository.GetAllAsync()).FirstOrDefault(c => c.Id == product.Id_Collection).Name,
                        ImgUrl = (await _imageRepository.GetAllAsync()).FirstOrDefault(c => c.Id_Product == product.Id).ImageUrl
                    };
                    objlist.Add(obj);
                }
            }
            return objlist;
        }

        public async Task<WishListViewModel> GetByUserId(int userId, int productId)
		{
			var obj = (await _repository.GetAllAsync()).Where(c => c.Id_Product == productId && c.Id_User == userId).FirstOrDefault();
			return new WishListViewModel()
			{
				Id_Product = obj.Id_Product,
				Id_User = obj.Id_User,
				CreatedDate = obj.CreatedDate,
			};
		}

     
    }
}
