using BookShop.BLL.ConfigurationModel.GenreModel;
using BookShop.BLL.ConfigurationModel.CollectionBookModel;
using BookShop.BLL.IService;
using BookShop.DAL.Entities;
using BookShop.DAL.Repositopy;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.Service
{
	public class CollectionService : ICollectionService
	{
		protected readonly IRepository<CollectionBook> _collectionRepository;
		public CollectionService()
		{
			_collectionRepository = new Repository<CollectionBook>();
		}
		public async Task<bool> Add(CreateCollectionModel requet)
		{
			try
			{
				var obj = new CollectionBook()
				{
					Name = requet.Name,
					Status = 1,
					CreatedDate = DateTime.Now,

				};
				await _collectionRepository.CreateAsync(obj);
				return true;
			}
			catch (Exception)
			{

				return false;
			}
		}

		public async Task<List<CollectionModel>> GetAll()
		{
			var obj = await _collectionRepository.GetAllAsync();
			var query = from c in obj
						select new CollectionModel()
						{
							Id = c.Id,
							Name = c.Name,
							Status = c.Status,
							CreatedDate = c.CreatedDate,
						};
			return query.ToList();
		}

		public async Task<CollectionModel> GetById(int id)
		{
			var obj = await _collectionRepository.GetByIdAsync(id);

			return new CollectionModel()
			{
				Id = obj.Id,
				Name = obj.Name,
				Status = obj.Status,
				CreatedDate = obj.CreatedDate,
			};

		}

		public async Task<bool> Delete(int id)
		{
			try
			{
				var obj = await _collectionRepository.GetByIdAsync(id);
				if (obj != null)
				{
					await _collectionRepository.RemoveAsync(id);
					return true;
				}
				return false;
			}
			catch (Exception)
			{

				return false;
			}
		}

		public async Task<bool> Update(int id, UpdateCollectionModel requet)
		{
			try
			{
				var obj = await _collectionRepository.GetByIdAsync(id);
				if (obj != null)
				{
					obj.Status = requet.Status;
					obj.Name = requet.Name;
						await _collectionRepository.UpdateAsync(id, obj);
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
