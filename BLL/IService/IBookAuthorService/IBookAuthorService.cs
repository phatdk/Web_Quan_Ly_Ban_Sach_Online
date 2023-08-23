using BookShop.BLL.ConfigurationModel.BookAuthorModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.IService.IBookAuthorService
{
	public interface IBookAuthorService
	{
		public Task<List<BookAuthorModel>> GetAllBooksAuthor();
	}
}
