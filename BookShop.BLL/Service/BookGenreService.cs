using BookShop.BLL.ConfigurationModel.BookAuthorModel;
using BookShop.BLL.ConfigurationModel.BookGenreModel;
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
	public class BookGenreService : IBookGenreService
	{
		private readonly IRepository<BookGenre> _repository;
        public BookGenreService()
        {
            _repository = new Repository<BookGenre>();
        }

		public async Task<bool> Add(EditBookGenreModel model)
		{
			try
			{
				var obj = new BookGenre()
				{
					Id_Genre = model.Id_Genre,
					Id_Book = model.Id_Book,
				};
				await _repository.CreateAsync(obj);
				return true;
			}
			catch (Exception ex) { return false; }
		}

		public async Task<bool> Delete(int id)
		{
			try
			{
				await _repository.RemoveAsync(id);
				return true;
			}
			catch (Exception ex) { return false; }
		}

		public async Task<List<BookGenre>> GetByBook(int bookId)
		{
			return (await _repository.GetAllAsync()).Where(c => c.Id_Book == bookId).ToList();
		}

		public async Task<bool> Update(int id, EditBookGenreModel model)
		{
			try
			{
				var obj = await _repository.GetByIdAsync(id);
				obj.Id_Genre = model.Id_Genre;
				await _repository.UpdateAsync(id, obj);
				return true;
			}
			catch (Exception ex) { return false; }
		}
	}
}
