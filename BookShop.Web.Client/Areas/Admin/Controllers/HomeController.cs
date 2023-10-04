using BookShop.Web.Client.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Web.Client.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class HomeController : Controller
	{
		
		public IActionResult Index()
		{
			return View();
		}
	}
}
