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

namespace BookShop.BLL.Service
{
	public class GenreService : IGenreService
	{
		protected readonly IRepository<Genre> _repository;
		protected readonly IRepository<Category> _categoryrepository;
        public GenreService()
        {
			_categoryrepository = new Repository<Category>();
            _repository = new Repository<Genre>();
        }
        public async Task<bool> Add(CreateGenreModel requet)
		{
			try
			{
				var obj = new Genre()
				{
					Name = requet.Name,
					CreatedDate = DateTime.Now,
					Id_Category = requet.Id_Category,
					Status = requet.Status,
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

		public async Task<List<GenreModel>> GetAll()
		{
			var genr = await _repository.GetAllAsync();
			var cate = await _categoryrepository.GetAllAsync();
			var query = from g in genr join
						c in cate on g.Id_Category equals c.Id
						select new GenreModel()
						{
							Id = g.Id,
							Name = g.Name,
							Status= g.Status,
							CreatedDate = g.CreatedDate,
							Id_Category = g.Id_Category,
							Index = g.Index,
							NameCategory = c.Name
						};
			return query.ToList();
		}

		public async Task<GenreModel> GetById(int id)
		{
			var obj = await _repository.GetByIdAsync(id);

			return new GenreModel()
			{
				Id = obj.Id,
				Name = obj.Name,
				Status=obj.Status,
				CreatedDate = obj.CreatedDate,
				Id_Category = obj.Id_Category,
				Index = obj.Index,
			};
		}

		public async Task<bool> Delete(int id)
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

		public async Task<bool> Update(int id, updateGenreModel requet)
		{
			try
			{
				var obj = await _repository.GetByIdAsync(id);
				if (obj != null)
				{
					obj.Name = requet.Name;
					obj.Id_Category = requet.Id_Category;
					obj.Status = requet.Status;
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

		public async Task<List<GenreModel>> GetByCategory(int categoryId)
		{
			var genres = await _repository.GetAllAsync();
			var query = from g in genres
						select new GenreModel()
						{
							Id = g.Id,
							Name = g.Name,
							Status = g.Status,
							CreatedDate = g.CreatedDate,
							Id_Category = g.Id_Category,
							Index = g.Index,
						};
			return query.ToList();
		}
	}
}
