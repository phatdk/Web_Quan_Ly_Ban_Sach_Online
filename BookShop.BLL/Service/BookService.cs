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
using BookShop.BLL.ConfigurationModel.AuthorModel;
using BookShop.BLL.ConfigurationModel.GenreModel;

namespace BookShop.BLL.Service
{
    public class BookService : IBookService
    {
        protected readonly IRepository<Book> _bookRepository;
        protected readonly IRepository<CollectionBook> _collectionRepository;
        protected readonly IRepository<Supplier> _supplierRepository;
        protected readonly IRepository<Author> _authorRepository;
        protected readonly IRepository<Genre> _genreRepository;
        protected readonly IRepository<BookAuthor> _bookAuthorRepository;
        protected readonly IRepository<BookGenre> _bookGenreRepository;
        protected readonly IRepository<Product> _productRepository;
        protected readonly IRepository<ProductBook> _productBookRepository;
        public BookService()
        {
            _bookRepository = new Repository<Book>();
            _collectionRepository = new Repository<CollectionBook>();
            _supplierRepository = new Repository<Supplier>();
            _authorRepository = new Repository<Author>();
            _genreRepository = new Repository<Genre>();
            _bookAuthorRepository = new Repository<BookAuthor>();
            _bookGenreRepository = new Repository<BookGenre>();
            _productBookRepository = new Repository<ProductBook>();
            _productRepository = new Repository<Product>();
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
                    Status = requet.Status,
                    Id_Supplier = requet.Id_Supplier,
                };
                var book = await _bookRepository.CreateAsync(obj);
                // var book = (await _bookRepository.GetAllAsync()).MaxBy(x => x.CreatedDate);
                if (book != null)
                {
                    foreach (AuthorModel item in requet.authorModels)
                    {
                        var author = new BookAuthor()
                        {
                            Id_Book = book.Id,
                            Id_Author = item.Id,
                        };
                        await _bookAuthorRepository.CreateAsync(author);
                    }
                    foreach (GenreModel item in requet.genreModels)
                    {
                        var genre = new BookGenre()
                        {
                            Id_Book = book.Id,
                            Id_Genre = item.Id,
                        };
                        await _bookGenreRepository.CreateAsync(genre);
                    }
                    var product = new Product()
                    {
                        Name = book.Title,
                        Quantity = book.Quantity,
                        Price = book.Price,
                        Description = book.Description,
                        CreatedDate = DateTime.Now,
                        Status = book.Status,
                        Type = 0,
                    };
                    var productSS = await _productRepository.CreateAsync(product);
                    if (productSS != null)
                    {
                        var pb = new ProductBook()
                        {
                            Id_Book = book.Id,
                            Id_Product = productSS.Id,
                            Status = 1,
                        };
                        await _productBookRepository.CreateAsync(pb);
                    }
                    return true;
                }
                return false;
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

        public async Task<bool> Update(UpdateBookModel requet)
        {
            try
            {
                var obj = await _bookRepository.GetByIdAsync(requet.Id);
                obj.ISBN = requet.ISBN;
                obj.Title = requet.Title;
                obj.Description = requet.Description;
                obj.Reader = requet.Reader;
                obj.Price = requet.Price;
                obj.ImportPrice = requet.ImportPrice;
                obj.Quantity = requet.Quantity;
                obj.PageSize = requet.PageSize;
                obj.Pages = requet.Pages;
                obj.Cover = requet.Cover;
                obj.PublicationDate = requet.PublicationDate;
                obj.Weight = requet.Weight;
                obj.Height = requet.Height;
                obj.Widght = requet.Widght;
                obj.Length = requet.Length;
                obj.Status = requet.Status;
                obj.Id_Supplier = requet.Id_Supplier;

                await _bookRepository.UpdateAsync(obj.Id, obj);
                foreach (AuthorModel item in requet.authorModels)
                {
                    BookAuthor author = await _bookAuthorRepository.GetByIdAsync(item.Id);
                    if (item.Id != author.Id_Author)
                    {
                        await _bookAuthorRepository.UpdateAsync(author.Id, author);
                    }
                }
                foreach (GenreModel item in requet.genreModels)
                {
                    BookGenre genre = await _bookGenreRepository.GetByIdAsync(item.Id);
                    if (item.Id != genre.Id_Genre)
                    {
                        await _bookGenreRepository.UpdateAsync(genre.Id, genre);
                    }
                }
                var product = await _productRepository.GetByIdAsync((await _productBookRepository.GetAllAsync()).Where(x => x.Id_Book == requet.Id).MinBy(x => x.Id).Id_Product);
                if (product.Price != requet.Price || !(product.Name).Equals(requet.Title) || product.Quantity != requet.Quantity || !(product.Description).Equals(requet.Description))
                {
                    product.Name = requet.Title;
                    product.Quantity = requet.Quantity;
                    product.Description = requet.Description;
                    product.Price = requet.Price;
                    await _productRepository.UpdateAsync(product.Id, product);
                }
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
                           select new BookViewModel()
                           {
                               Id = a.Id,
                               Title = a.Title,
                               Price = a.Price,
                               Quantity = a.Quantity,
                               Description = a.Description,
                               Status = a.Status,
                           }).ToList();
            return objlist;
        }

        public async Task<BookViewModel> GetById(int id)
        {
            var books = (await _bookRepository.GetAllAsync()).Where(c => c.Id == id);
            var suppliers = await _supplierRepository.GetAllAsync();
            var collections = await _collectionRepository.GetAllAsync();
            var objlist = (from a in books
                           join b in suppliers on a.Id_Supplier equals b.Id
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
                               Id_Supplier = a.Id_Supplier,
                               SupplierName = b.Name,
                           }).FirstOrDefault();
            var listAuthor = (await _bookAuthorRepository.GetAllAsync()).Where(x => x.Id_Book == id);
            foreach (BookAuthor item in listAuthor)
            {
                Author author = await _authorRepository.GetByIdAsync(item.Id_Author);
                AuthorModel authorModel = new AuthorModel()
                {
                    Id = author.Id,
                    Name = author.Name,
                };
                objlist.authorModels.Add(authorModel);
            }
            var listGenre = (await _bookGenreRepository.GetAllAsync()).Where(x => x.Id_Book == id);
            foreach (BookGenre item in listGenre)
            {
                Genre genre = await _genreRepository.GetByIdAsync(item.Id_Genre);
                GenreModel genreModel = new GenreModel()
                {
                    Id = genre.Id,
                    Name = genre.Name,
                };
                objlist.genreModels.Add(genreModel);
            }
            return objlist;
        }

        public async Task<List<BookViewModel>> GetByAuthor(int authorId)
        {
            var listAuthor = (await _bookAuthorRepository.GetAllAsync()).Where(x => x.Id_Author == authorId);
            var books = new List<Book>();
            foreach (BookAuthor item in listAuthor)
            {
                var book = await _bookRepository.GetByIdAsync(item.Id_Book);
                books.Add(book);
            }

            var suppliers = await _supplierRepository.GetAllAsync();
            var collections = await _collectionRepository.GetAllAsync();
            var objlist = (from a in books
                           join b in suppliers on a.Id_Supplier equals b.Id
                           select new BookViewModel()
                           {
                               Id = a.Id,
                               Title = a.Title,
                               Price = a.Price,
                               Quantity = a.Quantity,
                               Description = a.Description,
                               Status = a.Status,
                           }).ToList();
            return objlist;
        }

        public async Task<List<BookViewModel>> GetByGenre(int genrerId)
        {
            var listGenre = (await _bookGenreRepository.GetAllAsync()).Where(x => x.Id_Genre == genrerId);
            var books = new List<Book>();
            foreach (BookGenre item in listGenre)
            {
                var book = await _bookRepository.GetByIdAsync(item.Id_Book);
                books.Add(book);
            }

            var suppliers = await _supplierRepository.GetAllAsync();
            var collections = await _collectionRepository.GetAllAsync();
            var objlist = (from a in books
                           join b in suppliers on a.Id_Supplier equals b.Id
                           select new BookViewModel()
                           {
                               Id = a.Id,
                               Title = a.Title,
                               Price = a.Price,
                               Quantity = a.Quantity,
                               Description = a.Description,
                               Status = a.Status,
                           }).ToList();
            return objlist;
        }
    }
}
