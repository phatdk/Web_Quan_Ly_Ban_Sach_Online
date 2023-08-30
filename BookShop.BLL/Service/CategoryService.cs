using BookShop.BLL.ConfigurationModel.AuthorModel;
using BookShop.BLL.ConfigurationModel.BookGenreCategoryModel;
using BookShop.BLL.IService;
using BookShop.DAL.Entities;
using BookShop.DAL.Repositopy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.Service.BookGenreCategoryService
{
    public class CategoryService : ICategoryService
	{
		protected readonly IRepository<Category> _repository;
		public CategoryService()
		{
			_repository = new Repository<Category>();
		}
		public async Task<bool> add(CreateCategoryModel requet)
		{
			try
			{
				var obj = new Category()
				{
					CreatedDate = DateTime.Now,
					Status = 1,
					Name = requet.Name,
				};
				await _repository.CreateAsync(obj);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public async Task<List<CategoryModel>> Getall()
		{
			var obj = await _repository.GetAllAsync();
			var query = from c in obj
						select new CategoryModel()
						{
							Id = c.Id,
							Name = c.Name,
							Status = c.Status,
						};
			return query.ToList();
		}

		public async Task<CategoryModel> GetbyId(int id)
		{
			var obj = await _repository.GetByIdAsync(id);
			
					return new CategoryModel()
						{
							Id = obj.Id,
							Name = obj.Name,
							Status = obj.Status,
						};
			
		}

		public async Task<bool> remove(int id)
		{
			try
			{
				if (id != null)
				{
					var obj = await _repository.RemoveAsync(id);

					return true;
				}
				return false;
			}
			catch (Exception)
			{

				return false;
			}
		}

		public async Task<bool> update(int id, UpdateCategoryModel requet)
		{
			try
			{
				var obj = await _repository.GetByIdAsync(id);
				if (obj != null)
				{

					obj.Name = requet.Name;
					obj.Status = requet.Status;
					await _repository.UpdateAsync(id, obj);
					return true;
				}
				return false;
			}
			catch (Exception)
			{

				return false;
			}
		}
	}
}
