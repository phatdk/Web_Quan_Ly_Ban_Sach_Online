using BookShop.Web.Client.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Web.Client.Areas.Admin.Controllers
{
	public class HomeController : Controller
	{
		[Area("Admin")]
		public IActionResult Index()
		{
			IList<BookViewModel> list = new List<BookViewModel>();
			list.Add(new BookViewModel() { Id = 1, Title = "300 bài code thiếu nhi" });
			list.Add(new BookViewModel() { Id = 2, Title = "300 bài code thiếu nhi new version" });
			ViewData["BookList"] = list;
			return View();
		}
	}
}
