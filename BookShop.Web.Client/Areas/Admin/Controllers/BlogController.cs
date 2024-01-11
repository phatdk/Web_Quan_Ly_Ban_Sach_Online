using BookShop.BLL.ConfigurationModel.AuthorModel;
using BookShop.BLL.ConfigurationModel.BookModel;
using BookShop.BLL.ConfigurationModel.NewsModel;
using BookShop.BLL.IService;
using BookShop.BLL.Service;
using BookShop.DAL.Entities;
using BookShop.Web.Client.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging.Signing;
using System.Globalization;
using System.Text;

namespace BookShop.Web.Client.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Staff")]
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
            var news = await _newService.GetById(id);
            return View(news);
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
        public async Task<IActionResult> Create(CreateNewsModel model)
        {
            var convert = ConvertToValidString(model.Title);
            if (model.imageFile != null && model.imageFile.Length > 0)
            {
                var extension = Path.GetExtension(model.imageFile.FileName);
                var filename = "Blog_" + convert + extension;
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "blog", filename);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    model.imageFile.CopyTo(stream);
                    model.Img = filename;
                }

            }
            var createNewModel = new CreateNewsModel
            {
                Title = model.Title,
                Content = model.Content,
                Description = model.Description,
                Status = model.Status,
                Img = model.Img,
            };
            var statusCreate = await _newService.Add(createNewModel);
            if (!statusCreate)
            {
                return View(model);
            }
            return RedirectToAction("Index");


        }
        public async Task<IActionResult> Edit(int id)
        {
            var author = await _newService.GetById(id);
            return View(author);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, CreateNewsModel model)
        {
            string newFileName = "";
            if (model.imageFile != null)
            {
                newFileName = Guid.NewGuid().ToString() + model.imageFile.FileName;
            }

            model.Img = newFileName;
            var statusUpdate = await _newService.Update(id, model);
            if (!statusUpdate)
            {

                return View(model);

            }
            if (model.imageFile != null)
            {
                var blog = await _newService.GetById(id);
                var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "blog", blog.Img);

                // Nếu tệp cũ tồn tại, xóa nó
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }

               
                var newFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "blog", newFileName);

                using (var stream = new FileStream(newFilePath, FileMode.Create))
                {
                    model.imageFile.CopyTo(stream);
                }

                 
            }

            return RedirectToAction("Index");
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
