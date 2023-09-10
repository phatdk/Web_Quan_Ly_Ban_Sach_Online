using BookShop.BLL.ConfigurationModel.BookModel;
using BookShop.BLL.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		private readonly IBookService _bookService;
		private readonly IProductService _productService;
		private readonly IAuthorService _authorService;
		private readonly IGenreService _genreService;
		private readonly IBookAuthorService _bookAuthorService;
		private readonly IBookGenreService _bookGenreService;

		public ProductController(IBookService bookService, IProductService productService, IAuthorService authorService, IGenreService genreService, IBookAuthorService bookAuthorService, IBookGenreService bookGenreService)
		{
			_bookService = bookService;
			_productService = productService;
			_authorService = authorService;
			_genreService = genreService;
			_bookAuthorService = bookAuthorService;
			_bookGenreService = bookGenreService;
		}

		[HttpGet]
		public async Task<List<BookViewModel>> GetList(string page)
		{
			var objlist = await _bookService.GetAll();
			foreach (var item in objlist)
			{
				var authors = await _bookAuthorService.GetByBook(item.Id);
				foreach (var author in authors)
				{
					item.authorModels.Add(await _authorService.GetById(author.Id_Author));
				}
				var genres = await _bookGenreService.GetByBook(item.Id);
				foreach( var genre in genres)
				{
					item.genreModels.Add(await _genreService.GetById(genre.Id_Genre));				}
			}
			int pageNumber;
			if(page != null)
			{
				pageNumber = int.Parse(page);
				var pagesize = 10;
				var skip = (pageNumber - 1) * pagesize;

				return objlist.Skip(skip).Take(pagesize).ToList();
			}
			return objlist;
		}
	}
}
