using Microsoft.AspNetCore.Mvc;

namespace BookShop.Web.Client.Controllers
{
	public class Admincontroll : Controller
	{
		private readonly ILogger<Admincontroll> _logger;

		public Admincontroll(ILogger<Admincontroll> logger)
		{
			_logger = logger;
		}
		public IActionResult MenuAdmin()
		{
			return View();
		}
	}
}
