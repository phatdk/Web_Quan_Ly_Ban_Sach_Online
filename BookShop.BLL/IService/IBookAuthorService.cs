using BookShop.BLL.ConfigurationModel.BookAuthorModel;
using BookShop.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.IService
{
	public interface IBookAuthorService
	{
		public Task<List<BookAuthor>> GetByBook(int bookId);
		public Task<bool> Add(EditBookAuthorModel model);
		public Task<bool> Update(int id, EditBookAuthorModel model);
		public Task<bool> Delete(int id);
	}
}
