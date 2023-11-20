using BookShop.BLL.ConfigurationModel.AuthorModel;
using BookShop.BLL.IService;
using BookShop.DAL.Entities;
using BookShop.Web.Client.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Web.Client.Areas.Admin.Controllers.BookController
{
    [Area("Admin")] // cấu hình cái này để nó biết đây là trang quan lí
    [Route("admin/Author")] // thêm đường dẫn ở đây để tránh bị trùng với với các from khác ( bắt buộc ) đặt tên giống Bảng code u
    public class AuthorController : Controller
    {
        private readonly IAuthorService _authorService;
        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet]
        [Route("listauthor")]
        public async Task<IActionResult> Index([FromQuery(Name = "p")] int currentPages)
        {
            var author = await _authorService.Getall();
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
                generateUrl = (int? p) => Url.Action("Index", "Author", new { areas = "Admin", p = p, pagesize = pagesize })
            };
            ViewBag.pagingmodel = pagingmodel;
            author = author.Skip((pagingmodel.currentpage - 1) * pagesize).Take(pagesize).ToList();
            return View(author);
        }

        [HttpGet("Create/author")]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost("Create/author")]
        public async Task<IActionResult> Create(Author author, IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", imageFile.FileName);
                var stream = new FileStream(path, FileMode.Create);
                imageFile.CopyTo(stream);
                author.Img = imageFile.FileName;
            }
            if (!ModelState.IsValid)
            {
                var createAuthorModel = new CreateAuthorModel
                {
                    Name = author.Name,
                    Img = author.Img,
                    Index = author.Index,
                    CreatedDate = author.CreatedDate,
                    Status = author.Status
                };

                await _authorService.Add(createAuthorModel);
                return RedirectToAction("Index");
            }
            return View(author);
        }
        [HttpGet("Edit/author/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var author = await _authorService.GetById(id);
            return View(author);
        }
        [HttpPost("Edit/author/{id}")]
        public async Task<IActionResult> Edit(int id, Author author)
        {         
            if (!ModelState.IsValid)
            {
                var updateAuthorModel = new UpdateAuthorModel
                {
                    Name = author.Name,
                    Img = author.Img,
                    Index = author.Index,
                    Status = author.Status
                };

                await _authorService.Update(id, updateAuthorModel);
                return RedirectToAction("Index");
            }
            return View(author);

        }

        [HttpGet("Detail/author/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var author = await _authorService.GetById(id);
            return View(author);
        }

        //[HttpDelete("Delete/author/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _authorService.Delete(id))
            {
                return RedirectToAction("Index");
            }
            else return BadRequest();
        }
    }
}
