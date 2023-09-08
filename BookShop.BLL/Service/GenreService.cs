using BookShop.BLL.ConfigurationModel.GenreModel;
using BookShop.BLL.IService;
using BookShop.DAL.Entities;
using BookShop.DAL.Repositopy;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.Service.BookGenreCategoryService
{
    public class GenreService : IGenreService
	{
		protected readonly IRepository<Genre> _repository;
		protected readonly IRepository<Category> _categoryRepository;
        public GenreService()
        {
            _repository = new Repository<Genre>();
			_categoryRepository = new Repository<Category>();
        }
        public async Task<bool> add(CreateGenreModel requet)
		{
			try
			{
				var obj = new Genre()
				{
					Name = requet.Name,
					CreatedDate = DateTime.Now,
					Id_Category = requet.Id_Category,
					Index = requet.Index,
				};
				await _repository.CreateAsync(obj);
				return true;
			}
			catch (Exception)
			{

				return false;
			}
		}

		public async Task<List<GenreModel>> Getall()
		{
			var obj = await _repository.GetAllAsync();
			var objcate = await	_categoryRepository.GetAllAsync();
			var query = from g in obj
						join c in objcate on g.Id_Category equals c.Id
						select new GenreModel()
						{
							Id = g.Id,
							Name = g.Name,
							CreatedDate = g.CreatedDate,
							Id_Category = g.Id_Category,
							Index = g.Index,
						};
			return query.ToList();
		}

		public async Task<GenreModel> GetbyId(int id)
		{
			var obj = await _repository.GetByIdAsync(id);

			return new GenreModel()
			{
				Id = obj.Id,
				Name = obj.Name,
				CreatedDate = obj.CreatedDate,
				Id_Category = obj.Id_Category,
				Index = obj.Index,
			};
		}

		public async Task<bool> remove(int id)
		{
			try
			{
				var obj = _repository.GetByIdAsync(id);
				if (obj != null)
				{
				 await _repository.RemoveAsync(id);
					return true;
				}
				return false;
			}
			catch (Exception)
			{

				return false;
			}
		}

		public async Task<bool> update(int id, updateGenreModel requet)
		{
			try
			{
				var obj = await _repository.GetByIdAsync(id);
				if (obj != null)
				{
					obj.Name = requet.Name;
					obj.Id_Category = requet.Id_Category;
					obj.Index = requet.Index;
					await	_repository.UpdateAsync(id, obj);
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
