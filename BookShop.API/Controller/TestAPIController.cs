
using BookShop.BLL.ConfigurationModel.BookModel;
using BookShop.BLL.IService.IBookAuthorService;
using BookShop.BLL.IService.IBookService;
using BookShop.DAL.Entities;
using BookShop.DAL.Repositopy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.API.Controller
{
	[Route("api/[controller]")]
	[ApiController]
	public class TestAPIController : ControllerBase
	{
		
		
		protected readonly IBookService _BookService;
		protected readonly IBookAuthorService _BookAuthorService;

		public TestAPIController(
			IBookService BookService, IBookAuthorService BookAuthorService
			)
		{
			_BookAuthorService = BookAuthorService;
			_BookService = BookService;
		}

		
		[HttpGet("getall")]
		public async Task<IActionResult> Getall()
		{
			var ok = await _BookAuthorService.GetAllBooksAuthor();
		 return Ok(ok);
		}
		
	}
}
