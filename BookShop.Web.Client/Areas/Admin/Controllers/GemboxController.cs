using Microsoft.AspNetCore.Mvc;
using BookShop.BLL.Gembox;
using Microsoft.AspNetCore.Mvc.Filters;
using NuGet.Protocol.Plugins;
using Microsoft.EntityFrameworkCore;
using BookShop.BLL.Service;
using SkiaSharp;
using System.Linq;
using BookShop.BLL.IService;
using BookShop.DAL.Entities;
using BookShop.BLL.ConfigurationModel.OrderDetailModel;
using BookShop.BLL.ConfigurationModel.ProductModel;

namespace BookShop.Web.Client.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GemboxController : Controller
    {
        public Gen _gen;
        public OrderDetailService _orderdettailservices;
        public OrderService _OrderService;
        public ProductService _ProductService;

        public GemboxController()
        {
            _gen = new Gen();
            _ProductService = new ProductService();
            _OrderService = new OrderService();
            _orderdettailservices = new OrderDetailService();
          
        }
        public IActionResult export()
        {

            _gen.xuatExel();
            Ok("Xuaat file ok").ToString();
            return View("Index");

        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult product()
        {
            return View();
        }

        public async Task<IActionResult> Topsale()
        {
            var orders = await _OrderService.GetAll();

            // Lấy danh sách chi tiết đơn đặt hàng cho mỗi đơn đặt hàng
            var orderDetails = await Task.WhenAll(orders.Select(async order =>
            {
                var details = await _orderdettailservices.GetByOrder(order.Id);
                return details?.ToList() ?? new List<OrderDetailViewModel>();
            }));

            var flattenedOrderDetails = orderDetails.SelectMany(details => details).ToList();

            var top5Products = flattenedOrderDetails
                .GroupBy(orderDetail => orderDetail.Id_Product)
                .Select(group =>
                {
                    var productId = group.Key;
                    var totalQuantity = group.Sum(orderDetail => orderDetail.Quantity);
                    return new { ProductId = productId, TotalQuantity = totalQuantity };
                })
                .OrderByDescending(x => x.TotalQuantity)
                .ToList();

            var top5ProductsInfo = new List<ProductViewModel>();

            foreach (var product in top5Products)
            {
                var productInfo = await _ProductService.GetById(product.ProductId);
                if (productInfo != null)
                {
                   // productInfo.TotalQuantity = productInfo.TotalQuantity;
                    top5ProductsInfo.Add(productInfo);
                }
            }

            ViewBag.Top5 = top5ProductsInfo;
            return View();
        }


    }

}
