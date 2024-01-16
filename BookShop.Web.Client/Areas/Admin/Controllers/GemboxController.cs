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
using BookShop.BLL.ConfigurationModel.GemboxViewModel;
using BookShop.BLL.ConfigurationModel.OrderModel;
using Newtonsoft.Json;
using Humanizer;
using System.Security.Cryptography.X509Certificates;
using MailKit.Search;

namespace BookShop.Web.Client.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GemboxController : Controller
    {
        public Gen _gen;
        public List<OrderViewModel> _orderViewModels;
        public OrderDetailService _orderdettailservices;
        public OrderService _OrderService;
        public ProductService _ProductService;
        public StatusOrderService _statusservice;
        public ProductBookService _productbookservices;
        public BookService _bookService;

        public GemboxController()
        {
            _gen = new Gen();
            _ProductService = new ProductService();
            _OrderService = new OrderService();
            _orderdettailservices = new OrderDetailService();
            _statusservice = new StatusOrderService();

        }
        public IActionResult export()
        {
            _gen.xuatExel();
            return View();
        }
        public async Task<IActionResult> Index()
        {
            var orders = await _OrderService.GetAll();

            var productSales = new Dictionary<int, int>();
            foreach (var order in orders)
            {
                var orderDetails = await _orderdettailservices.GetByOrder(order.Id);
                // Cập nhật số lượng bán cho từng sản phẩm
                foreach (var orderDetail in orderDetails)
                {
                    productSales.TryGetValue(orderDetail.Id_Product, out var quantity);
                    productSales[orderDetail.Id_Product] = quantity + orderDetail.Quantity;
                }
            }

            // Lấy danh sách sản phẩm bán chạy
            var topSellingProducts = productSales.OrderByDescending(x => x.Value)
                                                 .Select(x => new TopSalingProduct
                                                 {
                                                     Id = x.Key,
                                                     QuantitySold = x.Value
                                                 })
                                                 .ToList();



            foreach (var product in topSellingProducts)
            {
                var productDetails = await _ProductService.GetById(product.Id);
                product.TotalRevenue = productDetails.Price * product.QuantitySold;
            }

            foreach (var product in topSellingProducts)
            {
                var productDetails = await _ProductService.GetById(product.Id);
                product.Name = productDetails.Name;
                product.Price = productDetails.Price;
            }
            ViewBag.ProductList = topSellingProducts.Take(5);

            return View();
        }

        public async Task<IActionResult> GetTotalRevenue()
        {




            var orders = await _OrderService.GetAll();
            var totalRevenue = 0;
            foreach (var order in orders)
            {

                var orderDetails = await _orderdettailservices.GetByOrder(order.Id);
                var or = new OrderViewModel();
                // Cập nhật tổng doanh thu
                foreach (var orderDetail in orderDetails)
                {
                    totalRevenue += orderDetail.Quantity * orderDetail.Price;

                }
            }





            return Json(totalRevenue);
        }
        public async Task<IActionResult> GetTotalDateRevenue()
        {



            var orders = await _OrderService.GetAll();

            orders = orders.FindAll(x => x.CreatedDate.Date == DateTime.Now.Date);


            var totalDateRevenue = 0;
            // Duyệt qua List<OrderViewModel>
            foreach (var order in orders)
            {
                var orderDetails = await _orderdettailservices.GetByOrder(order.Id);
                foreach (var orderDetail in orderDetails)
                {
                    totalDateRevenue += orderDetail.Quantity * orderDetail.Price;

                }
            }





            return Json(totalDateRevenue);
        }
        public async Task<IActionResult> GetTotalRevenueByMonth()
        {



            var orders = await _OrderService.GetAll();

            // Lọc đơn hàng theo ngày
            orders = orders.FindAll(x => x.CreatedDate.Month == DateTime.Now.Month);
            var TotalRevenuebyMuonth = 0;

            foreach (var order in orders)
            {
                var orderDetails = await _orderdettailservices.GetByOrder(order.Id);

                // Cập nhật tổng doanh thu
                foreach (var orderDetail in orderDetails)
                {
                    TotalRevenuebyMuonth += orderDetail.Quantity * orderDetail.Price;
                }
            }


            return Json(TotalRevenuebyMuonth);
        }


        [HttpGet]
        public async Task<IActionResult> Chart(int? month, int? year, int? online)
        {
            var orders = await _OrderService.GetAll();
            var totalRevenueByDay = new Dictionary<int, decimal>();
            if (online == null)
            {
                var orderss = orders.Where(x => x.CreatedDate.Month == month && x.CreatedDate.Year == year);
                // tạo biến truyền về view
                foreach (var order in orderss)
                {
                    var day = order.CreatedDate.Day;
                    if (totalRevenueByDay.ContainsKey(day))
                    {
                        totalRevenueByDay[day] += order.TotalRevenue;
                    }
                    else
                    {
                        totalRevenueByDay.Add(day, order.TotalRevenue);
                    }
                }
            }
            else
            {
                var isonline = online == 1 ? true : false;
                var orderss = orders.Where(x => x.CreatedDate.Month == month && x.CreatedDate.Year == year && x.IsOnlineOrder == isonline);


                // tạo biến truyền về view
                foreach (var order in orderss)
                {
                    var day = order.CreatedDate.Day;
                    if (totalRevenueByDay.ContainsKey(day))
                    {
                        totalRevenueByDay[day] += order.TotalRevenue;
                    }
                    else
                    {
                        totalRevenueByDay.Add(day, order.TotalRevenue);
                    }
                }
            }

            return Json(totalRevenueByDay);
        }


        public async Task<IActionResult> GetHighestInvoice()
        {
            var invoices = await _OrderService.GetAll();
            var topInvoices = invoices.OrderByDescending(x => x.Total).ToList();
            ViewBag.HighestInvoice = topInvoices;

            return Json(topInvoices);


        }
        public async Task<IActionResult> GetProductsSortedByQuantityAsc()
        {
            var products = await _ProductService.GetAll();

            var sortedProducts = products.OrderBy(x => x.Quantity).ToList();
            ViewBag.ProductsSortedByQuantityAsc = sortedProducts;

            return Json(sortedProducts);

        }
        public async Task<IActionResult> GetOrderByStatus()
        {

            var orders = await _OrderService.GetAll();


            var orderssdone = orders.Where(x => x.Id_Status == 1).Count();
            var orderwaitingtouse = orders.Where(orders => orders.Status == 6).Count();
            var ordercancel = orders.Where(x => x.Id_Status == 10).Count();

            var orderCounts = new
            {
                orderssdone = orderssdone,
                orderwaitingtouse = orderwaitingtouse,
                ordercancel = ordercancel
            };
            return Json(orderCounts);
        }
        public async Task<IActionResult> GetProductBook()
        {
            var productbook = await _productbookservices.GetAll();

            return Json(productbook);
        }
        public async Task<IActionResult> GetAllHighestInvoice()
        {
            return View();
        }
        public async Task<IActionResult> GetAllProducSortedByQuatityAsc()
        {
            return View();
        }
        public async Task<IActionResult> GetAllProductBook()
        {
            return View();
        }
        public async Task<IActionResult> GetTopsale()
        {
            var orders = await _OrderService.GetAll();

            var productSales = new Dictionary<int, int>();
            foreach (var order in orders)
            {
                var orderDetails = await _orderdettailservices.GetByOrder(order.Id);
                // Cập nhật số lượng bán cho từng sản phẩm
                foreach (var orderDetail in orderDetails)
                {
                    productSales.TryGetValue(orderDetail.Id_Product, out var quantity);
                    productSales[orderDetail.Id_Product] = quantity + orderDetail.Quantity;
                }
            }

            // Lấy danh sách sản phẩm bán chạy
            var topSellingProducts = productSales.OrderByDescending(x => x.Value)
                                                 .Select(x => new TopSalingProduct
                                                 {
                                                     Id = x.Key,
                                                     QuantitySold = x.Value
                                                 })
                                                 .ToList();



            foreach (var product in topSellingProducts)
            {
                var productDetails = await _ProductService.GetById(product.Id);
                product.TotalRevenue = productDetails.Price * product.QuantitySold;
            }

            foreach (var product in topSellingProducts)
            {
                var productDetails = await _ProductService.GetById(product.Id);
                product.Name = productDetails.Name;
                product.Price = productDetails.Price;
            }
            return Json(topSellingProducts);
        }
        public async Task<IActionResult> GetAllTopsale()
        {


            return View();

        }
    }
}

