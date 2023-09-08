using BookShop.BLL.IService;
using BookShop.BLL.ConfigurationModel.BookModel;
using BookShop.DAL.Entities;
using BookShop.DAL.Repositopy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.Service
{
	public class BookService : IBookService
	{
		protected readonly IRepository<Book> _bookRepository;
		protected readonly IRepository<CollectionBook> _collectionRepository;
		protected readonly IRepository<Supplier> _supplierRepository;
		public BookService()
		{
			_bookRepository = new Repository<Book>();
			_collectionRepository = new Repository<CollectionBook>();
			_supplierRepository = new Repository<Supplier>();
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
					CreatedDate = DateTime.Now,
					Status = 1,
					Id_Collection = requet.Id_Collection,
					Id_Supplier = requet.Id_Supplier,
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
			try
			{
				await _bookRepository.RemoveAsync(id);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public async Task<bool> Update(int id, UpdateBookModel requet)
		{
			try
			{
				await _bookRepository.RemoveAsync(id);
				return true;
			}
			catch (Exception)
			{

				return false;
			}
		}

		public async Task<List<BookViewModel>> GetAll()
		{
			var books = await _bookRepository.GetAllAsync();
			var suppliers = await _supplierRepository.GetAllAsync();
			var collections = await _collectionRepository.GetAllAsync();
			var objlist = (from a in books
						   join b in suppliers on a.Id_Supplier equals b.Id
						   join c in collections on a.Id_Collection equals c.Id into t
						   from c in t.DefaultIfEmpty()
						   select new BookViewModel()
						   {
							   Id = a.Id,
							   Title = a.Title,
							   Reader = a.Reader,
							   Price = a.Price,
							   ImportPrice = a.ImportPrice,
							   Quantity = a.Quantity,
							   PageSize = a.PageSize,
							   Pages = a.Pages,
							   Cover = a.Cover,
							   PublicationDate = a.PublicationDate,
							   Description = a.Description,
							   Weight = a.Weight,
							   Widght = a.Widght,
							   Length = a.Length,
							   Height = a.Height,
							   CreatedDate = a.CreatedDate,
							   Status = a.Status,
							   Id_Collection = a.Id_Collection,
							   Id_Supplier = a.Id_Supplier,
							   CollectionName = c.Name != null ? c.Name : "No collection",
							   SupplierName = b.Name,
						   }).ToList();
			return objlist;
		}

		public async Task<BookViewModel> GetById(int id)
		{
			var books = (await _bookRepository.GetAllAsync()).Where(c=>c.Id == id);
			var suppliers = await _supplierRepository.GetAllAsync();
			var collections = await _collectionRepository.GetAllAsync();
			var objlist = (from a in books
						   join b in suppliers on a.Id_Supplier equals b.Id
						   join c in collections on a.Id_Collection equals c.Id into t
						   from c in t.DefaultIfEmpty()
						   select new BookViewModel()
						   {
							   Id = a.Id,
							   Title = a.Title,
							   Reader = a.Reader,
							   Price = a.Price,
							   ImportPrice = a.ImportPrice,
							   Quantity = a.Quantity,
							   PageSize = a.PageSize,
							   Pages = a.Pages,
							   Cover = a.Cover,
							   PublicationDate = a.PublicationDate,
							   Description = a.Description,
							   Weight = a.Weight,
							   Widght = a.Widght,
							   Length = a.Length,
							   Height = a.Height,
							   CreatedDate = a.CreatedDate,
							   Status = a.Status,
							   Id_Collection = a.Id_Collection,
							   Id_Supplier = a.Id_Supplier,
							   CollectionName = c.Name != null ? c.Name : "No collection",
							   SupplierName = b.Name,
						   }).FirstOrDefault();
			return objlist;
		}
	}
}
