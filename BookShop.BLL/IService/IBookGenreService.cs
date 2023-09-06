using BookShop.BLL.ConfigurationModel.BookGenreModel;
using BookShop.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.IService
{
	public interface IBookGenreService
	{
		public Task<List<BookGenre>> GetByBook(int bookId);
		public Task<bool> Add(EditBookGenreModel model);
		public Task<bool> Update(int id, EditBookGenreModel model);
		public Task<bool> Delete(int id);
	}
}
