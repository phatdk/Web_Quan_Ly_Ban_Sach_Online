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
        private readonly IOrderDetailService _OrderDetails;

        public EvaluateController(IEvaluateService evaluate, IOrderDetailService orderDetails )
        {
            Evaluate = evaluate;
            _OrderDetails = orderDetails;
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
                Point = model.Point,
                Id_User = model.Id_User,
                Id_Product = model.Id_Product
            };
            if (model.Point==null)
            {
                comment.Point = 5;
            }
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
            var orderdetails= (await _OrderDetails.GetAll()).Where(x => x.Id_User == Comment.Id_User).FirstOrDefault();
            if (orderdetails==null)
            {
                return Content("đã có lỗi xảy ra");
            }
            Comment.Id_User = orderdetails.Id_User;
            Comment.Id_Product = orderdetails.Id;
            if (Comment.Point == -1)
            {
                Comment.Point = 5;
            }
            //	Comment.Point =5;
            var statusadd = await Evaluate.Add(Comment);
            if (!statusadd)
            {
                return Content("đã có lỗi xảy ra");
            }
            return Redirect($"/Home/ChiTietSanPham/{orderdetails.Id_Product}");
        }
    }
}
