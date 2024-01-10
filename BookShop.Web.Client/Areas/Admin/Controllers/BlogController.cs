using BookShop.BLL.ConfigurationModel.AuthorModel;
using BookShop.BLL.ConfigurationModel.BookModel;
using BookShop.BLL.ConfigurationModel.NewsModel;
using BookShop.BLL.IService;
using BookShop.BLL.Service;
using BookShop.DAL.Entities;
using BookShop.Web.Client.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text;

namespace BookShop.Web.Client.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BlogController : Controller
    {
        private readonly INewsService _newService;
        List<NewsViewModel> _listBook;
        NewsViewModel _news;

        public BlogController(INewsService newService)
        {
            _news = new NewsViewModel();
            _newService = newService;
            _listBook = new List<NewsViewModel>();
        }
        public async Task<IActionResult> Getdata(int page, int? status, string? keyWord)
        {
            _listBook = await _newService.GetAll();
            if (keyWord != null)
            {
                _listBook = _listBook.Where(c => c.Title.Contains(keyWord)).ToList();
            }

            var listBook = _listBook.OrderByDescending(c => c.CreatedDate).ToList();
            int pageSize = 10;
            double totalPage = (double)listBook.Count / pageSize;
            listBook = listBook.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return Json(new { data = listBook, page = page, max = Math.Ceiling(totalPage) });
        }
        public async Task<IActionResult> Details(int id)
        {
            _news = await _newService.GetById(id);
            return View(_news);
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
        [HttpGet("Blog/Create")]
        // GET: BookController/Create
        public async Task<IActionResult> Create()
        {
            return View();
        }

        // POST: BookController/Create
        [HttpPost("Blog/Create")]
        public async Task<IActionResult> Create(CreateNewsModel model, IFormFile imageFile)
        {
            var convert = ConvertToValidString(model.Title);
            if (imageFile != null && imageFile.Length > 0)
            {
                var extension = Path.GetExtension(imageFile.FileName);
                var filename = "Blog_" + convert + extension;
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "blog", filename);

                var stream = new FileStream(path, FileMode.Create);
                imageFile.CopyTo(stream);
                model.Img = filename;
            }
            if (!ModelState.IsValid)
            {
                var createNewModel = new CreateNewsModel
                {
                    Title = model.Title,
                    Content = model.Content,
                    Description = model.Description,
                    Status = model.Status,
                    Img = model.Img,
                };
                await _newService.Add(createNewModel);
                return RedirectToAction("Index");
            }
            return View(model);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var author = await _newService.GetById(id);
            return View(author);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, NewsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var authorud = new NewsViewModel
                {
                    Title = model.Title,
                    Content = model.Content,
                    Description = model.Description,
                    Status = model.Status,
                    Img = model.Img,
                };

                await _newService.Update(id, authorud);
                return RedirectToAction("Index");
            }
            return View(model);

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
        static string ConvertToValidString(string input)
        {
            // Loại bỏ dấu tiếng Việt
            string removedDiacritics = RemoveDiacritics(input);

            // Chuyển thành chữ thường và loại bỏ khoảng trắng
            string result = removedDiacritics.ToLower().Replace(" ", "");

            return result;
        }
        static string RemoveDiacritics(string text)
        {
            string normalizedString = text.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in normalizedString)
            {
                UnicodeCategory category = CharUnicodeInfo.GetUnicodeCategory(c);
                if (category != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
