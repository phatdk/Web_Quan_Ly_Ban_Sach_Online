using BookShop.BLL.ConfigurationModel.BookModel;
using BookShop.BLL.IService.IBookAuthorService;
using BookShop.BLL.IService.IBookService;
using BookShop.DAL.Entities;
using BookShop.DAL.Repositopy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.Service.BookService
{
	public class BookService : IBookService
	{
		protected readonly IRepository<Book> _bookRepository;
		protected readonly IRepository<BookAuthor>_bookAuthorService;
		protected readonly IRepository<Author> _authorRepository;
        public BookService()
        {
			
			_authorRepository = new Repository<Author>();
			_bookRepository = new Repository<Book>();
			_bookAuthorService = new Repository<BookAuthor>();
        }
		public async Task<bool> Add(CreateBookModel requet)
		{
			try
			{
				var obj = new Book()
				{
					ISBN = requet.ISBN,
					Title = requet.Title,
					Description = requet.Description,
					Reader = requet.Reader,
					Price = requet.Price,
					ImportPrice = requet.ImportPrice,
					Quantity = requet.Quantity,
					PageSize = requet.PageSize,
					Pages = requet.Pages,
					Cover = requet.Cover,
					PublicationDate = requet.PublicationDate,
					Weight = requet.Weight,
					Height = requet.Height,
					Length = requet.Length,
					Widght = requet.Widght,
					CreatedDate = DateTime.UtcNow,
					Status = 1,
					Id_Collection = requet.Id_Collection,
					Id_Supplier = requet.Id_Supplier,
					Id_Language = requet.Id_Language,
				};
			await _bookRepository.CreateAsync(obj);
				return true;
			}
			catch (Exception)
			{

				return false;
			}
		}

		public async Task<bool> Delete(int id)
		{
			if (id != null)
			{
				await _bookRepository.RemoveAsync(id);
				return true;
			}
			return false;
		}

		public async Task<IEnumerable<Book>> Getall()
		{
			return await _bookRepository.GetAllAsync();
		}

		public async Task<List<Author>> GetAuthorAsync(int BookId)
		{
			var book = await _bookRepository.GetAllAsync();
			var author = await _authorRepository.GetAllAsync();
			var bookauthor = await _bookAuthorService.GetAllAsync();
			var query = from pa in bookauthor join 
							b in book on pa.Id_Book equals b.Id
							join a in author on pa.Id_Author equals a.Id
							where b.Id == BookId
							select a;
						return  query.ToList();
		}

		public async Task<List<Book>> GetBooksAsync(int AuthorId)
		{
			var book = await _bookRepository.GetAllAsync();
			var author = await _authorRepository.GetAllAsync();
			var bookauthor = await _bookAuthorService.GetAllAsync();
			var query = from pa in bookauthor
						join
							b in book on pa.Id_Book equals b.Id
						join a in author on pa.Id_Author equals a.Id
						where a.Id == AuthorId
						select b;
			return query.ToList();
		}



		public async Task<Book> GetbyId(int id)
		{
			return await _bookRepository.GetByIdAsync(id);
		}

		public async Task<bool> Update(int id, UpdateBookModel requet)
		{
			try
			{
				if (id!= null)
				{
				await _bookRepository.RemoveAsync(id);
					return true;
				}
				return false;
			}
			catch (Exception)
			{

				return false;
			}
		}

		Task<List<Book>> IBookService.Getall()
		{
			throw new NotImplementedException();
		}
	}
}
