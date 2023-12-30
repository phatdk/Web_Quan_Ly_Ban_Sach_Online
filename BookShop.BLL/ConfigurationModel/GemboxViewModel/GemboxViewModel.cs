using BookShop.BLL.ConfigurationModel.OrderModel;
using BookShop.BLL.ConfigurationModel.ProductModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.ConfigurationModel.GemboxViewModel
{
    public class GemboxViewModel
    {
        public string ProductName { get; set; }
        public int QuantitySold { get; set; }
        public int TotalAmount { get; set; }
        public List<OrderViewModel> TopSales { get; set; }
        public List<ProductViewModel> ProductList { get; set; }
        public int? GetTotalRevenue {  get; set; }
        public int? GetTotalRevenueByDay { get; set; }
        public int? GetTotalRevenueByMonth { get; set; }


    }
    
}