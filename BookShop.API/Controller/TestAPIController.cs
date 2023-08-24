using BLL.IService.IAuthorService;
using BookShop.BLL.IService.IBookService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.API.Controller
{
	[Route("api/[controller]")]
	[ApiController]
	public class TestAPIController : ControllerBase
	{
		public IBookService _BookService;
		public IAuthorService _AuthorService;

		public TestAPIController(IBookService BookService,
			IAuthorService AuthorService
			)
		{
			_AuthorService = AuthorService;
			_BookService = BookService;
		}

		
		[HttpGet("getalldauthor")]
		public async Task<IActionResult> getauthor() {
			var ok = await _AuthorService.Getall();
			return Ok(ok);
		} 
			
    }
}
