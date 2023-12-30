using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.ConfigurationModel.GemboxViewModel
{
    public class TopSalingProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int QuantitySold { get; set; }
        public decimal TotalRevenue { get; set; }   
        public int Price { get; set; }  
    }
}
