using BLL.ConfigurationModel.AuthorModel;
using BLL.ConfigurationModel.UserModel;
using BLL.IService.IAuthorService;
using BookShop.DAL.Entities;
using BookShop.DAL.Repositopy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Service
{
	public class AuthorService : IAuthorService
	{
		protected readonly IRepository<Author> _repository;
        public AuthorService(IRepository<Author> repository)
        {
            _repository = repository;
        }
        public async Task<bool> Add(CreateAuthorModel requet)
		{
			try
			{
				var obj = new Author()
				{
					CreatedDate = DateTime.Now,
					Img = requet.Img,
					Index = requet.Index,
					Status = requet.Status,
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
				return true;
			}
		}

		public async Task<List<Author>> Getall()
		{
			var obj = await _repository.GetAllAsync();
			var query = from p in obj
						orderby p.Index ascending
						select p;
			return query.ToList();
		}

		public async Task<Author> GetbyId(int id)
		{
			
				var obj = await _repository.GetByIdAsync(id);
				return obj;
		
		}

		public async Task<bool> Update(int id, UpdateAuthorModel requet)
		{
			try
			{
				var obj = await _repository.GetByIdAsync(id);
				if (obj != null)
				{
					obj.Status = requet.Status;
					obj.Name = requet.Name;
					obj.Img = requet.Img;
					obj.Index = requet.Index;
					await _repository.UpdateAsync(id,obj);
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
