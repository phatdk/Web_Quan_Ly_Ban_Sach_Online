using Microsoft.AspNetCore.Mvc;

namespace BookShop.Web.Client.Areas.Admin.Controllers.BookController
{
    public class BookController : Controller
    { 

        public IActionResult Index()
        {
            return View();
        }
    }
}
