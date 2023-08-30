using BookShop.BLL.ConfigurationModel.ProductPromotionModel;
using BookShop.BLL.IService.IProductPromotionService;
using BookShop.DAL.Entities;
using BookShop.DAL.Repositopy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.Service.ProductPromotionService
{
	public class ProductPromotionService : IProductPromotionService
	{
		private readonly IRepository<ProductPromotion> _productPromotionRepository;
		private readonly IRepository<Product> _productRepository;
		private readonly IRepository<Promotion> _promotionRepository;
		public ProductPromotionService()
		{
			_productPromotionRepository = new Repository<ProductPromotion>();
			_productRepository = new Repository<Product>();
			_promotionRepository = new Repository<Promotion>();
		}
		public async Task<bool> Add(ProductPromotion model)
		{
			try
			{
				await _productPromotionRepository.CreateAsync(model);
				return true;
			}
			catch (Exception ex) { return false; }
		}

		public async Task<bool> Delete(int id)
		{
			try
			{
				await _productPromotionRepository.RemoveAsync(id);
				return true;
			}
			catch (Exception ex) { return false; }
		}

		public async Task<ProductPromotionViewModel> GetById(int id)
		{
			var pp = (await _productPromotionRepository.GetAllAsync()).Where(x => x.Id == id);
			var products = await _productRepository.GetAllAsync();
			var promotions = await _promotionRepository.GetAllAsync();
			var obj = (from a in pp
					   join b in products on a.Id_Product equals b.Id
					   join c in promotions on a.Id_Promotion equals c.Id
					   select new ProductPromotionViewModel()
					   {
						   Id = a.Id,
						   Id_Promotion = a.Id_Promotion,
						   Id_Product = a.Id_Product,
						   Index = a.Index,
						   CreatedDate = a.CreatedDate,
						   Status = a.Status,
						   NameProduct = b.Name,
						   NamePromotion = c.Name,
					   }).FirstOrDefault();
			return obj;
		}

		public async Task<List<ProductPromotionViewModel>> GetByProduct(int productId)
		{
			var pp = (await _productPromotionRepository.GetAllAsync()).Where(x => x.Id_Product == productId);
			var products = await _productRepository.GetAllAsync();
			var promotions = await _promotionRepository.GetAllAsync();
			var objlist = (from a in pp
					   join b in products on a.Id_Product equals b.Id
					   join c in promotions on a.Id_Promotion equals c.Id
					   select new ProductPromotionViewModel()
					   {
						   Id = a.Id,
						   Id_Promotion = a.Id_Promotion,
						   Id_Product = a.Id_Product,
						   Index = a.Index,
						   CreatedDate = a.CreatedDate,
						   Status = a.Status,
						   NameProduct = b.Name,
						   NamePromotion = c.Name,
					   }).ToList();
			return objlist;
		}

		public async Task<List<ProductPromotionViewModel>> GetByPromotion(int promorionId)
		{
			var pp = (await _productPromotionRepository.GetAllAsync()).Where(x => x.Id_Promotion == promorionId);
			var products = await _productRepository.GetAllAsync();
			var promotions = await _promotionRepository.GetAllAsync();
			var objlist = (from a in pp
					   join b in products on a.Id_Product equals b.Id
					   join c in promotions on a.Id_Promotion equals c.Id
					   select new ProductPromotionViewModel()
					   {
						   Id = a.Id,
						   Id_Promotion = a.Id_Promotion,
						   Id_Product = a.Id_Product,
						   Index = a.Index,
						   CreatedDate = a.CreatedDate,
						   Status = a.Status,
						   NameProduct = b.Name,
						   NamePromotion = c.Name,
					   }).ToList();
			return objlist;
		}

		public async Task<bool> Update(int id, UpdateProductPromotionModel model)
		{
			try
			{
				var obj = await _productPromotionRepository.GetByIdAsync(id);
				obj.Index = model.Index;
				obj.Status = model.Status;
				await _productPromotionRepository.UpdateAsync(id, obj);
				return true;
			}
			catch (Exception ex) { return false; }
		}
	}
}
