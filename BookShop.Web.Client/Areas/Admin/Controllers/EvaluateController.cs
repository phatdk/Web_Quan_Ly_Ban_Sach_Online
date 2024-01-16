using BookShop.BLL.ConfigurationModel.EvaluateModel;
using BookShop.BLL.IService;
using BookShop.BLL.Service;
using BookShop.Web.Client.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text;

namespace BookShop.Web.Client.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles = "Admin,Staff")]
	public class EvaluateController : Controller
    {
        private readonly IEvaluateService _evaluateService;
        private List<EvaluateViewModel> _evaluateList;
        EvaluateViewModel _evaluateViewModel;

        public EvaluateController(IEvaluateService evaluateService)
        {
            _evaluateService = evaluateService;
            _evaluateList = new List<EvaluateViewModel>();
            _evaluateViewModel = new EvaluateViewModel();
        }
        public async Task<IActionResult> Getdata(int page, int? status, string? keyWord)
        {
            _evaluateList = await _evaluateService.GetAll();
            if (keyWord != null)
            {
                _evaluateList = _evaluateList.Where(c => c.NameUser.Contains(keyWord)).ToList();
            }

            var listBook = _evaluateList.OrderByDescending(c => c.CreatedDate).ToList();
            int pageSize = 10;
            double totalPage = (double)listBook.Count / pageSize;
            listBook = listBook.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return Json(new { data = listBook, page = page, max = Math.Ceiling(totalPage) });
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
        public async Task<IActionResult> Index([FromQuery(Name = "p")] int currentPages)
        {
            var author = await _evaluateService.GetAll();
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
                generateUrl = (int? p) => Url.Action("Index", "Evaluate", new { areas = "Admin", p = p, pagesize = pagesize })
            };
            ViewBag.pagingmodel = pagingmodel;
            author = author.Skip((pagingmodel.currentpage - 1) * pagesize).Take(pagesize).ToList();
            return View(author);
        }
    }
}
