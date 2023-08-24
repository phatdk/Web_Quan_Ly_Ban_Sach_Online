using BLL.ConfigurationModel.AuthorModel;
using BLL.ConfigurationModel.UserModel;
using BookShop.BLL.ConfigurationModel.BookAuthorModel;
using BookShop.BLL.ConfigurationModel.BookModel;
using BookShop.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.IService.IBookService
{
	public interface IBookService
	{
		public Task<List<Book>> Getall();
		public Task<Book> GetbyId(int id);
		public Task<bool> Update(int id, UpdateBookModel requet);
		public Task<bool> Delete(int id);
		public Task<bool> Add(CreateBookModel requet);

		public Task<List<Book>> GetBooksAsync(int AuthorId);
		public Task<List<Author>> GetAuthorAsync(int BookId);

		
	}
}
