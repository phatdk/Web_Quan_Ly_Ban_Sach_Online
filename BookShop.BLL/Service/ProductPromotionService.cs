using BookShop.BLL.ConfigurationModel.ProductPromotionModel;
using BookShop.BLL.IService;
using BookShop.DAL.Entities;
using BookShop.DAL.Repositopy;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.Service
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

		public async Task<bool> AddPromotions(int productId, List<int> promotionIdList)
		{
			try
			{
				var prodp = (await _productPromotionRepository.GetAllAsync()).Where(x => x.Id_Product == productId);
				foreach (var item in prodp)
				{
					foreach (var subItem in promotionIdList)
					{
						if (item.Id_Promotion == subItem)
						{
							promotionIdList.Remove(subItem);
							goto skip;
						}
					}
					await _productPromotionRepository.RemoveAsync(item.Id);
				skip:;
				}
				foreach (var promotionId in promotionIdList)
				{
					await _productPromotionRepository.CreateAsync(new ProductPromotion
					{
						Id_Product = productId,
						Id_Promotion = promotionId,
						Index = 0,
						CreatedDate = DateTime.Now,
						Status = 1,
					});
				}
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public async Task<bool> AddProducts(int promotionId, List<int> productIdList)
		{
			try
			{
				var promop = (await _productPromotionRepository.GetAllAsync()).Where(x => x.Id_Promotion == promotionId);
				foreach (var item in promop)
				{
					foreach (var subItem in productIdList)
					{
						if (item.Id_Promotion == subItem)
						{
							productIdList.Remove(subItem);
							goto skip;
						}
					}
					await _productPromotionRepository.RemoveAsync(item.Id);
				skip:;
				}
				foreach (var productId in productIdList)
				{
					await _productPromotionRepository.CreateAsync(new ProductPromotion
					{
						Id_Product = productId,
						Id_Promotion = promotionId,
						Index = 0,
						CreatedDate = DateTime.Now,
						Status = 1,
					});
				}
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
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
							   ProductPrice = b.Price,
							   NamePromotion = c.Name,
							   AmountReduct = c.AmountReduct,
							   PercentReduct = c.PercentReduct,
							   ReductMax = c.ReductMax,
						   }).ToList();
			foreach (var item in objlist)
			{
				if (item.PercentReduct != null)
				{
					var amount = Convert.ToInt32(Math.Floor(Convert.ToDouble((item.ProductPrice / 100) * item.PercentReduct)));
					if (amount > item.ReductMax) amount = item.ReductMax;
					item.TotalReduct = amount;
				}
				else item.TotalReduct = Convert.ToInt32(item.AmountReduct);
			}
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
							   ProductPrice = b.Price,
							   NamePromotion = c.Name,
							   AmountReduct = c.AmountReduct,
							   PercentReduct = c.PercentReduct,
							   ReductMax = c.ReductMax,
						   }).ToList();
			foreach (var item in objlist)
			{
				if (item.PercentReduct != null)
				{
					var amount = Convert.ToInt32(Math.Floor(Convert.ToDouble((item.ProductPrice / 100) * item.PercentReduct)));
					if (amount > item.ReductMax) amount = item.ReductMax;
					item.TotalReduct = amount;
				}
				else item.TotalReduct = Convert.ToInt32(item.AmountReduct);
			}
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
