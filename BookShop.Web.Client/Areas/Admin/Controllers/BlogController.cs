using BookShop.BLL.ConfigurationModel.NewsModel;
using BookShop.BLL.IService;
using BookShop.BLL.Service;
using BookShop.Web.Client.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace BookShop.Web.Client.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BlogController : Controller
    {
        private readonly INewsService _newService;

        public BlogController(INewsService newService)
        {
            _newService = newService;
        }

        public async Task<IActionResult> Index([FromQuery(Name = "p")] int currentPages)
        {
            var author = await _newService.GetAll();
            int pagesize = 10;
            if (pagesize <= 0)
            {
                pagesize = 10;
            }
            int countPages = (int)Math.Ceiling((double)author.Count() / pagesize);
            if (currentPages > countPages)
            {
                currentPages = countPages;
            }
            if (currentPages < 1)
            {
                currentPages = 1;
            }

            var pagingmodel = new PagingModel()
            {
                currentpage = currentPages,
                countpages = countPages,
                generateUrl = (int? p) => Url.Action("Index", "Blog", new { areas = "Admin", p = p, pagesize = pagesize })
            };
            ViewBag.pagingmodel = pagingmodel;
            author = author.Skip((pagingmodel.currentpage - 1) * pagesize).Take(pagesize).ToList();
            return View(author);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost, ActionName("Create"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCF(CreateNewsModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var a = await _newService.Add(model);
            if (a != false)
            {
                return RedirectToAction("Index");
            }
            return View();

        }
        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var a = await _newService.GetById(Id);
            if (a == null)
            {
                return NotFound();
            }
            return View(a);
        }
        [HttpPost, ActionName("Edit"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCF(int Id, NewsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var a = await _newService.Update(Id,model);
            if (a != false)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
        public async Task<IActionResult> Delete(int Id)
        {
            var a = await _newService.Delete(Id);
            if (a != false)
            {
                return RedirectToAction("Index");
            }
            return View();
         
        }
    }
}
