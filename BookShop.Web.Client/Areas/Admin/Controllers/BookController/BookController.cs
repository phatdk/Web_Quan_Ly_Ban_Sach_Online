using BookShop.BLL.ConfigurationModel.AuthorModel;
using BookShop.BLL.ConfigurationModel.BookModel;
using BookShop.BLL.ConfigurationModel.CollectionBookModel;
using BookShop.BLL.ConfigurationModel.GenreModel;
using BookShop.BLL.ConfigurationModel.SupplierModel;
using BookShop.BLL.IService;
using BookShop.DAL.Entities;
using BookShop.Web.Client.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text;

namespace BookShop.Web.Client.Areas.Admin.Controllers.BookController
{
	[Area("Admin")]

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
		[HttpGet]
		public async Task<IActionResult> Index()
		{
			return View();
		}

		public async Task<IActionResult> Getdata(int page, int? status, string? keyWord)
		{
			_listBook = await _bookService.GetAll();
			if (keyWord != null)
			{
				_listBook = _listBook.Where(c => c.Title.Contains(keyWord)).ToList();
			}

			var listBook = _listBook.OrderByDescending(c => c.CreatedDate).ToList();
			int pageSize = 10;
			double totalPage = (double)listBook.Count / pageSize;
			listBook = listBook.Skip((page - 1) * pageSize).Take(pageSize).ToList();
			return Json(new { data = listBook, page = page, max = Math.Ceiling(totalPage) });
		}




		// GET: BookController/Details/5
		[HttpGet("Book/Details")]
		public async Task<IActionResult> Details(int id)
		{
			_book = await _bookService.GetById(id);
			return View(_book);
		}

		[HttpGet("Book/Create")]
		// GET: BookController/Create
		public async Task<IActionResult> Create()
		{
			ViewBag.Authors = await LoadAuthor(1);
			ViewBag.Genres = await LoadGenre(1);
			ViewBag.Supplier = await LoadSupplier(1);
			return View();
		}

		// POST: BookController/Create
		[HttpPost("Book/Create")]

		public async Task<IActionResult> Create(CreateBookModel book, IFormFile imageFile)
		{
			try
			{
				var convert = ConvertToValidString(book.Title);
				if (imageFile != null && imageFile.Length > 0)
				{
					var extension = Path.GetExtension(imageFile.FileName);
					var filename = "Book_" + convert + extension;
					var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "book", filename);

					var stream = new FileStream(path, FileMode.Create);
					imageFile.CopyTo(stream);
					book.Img = filename;

				}
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
					genreSelected = book.genreSelected,
					Img = book.Img,

				};
				var result = await _bookService.Add(createBookModel);
				if (result)
				{
					return RedirectToAction(nameof(Index));
				}
				return View();
			}
			catch
			{
				return View();
			}
		}
		static string ConvertToValidString(string input)
		{
			// Loại bỏ dấu tiếng Việt
			string removedDiacritics = RemoveDiacritics(input);

			// Chuyển thành chữ thường và loại bỏ khoảng trắng
			string result = removedDiacritics.ToLower().Replace(" ", "");

			return result;
		}

		static string RemoveDiacritics(string text)
		{
			string normalizedString = text.Normalize(NormalizationForm.FormD);
			StringBuilder stringBuilder = new StringBuilder();

			foreach (char c in normalizedString)
			{
				UnicodeCategory category = CharUnicodeInfo.GetUnicodeCategory(c);
				if (category != UnicodeCategory.NonSpacingMark)
				{
					stringBuilder.Append(c);
				}
			}

			return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
		}

		// GET: BookController/Edit/5
		[HttpGet("Book/Edit")]
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
				genreModels = _book.genreModels,
				Img = _book.Img,
			};
			updateBookModel.authorSelected = new List<int>();
			updateBookModel.genreSelected = new List<int>();
			foreach (var item in _book.authorModels)
			{
				updateBookModel.authorSelected.Add(item.Id);
			}
			foreach (var item in _book.genreModels)
			{
				updateBookModel.genreSelected.Add(item.Id);
			}
			return View(updateBookModel);
		}

		// POST: BookController/Edit/5
		[HttpPost("Book/Edit")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(UpdateBookModel book, IFormFile imageFile)
		{
			try
			{
				var convert = ConvertToValidString(book.Title);
				//...

				if (imageFile != null && imageFile.Length > 0)
				{
					// Kiểm tra nếu có ảnh cũ
					if (!string.IsNullOrEmpty(book.Img))
					{
						// Xóa ảnh cũ
						var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "book", book.Img);
						if (System.IO.File.Exists(oldImagePath))
						{
							System.IO.File.Delete(oldImagePath);
						}
					}

					var extension = Path.GetExtension(imageFile.FileName);
					var filename = "Book_" + convert + "_" + Guid.NewGuid().ToString() + extension;
					var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "book", filename);

					var stream = new FileStream(path, FileMode.Create);
					imageFile.CopyTo(stream);
					book.Img = filename;
				}

				//...

				if (book.Quantity == 0)
				{
					book.Status = 0;
				}
				var result = await _bookService.Update(book);
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

	}
}
