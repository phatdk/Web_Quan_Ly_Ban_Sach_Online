using BookShop.BLL.ConfigurationModel.BookAuthorModel;
using BookShop.BLL.IService;
using BookShop.DAL.Entities;
using BookShop.DAL.Repositopy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.Service
{
	public class BookAuthorService : IBookAuthorService
    {
        protected readonly IRepository<BookAuthor> _repository;
        public BookAuthorService()
        {
            _repository = new Repository<BookAuthor>();
        }

		public async Task<bool> Add(EditBookAuthorModel model)
		{
			try
			{
				var obj = new BookAuthor()
				{
					Id_Author = model.Id_Author,
					Id_Book = model.Id_Book,
				};
				await _repository.CreateAsync(obj);
				return true;
			}catch (Exception ex) { return false; }
		}

		public async Task<bool> Delete(int id)
		{
			try
			{
				await _repository.RemoveAsync(id);
				return true;
			}catch(Exception ex) { return false; }
		}

		public async Task<List<BookAuthor>> GetByBook(int bookId)
		{
			return (await _repository.GetAllAsync()).Where(c=>c.Id_Book == bookId).ToList();
		}

		public async Task<bool> Update(int id, EditBookAuthorModel model)
		{
			try
			{
				var obj = await _repository.GetByIdAsync(id);
				obj.Id_Author = model.Id_Author;
				await _repository.UpdateAsync(id, obj);
				return true;	
			}catch (Exception ex) { return false; }
		}
	}
}
