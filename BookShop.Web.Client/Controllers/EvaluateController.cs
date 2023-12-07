using BookShop.BLL.ConfigurationModel.EvaluateModel;
using BookShop.BLL.IService;
using BookShop.BLL.Service;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace BookShop.Web.Client.Controllers
{
    public class EvaluateController : Controller
    {

        private readonly IEvaluateService Evaluate;

        public EvaluateController(IEvaluateService evaluate)
        {
            Evaluate = evaluate;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ReplyComment(CreateEvaluateModel model)
        {
            var comment = new CreateEvaluateModel()
            {
                Id_Parents = model.Id_Parents,
                Content = model.Content,
                Point = 5,
                Id_User = model.Id_User,
                Id_Product = model.Id_Product
            };
            var statusadd = await Evaluate.Add(comment);
            if (!statusadd)
            {
                return Content("đã có lỗi xảy ra");
            }
            return Redirect($"/Home/ChiTietSanPham/{model.Id_Product}");
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment(CreateEvaluateModel Comment)
        {
            Comment.Point =5;
            var statusadd = await Evaluate.Add(Comment);
            if (!statusadd)
            {
                return Content("đã có lỗi xảy ra");
            }
            return Redirect($"/Home/ChiTietSanPham/{Comment.Id_Product}");
        }
    }
}
