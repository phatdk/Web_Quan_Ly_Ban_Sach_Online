using BookShop.BLL.ConfigurationModel.AuthorModel;
using BookShop.BLL.ConfigurationModel.BookModel;
using BookShop.BLL.ConfigurationModel.GenreModel;
using BookShop.BLL.IService;
using BookShop.DAL.Entities;
using BookShop.DAL.Repositopy;

namespace BookShop.BLL.Service
{
	public class BookService : IBookService
    {
        protected readonly IRepository<Book> _bookRepository;
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
                    CoverPrice = requet.CoverPrice,
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
                    Barcode = requet.Barcode,
                    CreatedDate = DateTime.Now,
                    Status = requet.Status,
                    Id_Supplier = requet.Id_Supplier,
                };
                var book = await _bookRepository.CreateAsync(obj);
                // var book = (await _bookRepository.GetAllAsync()).MaxBy(x => x.CreatedDate);
                if (book != null)
                {
                    foreach (var item in requet.authorSelected)
                    {
                        var author = new BookAuthor()
                        {
                            Id_Book = book.Id,
                            Id_Author = item,
                        };
                        await _bookAuthorRepository.CreateAsync(author);
                    }
                    foreach (var item in requet.genreSelected)
                    {
                        var genre = new BookGenre()
                        {
                            Id_Book = book.Id,
                            Id_Genre = item,
                        };
                        await _bookGenreRepository.CreateAsync(genre);
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
                obj.Barcode = requet.Barcode;
                obj.Description = requet.Description;
                obj.Reader = requet.Reader;
                obj.Barcode = requet.Barcode;
                obj.CoverPrice = requet.Price;
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

                var bookauthors = (await _bookAuthorRepository.GetAllAsync()).Where(x => x.Id_Book == obj.Id);
                // Loại bỏ phần tử không có và skip các phần tử đã có
                foreach (var item in bookauthors)
                {
                    for (int i = 0; i < requet.authorSelected.Count(); i++)
                    {
                        if (requet.authorSelected[i] == item.Id_Author)
                        {
                            requet.authorSelected.RemoveAt(i);
                            goto skip1;
                        }
                    }
                    await _bookAuthorRepository.RemoveAsync(item.Id);
                skip1:;
                }
                // tạo phần tử mới
                foreach (var item in requet.authorSelected)
                {
                    var pbnew = new BookAuthor()
                    {
                        Id_Book = requet.Id,
                        Id_Author = item,
                    };
                    await _bookAuthorRepository.CreateAsync(pbnew);
                }

                var bookgenres = (await _bookGenreRepository.GetAllAsync()).Where(x => x.Id_Book == obj.Id);
                // Loại bỏ phần tử không có và skip các phần tử đã có
                foreach (var item in bookgenres)
                {
                    for (int i = 0; i < requet.genreSelected.Count(); i++)
                    {
                        if (requet.genreSelected[i] == item.Id_Genre)
                        {
                            requet.genreSelected.RemoveAt(i);
                            goto skip2;
                        }
                    }
                    await _bookGenreRepository.RemoveAsync(item.Id);
                skip2:;
                }
                // tạo phần tử mới
                foreach (var item in requet.genreSelected)
                {
                    var pbnew = new BookGenre()
                    {
                        Id_Book = requet.Id,  // lưu đc
                        Id_Genre = item,

                        //Id_Genre = requet.Id, // k lưu đc
                        //Id_Book = item,
                    };
                    await _bookGenreRepository.CreateAsync(pbnew);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<BookViewModel>> GetAll()
        {
            var books = await _bookRepository.GetAllAsync();
            var suppliers = await _supplierRepository.GetAllAsync();
            var authors = await _authorRepository.GetAllAsync();
            var genres = await _genreRepository.GetAllAsync();
            var objlist = new List<BookViewModel>();
            foreach (var item in books)
            {
                var supplier = (await _supplierRepository.GetAllAsync()).Where(p => p.Id == item.Id_Supplier).FirstOrDefault();

                var authorIds = (await _bookAuthorRepository.GetAllAsync())
             .Where(ba => ba.Id_Book == item.Id)
             .Select(ba => ba.Id_Author)
             .ToList();

                var bookAuthors = (await _authorRepository.GetAllAsync())
                    .Where(a => authorIds.Contains(a.Id))
                    .Select(a => a.Name) // Lấy tên tác giả
                    .ToList();

                var genreIds = (await _bookGenreRepository.GetAllAsync())
                    .Where(bg => bg.Id_Book == item.Id)
                    .Select(bg => bg.Id_Genre)
                    .ToList();

                var bookGenres = (await _genreRepository.GetAllAsync())
                    .Where(g => genreIds.Contains(g.Id))
                    .Select(g => g.Name) // Lấy tên thể loại
                    .ToList();

                var obj = new BookViewModel()
                {
                    Id = item.Id,
                    ISBN = item.ISBN,
                    Title = item.Title,
                    Price = item.CoverPrice,
                    Quantity = item.Quantity,
                    Description = item.Description,
                    Status = item.Status,
                    ImportPrice = item.ImportPrice,
                    PageSize = item.PageSize,
                    Pages = item.Pages,
                    Reader = item.Reader,
                    Barcode = item.Barcode,
                    Cover = item.Cover,
                    PublicationDate = item.PublicationDate,
                    Weight = item.Weight,
                    Widght = item.Widght,
                    Length = item.Length,
                    Height = item.Height,
                    CreatedDate = item.CreatedDate,
                    Id_Supplier = item.Id_Supplier,
                    SupplierName = supplier.Name,
                    authors = bookAuthors,
                    genres = bookGenres,
                };
                objlist.Add(obj);
            }
            return objlist;
        }

        public async Task<BookViewModel> GetById(int id)
        {
            var books = await _bookRepository.GetByIdAsync(id);
            var ba = (await _bookAuthorRepository.GetAllAsync()).Where(x => x.Id_Book == id);
            var bg = (await _bookGenreRepository.GetAllAsync()).Where(x => x.Id_Book == id);
            var authors = new List<AuthorModel>();
            foreach (var item in ba)
            {
                var author = await _authorRepository.GetByIdAsync(item.Id_Author);
                var authormd = new AuthorModel()
                {
                    Id = author.Id,
                    Name = author.Name,
                    Img = author.Img,
                    Index = author.Index,
                    Status = author.Status,
                    CreatedDate = author.CreatedDate
                };
                authors.Add(authormd);
            }

            var genres = new List<GenreModel>();
            foreach (var item in bg)
            {
                var genre = await _genreRepository.GetByIdAsync(item.Id_Genre);
                var genremd = new GenreModel()
                {
                    Id = genre.Id,
                    Name = genre.Name,
                    Index = genre.Index,
                    Status = genre.Status,
                    CreatedDate = genre.CreatedDate
                };
                genres.Add(genremd);
            }

            return new BookViewModel()
            {
                Id = books.Id,
                ISBN = books.ISBN,
                Title = books.Title,
                Reader = books.Reader,
                Price = books.CoverPrice,
                ImportPrice = books.ImportPrice,
                Quantity = books.Quantity,
                PageSize = books.PageSize,
                Pages = books.Pages,
                Barcode = books.Barcode,
                Cover = books.Cover,
                PublicationDate = books.PublicationDate,
                Description = books.Description,
                Weight = books.Weight,
                Widght = books.Widght,
                Length = books.Length,
                Height = books.Height,
                CreatedDate = books.CreatedDate,
                Status = books.Status,
                Id_Supplier = books.Id_Supplier,
                authorModels = authors,
                genreModels = genres
            };

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
            var objlist = (from a in books
                           join b in suppliers on a.Id_Supplier equals b.Id
                           select new BookViewModel()
                           {
                               Id = a.Id,
                               Title = a.Title,
                               Price = a.CoverPrice,
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
            var objlist = (from a in books
                           join b in suppliers on a.Id_Supplier equals b.Id
                           select new BookViewModel()
                           {
                               Id = a.Id,
                               Title = a.Title,
                               Price = a.CoverPrice,
                               Quantity = a.Quantity,
                               Description = a.Description,
                               Status = a.Status,
                           }).ToList();
            return objlist;
        }

        public Task<bool> ChangeQuantity(int id, int quantity)
        {
            throw new NotImplementedException();
        }

        //public async Task<bool> ChangeQuantity(int id, int quantity)
        //{
        //getAgain:;
        //    var book = await _bookRepository.GetByIdAsync(id);
        //    try
        //    {
        //        if (book != null)
        //        {
        //            book.Quantity += quantity;
        //        }
        //        else goto getAgain;
        //        await _bookRepository.UpdateAsync(id, book);
        //        return true;
        //    }
        //    catch { return false; }
        //}
    }
}
