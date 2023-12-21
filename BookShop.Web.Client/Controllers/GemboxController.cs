using Microsoft.AspNetCore.Mvc;
using BookShop.BLL.Gembox;
using Microsoft.AspNetCore.Mvc.Filters;
using NuGet.Protocol.Plugins;

namespace BookShop.Web.Client.Controllers
{
	public class GemboxController : Controller
	{
		public Gen _gen;


		public GemboxController()
        {
            _gen = new Gen();
        }
		public IActionResult export()
		{

			_gen.xuatExel();
			Ok("Xuaat file ok").ToString();
			return View("Index");

		}
        public IActionResult Index()
		{
			return View();
		}
	}
}
