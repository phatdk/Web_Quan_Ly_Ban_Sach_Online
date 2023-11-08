using BookShop.BLL.ConfigurationModel.NewsModel;
using BookShop.BLL.IService;
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

        public async Task<IActionResult> Index()
        {
            return View(await _newService.GetAll());
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
