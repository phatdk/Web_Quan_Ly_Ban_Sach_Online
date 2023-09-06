using BookShop.BLL.ConfigurationModel.ProductModel;
using BookShop.BLL.IService;
using BookShop.DAL.Entities;
using BookShop.DAL.Repositopy;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.Service
{
	public class ProductService : IProductService
	{
		private readonly IRepository<Product> _reponsitory;
        public ProductService()
        {
            _reponsitory = new Repository<Product>();
        }
        public async Task<bool> Add(CreateProductModel model)
		{
			try
			{
				var obj = new Product()
				{
					Name = model.Name,
					Price = model.Price,
					Quantity = model.Quantity,
					Description = model.Description,
					CreatedDate = DateTime.Now,
					Status = model.Status,
					Type = model.Type,
				};
				await _reponsitory.CreateAsync(obj);
				return true;
			}catch (Exception ex) { return false; }
		}

		public async Task<bool> Delete(int id)
		{
			try
			{
				await _reponsitory.RemoveAsync(id);
				return true;
			}catch (Exception ex) { return false; }	
		}

		public async Task<List<ProductViewModel>> GetAll()
		{
			var list = await _reponsitory.GetAllAsync();
			var objlist = new List<ProductViewModel>();
			foreach(var item in list)
			{
				var obj = new ProductViewModel()
				{
					Id = item.Id,
					Name = item.Name,
					Price = item.Price,
					Quantity = item.Quantity,
					Description = item.Description,
					CreatedDate = item.CreatedDate,
					Status = item.Status,
					Type = item.Type,
				};
				objlist.Add(obj);
			}
			return objlist;
		}

		public async Task<ProductViewModel> GetById(int id)
		{
			var obj = await _reponsitory.GetByIdAsync(id);
			return new ProductViewModel()
			{
				Id = obj.Id,
				Name = obj.Name,
				Price = obj.Price,
				Quantity = obj.Quantity,
				Description = obj.Description,
				CreatedDate = obj.CreatedDate,
				Status = obj.Status,
				Type = obj.Type,
			};
		}

		public async Task<bool> Update(int id, UpdateProductModel model)
		{
			try
			{
				var obj = await _reponsitory.GetByIdAsync(id);
				obj.Name = model.Name;
				obj.Price = model.Price;
				obj.Quantity = model.Quantity;
				obj.Description = model.Description;
				obj.Status = model.Status;
				await _reponsitory.UpdateAsync(id, obj);
				return true;
			}catch (Exception ex) { return false; }
		}
	}
}
