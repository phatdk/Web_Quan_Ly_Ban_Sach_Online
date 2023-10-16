using BookShop.BLL.ConfigurationModel.AuthorModel;
using BookShop.BLL.ConfigurationModel.CategoryModel;
using BookShop.BLL.IService;
using BookShop.DAL.Entities;
using BookShop.DAL.Repositopy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.Service.CategoryService
{
	public class CategoryService : ICategoryService
	{
		protected readonly IRepository<Category> _repository;
		public CategoryService()
		{
			_repository = new Repository<Category>();
		}
		public async Task<bool> Add(CreateCategoryModel requet)
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

		public async Task<List<CategoryModel>> GetAll()
		{
			var obj = await _repository.GetAllAsync();
			var query = (from c in obj
						 select new CategoryModel()
						 {
							 Id = c.Id,
							 Name = c.Name,
							 Status = c.Status,
						 }).ToList();
			return query;
		}

		public async Task<CategoryModel> GetById(int id)
		{
			var obj = await _repository.GetByIdAsync(id);

			return new CategoryModel()
			{
				Id = obj.Id,
				Name = obj.Name,
				Status = obj.Status,
			};
		}

		public async Task<bool> Delete(int id)
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

		public async Task<bool> Update(int id, UpdateCategoryModel requet)
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
