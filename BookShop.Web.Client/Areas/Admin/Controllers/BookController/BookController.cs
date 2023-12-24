using BookShop.BLL.ConfigurationModel.AuthorModel;
using BookShop.BLL.ConfigurationModel.BookModel;
using BookShop.BLL.ConfigurationModel.CollectionBookModel;
using BookShop.BLL.ConfigurationModel.GenreModel;
using BookShop.BLL.ConfigurationModel.SupplierModel;
using BookShop.BLL.IService;
using BookShop.BLL.Service;
using BookShop.Web.Client.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace BookShop.Web.Client.Areas.Admin.Controllers.BookController
{
    [Area("Admin")]
    [Route("admin/Book")]
    [Authorize(Roles = "Admin,Staff")]
    public class BookController : Controller
    {
        List<BookViewModel> _listBook;
        BookViewModel _book;

        List<GenreModel> _listGenre;
        List<AuthorModel> _listAuthor;
        List<SupplierViewModel> _listSupplier;
        List<CollectionModel> _listCollections;

        private readonly IBookService _bookService;
        private readonly IAuthorService _authorService;
        private readonly IGenreService _genreService;
        private readonly ISupplierService _supplierService;
        private readonly ICollectionService _collectionService;


        public BookController(IBookService bookService, IAuthorService authorService, IGenreService genreService, ISupplierService supplierService, ICollectionService collectionService)
        {
            _listBook = new List<BookViewModel>();
            _book = new BookViewModel();
            _listSupplier = new List<SupplierViewModel>();
            _listAuthor = new List<AuthorModel>();
            _listGenre = new List<GenreModel>();
            _listCollections = new List<CollectionModel>();
            _bookService = bookService;
            _authorService = authorService;
            _genreService = genreService;
            _supplierService = supplierService;
            _collectionService = collectionService;
        }

        public async Task<List<AuthorModel>> LoadAuthor(int status)
        {
            var list = await _authorService.Getall();
            if (status == 1)
            {
                return list.Where(p => p.Status == 1).ToList();
            }
            else
            {
                return list;
            }
        }
        public async Task<List<GenreModel>> LoadGenre(int status)
        {
            var list = await _genreService.GetAll();
            if (status == 1)
            {
                return list.Where(p => p.Status == 1).ToList();
            }
            else
            {
                return list;
            }
        }
        public async Task<List<SupplierViewModel>> LoadSupplier(int status)
        {
            var list = await _supplierService.GetAll();
            if (status == 1)
            {
                return list.Where(p => p.Status == 1).ToList();

            }
            else
            {
                return list;
            }
        }
        // GET: BookController
        [HttpGet("listbook")]
        public async Task<IActionResult> Index([FromQuery(Name = "p")] int currentPages)
        {
            _listBook.Clear();
            _listBook = await _bookService.GetAll();
         
            int pagesize = 10;
            if (pagesize <= 0)
            {
                pagesize = 10;
            }
            int countPages = (int)Math.Ceiling((double)_listBook.Count() / pagesize);
            if (currentPages > countPages)
            {
                currentPages = countPages;
            }
            if (currentPages < 1)
            {
                currentPages = 1;
            }

            var pagingmodel = new PagingModel()
            {
                currentpage = currentPages,
                countpages = countPages,
                generateUrl = (int? p) => Url.Action("Index", "Book", new { areas = "Admin", p = p, pagesize = pagesize })
            };
            ViewBag.pagingmodel = pagingmodel;
            _listBook = _listBook.Skip((pagingmodel.currentpage - 1) * pagesize).Take(pagesize).ToList();
            return View(_listBook);
        }

        // GET: BookController/Details/5
        [HttpGet("Details")]
        public async Task<IActionResult> Details(int id)
        {
            _book = await _bookService.GetById(id);
            return View(_book);
        }

        [HttpGet("Create")]
        // GET: BookController/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Authors = await LoadAuthor(1);
            ViewBag.Genres = await LoadGenre(1);
            ViewBag.Supplier = await LoadSupplier(1);
            return View();
        }

        // POST: BookController/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBookModel book, IFormFile barcodeImg)
        {
            try
            {
                var createBookModel = new CreateBookModel()
                {
                    ISBN = book.ISBN,
                    Title = book.Title,
                    Description = book.Description,
                    Reader = book.Reader,
                    CoverPrice = book.CoverPrice,
                    ImportPrice = book.ImportPrice,
                    Quantity = book.Quantity,
                    Barcode = book.Barcode,
                    PageSize = book.PageSize,
                    Pages = book.Pages,
                    Cover = book.Cover,
                    PublicationDate = book.PublicationDate,
                    Weight = book.Weight,
                    Widght = book.Widght,
                    Length = book.Length,
                    Height = book.Height,
                    CreatedDate = book.CreatedDate,
                    Status = book.Status,
                    Id_Supplier = book.Id_Supplier,
                    authorSelected = book.authorSelected,
                    genreSelected = book.genreSelected
                };
                var result = await _bookService.Add(createBookModel);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BookController/Edit/5
        [HttpGet("Edit")]
        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.Authors = await LoadAuthor(1);
            ViewBag.Genres = await LoadGenre(1);
            ViewBag.Supplier = await LoadSupplier(1);
            _book = await _bookService.GetById(id);
            var updateBookModel = new UpdateBookModel()
            {
                Id = _book.Id,
                ISBN = _book.ISBN,
                Title = _book.Title ?? string.Empty,
                Description = _book.Description,
                Reader = _book.Reader ?? string.Empty,
                Price = Convert.ToInt32(_book.Price),
                ImportPrice = _book.ImportPrice ?? 0,
                Quantity = _book.Quantity ?? 0,
                PageSize = _book.PageSize ?? string.Empty,
                Pages = _book.Pages ?? 0,
                Barcode = _book.Barcode ?? string.Empty,
                Cover = _book.Cover ?? string.Empty,
                PublicationDate = _book.PublicationDate ?? string.Empty,
                Weight = _book.Weight,
                Widght = _book.Widght,
                Length = _book.Length,
                Height = _book.Height,
                CreatedDate = _book.CreatedDate.HasValue ? _book.CreatedDate.Value : default(DateTime),
                Status = _book.Status ?? 0,
                Id_Supplier = _book.Id_Supplier ?? 0,
                authorModels = _book.authorModels,
                genreModels = _book.genreModels               
            };
            updateBookModel.authorSelected = new List<int>();
            updateBookModel.genreSelected = new List<int>();
            foreach(var item in _book.authorModels)
            {
                updateBookModel.authorSelected.Add(item.Id);
            }
            foreach(var item in _book.genreModels)
            {
                updateBookModel.genreSelected.Add(item.Id);
            }
            return View(updateBookModel);
        }

        // POST: BookController/Edit/5
        [HttpPost("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateBookModel book)
        {
            try
            {
                book.Status = book.Quantity == 0 ? 0 : 1;
                var result = await _bookService.Update(book);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BookController/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _bookService.GetById(id);
            if (book == null)
            {
                return NotFound();
            }
            try
            {
                await _bookService.Delete(id);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
        }
    }
}
