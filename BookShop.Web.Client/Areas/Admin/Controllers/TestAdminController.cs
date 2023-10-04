using Microsoft.AspNetCore.Mvc;

namespace BookShop.Web.Client.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Route("admin/testadmin")]
	public class TestAdminController : Controller
	{
		
		public IActionResult Index()
		{
			return View();
		}
	}
}
