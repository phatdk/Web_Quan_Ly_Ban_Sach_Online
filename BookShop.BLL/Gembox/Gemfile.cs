using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShop.BLL.IService;
using BookShop.BLL.Service;
using BookShop.DAL.Entities;
//using GemBox.Spreadsheet;
using Microsoft.Data.SqlClient;

namespace BookShop.BLL.Gembox
{
    public class Gemfile
    {
        public IOrderService _IorderService;
        public IOrderDetailService _Iorderdetailservice;
        
        public Gemfile()
        {
           // _IorderService = new OrderService();
            _Iorderdetailservice = new OrderDetailService();
            
        }
       public void GetExel()
        {
           
        }
    }
}
