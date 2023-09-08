using BookShop.BLL.ConfigurationModel.BookModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.IService
{
	public interface IBookService
	{
		public Task<List<BookViewModel>> GetAll();
		public Task<BookViewModel> GetById(int id);
		public Task<bool> Update(int id, UpdateBookModel requet);
		public Task<bool> Delete(int id);
		public Task<bool> Add(CreateBookModel requet);
	}
}
