﻿using Microsoft.AspNetCore.Mvc;
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
            ViewBag.ProductList = topSellingProducts;
         
            return View();
        }

        public async Task<IActionResult> GetTotalRevenue()
        {
            var orders = await _OrderService.GetAll();
            var totalRevenue = 0;
            foreach (var order in orders)
            {

                var orderDetails = await _orderdettailservices.GetByOrder(order.Id);

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
        public async Task<IActionResult> Chart(int month, int year)
        {
            
            var orders = await _OrderService.GetAll();

            var orderss = orders.Where(x => x.CreatedDate.Month == 1 && x.CreatedDate.Year == 2023);
            var totalRevenueByDay = new Dictionary<int, decimal>();
             
           
            foreach (var order in orderss)
            {
                // Lấy ngày của đơn hàng
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
          

            // Trả về View
            return Json(totalRevenueByDay);
            
        }
    }

}
