using BookShop.BLL.ConfigurationModel.AuthorModel;
using BookShop.BLL.IService;
using BookShop.BLL.Service;
using BookShop.DAL.Entities;
using BookShop.Web.Client.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using System.Globalization;
using System.Text;

namespace BookShop.Web.Client.Areas.Admin.Controllers.BookController
{
	[Authorize(Roles = "Admin")]
	[Area("Admin")] // cấu hình cái này để nó biết đây là trang quan lí
	public class AuthorController : Controller
	{
		private readonly IAuthorService _authorService;
		List<AuthorModel> _authorView;
		public AuthorController(IAuthorService authorService)
		{
			_authorView = new List<AuthorModel>();
			_authorService = authorService;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			return View();
		}
		public async Task<IActionResult> Getdata(int page, int? status, string? keyWord)
		{
			_authorView = await _authorService.Getall();
			if (keyWord != null)
			{
				_authorView = _authorView.Where(c => c.Name.Contains(keyWord)).ToList();
			}
			if (status != null)
			{
				_authorView = _authorView.Where(c => c.Status == Convert.ToInt32(status)).ToList();
			}
			var authorView = _authorView.OrderByDescending(c => c.CreatedDate).ToList();
			int pageSize = 10;
			double totalPage = (double)authorView.Count / pageSize;
			authorView = authorView.Skip((page - 1) * pageSize).Take(pageSize).ToList();
			return Json(new { data = authorView, page = page, max = Math.Ceiling(totalPage) });
		}
		[HttpGet("Create/author")]
		public async Task<IActionResult> Create()
		{
			return View();
		}
		[HttpPost("Create/author")]
		public async Task<IActionResult> Create(Author author, IFormFile imageFile)
		{

			var convert = ConvertToValidString(author.Name);
			if (imageFile != null && imageFile.Length > 0)
			{
				var extension = Path.GetExtension(imageFile.FileName);
				var filename = "Author_" + convert + extension;
				var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "author", filename);

				var stream = new FileStream(path, FileMode.Create);
				imageFile.CopyTo(stream);
				author.Img = filename;

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
		public async Task<IActionResult> Edit(int id)
		{
			var author = await _authorService.GetById(id);
			return View(author);
		}
		[HttpPost]
		public async Task<IActionResult> Edit(int id, AuthorModel author)
		{
			if (!ModelState.IsValid)
			{
				var authorud = new AuthorModel
				{
					Name = author.Name,
					Img = author.Img,
					Index = author.Index,
					Status = author.Status
				};

				await _authorService.Update(id, authorud);
				return RedirectToAction("Index");
			}
			return View(author);

		}


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
