using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShop.BLL.IService;
using BookShop.BLL.Service;
using BookShop.DAL.Entities;



using Microsoft.Data.SqlClient;
using GemBox.Spreadsheet;
using MailKit.Search;

namespace BookShop.BLL.Gembox
{
    public class Gemfile
    {
        public IOrderService _IorderService;
        public IOrderDetailService _Iorderdetailservice;
        public Order _order;
        public OrderDetail _orderdetail;

        
        public Gemfile()
        {
            _IorderService = new OrderService();
            _Iorderdetailservice = new OrderDetailService();
            
        }
        public async void GetExel()
        {
            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
            var workbook = new ExcelFile();
            var worksheet = workbook.Worksheets.Add("DataSheet");
            var orders = await _IorderService.GetAll();
           
          
            // Ghi dữ liệu từ danh sách vào bảng tính Excel.
            for (int row = 0; row < orders.Count; row++)
            {
                worksheet.Cells[row + 1, 0].Value = orders[row].Id;
                worksheet.Cells[row + 1, 1].Value = orders[row].Code;
                worksheet.Cells[row + 1, 2].Value = orders[row].Phone;
                worksheet.Cells[row + 1, 3].Value = orders[row].Receiver;
                worksheet.Cells[row + 1, 4].Value = orders[row].AcceptDate;
                worksheet.Cells[row + 1, 5].Value = orders[row].CreatedDate;
                worksheet.Cells[row + 1, 6].Value = orders[row].DeliveryDate;
                worksheet.Cells[row + 1, 7].Value = orders[row].ReceiveDate;
                worksheet.Cells[row + 1, 8].Value = orders[row].PaymentDate;
                worksheet.Cells[row + 1, 9].Value = orders[row].CompleteDate;
                worksheet.Cells[row + 1, 10].Value = orders[row].ModifiDate;
                worksheet.Cells[row + 1, 11].Value = orders[row].ModifiNotes;
                worksheet.Cells[row + 1, 12].Value = orders[row].Description;
                worksheet.Cells[row + 1, 13].Value = orders[row].City;
                worksheet.Cells[row + 1, 14].Value = orders[row].District;
                worksheet.Cells[row + 1, 15].Value = orders[row].Commune;
                worksheet.Cells[row + 1, 16].Value = orders[row].Id_User;
                worksheet.Cells[row + 1, 17].Value = orders[row].Id_Promotion;
                worksheet.Cells[row + 1, 18].Value = orders[row].NameUser;
                worksheet.Cells[row + 1, 19].Value = orders[row].NamePromotion;
            }

            workbook.Save("DataExport.xlsx");
            Console.WriteLine("Dữ liệu đã được xuất thành công thành tệp Excel.");
            //// Tính tổng doanh số theo tuần.
            //var weeklyRevenue = transactions
            //    .GroupBy(t => GetWeekOfYearISO8601(t.AcceptDate))
            //    .Select(group => new
            //    {
            //        Week = group.Key,
            //        TotalRevenue = group.Sum(t => t.Amount)
            //    })
            //    .OrderBy(r => r.Week)
            //    .ToList();

            //// Tính tổng doanh số theo tháng.
            //var monthlyRevenue = transactions
            //    .GroupBy(t => new { Year = t.AcceptDate.Year, Month = t.AcceptDate.Month })
            //    .Select(group => new
            //    {
            //        Year = group.Key.Year,
            //        Month = group.Key.Month,
            //        TotalRevenue = group.Sum(t => t.Amount)
            //    })
            //    .OrderBy(r => r.Year)
            //    .ThenBy(r => r.Month)
            //    .ToList();

            //// Tính tổng doanh số theo năm.
            //var yearlyRevenue = transactions
            //    .GroupBy(t => t.AcceptDate.Year)
            //    .Select(group => new
            //    {
            //        Year = group.Key,
            //        TotalRevenue = group.Sum(t => t.Amount)
            //    })
            //    .OrderBy(r => r.Year)
            //    .ToList();

            //// Tạo một tệp Excel và một bảng tính Excel.
            //var workbook = new ExcelFile();
            //var worksheet = workbook.Worksheets.Add("RevenueData");

            //// Ghi dữ liệu tổng doanh số theo tuần vào bảng tính Excel.
            //for (int row = 0; row < weeklyRevenue.Count; row++)
            //{
            //    worksheet.Cells[row, 0].Value = weeklyRevenue[row].Week;
            //    worksheet.Cells[row, 1].Value = weeklyRevenue[row].TotalRevenue;
            //}

            //// Ghi dữ liệu tổng doanh số theo tháng vào bảng tính Excel.
            //for (int row = 0; row < monthlyRevenue.Count; row++)
            //{
            //    worksheet.Cells[row, 3].Value = $"{monthlyRevenue[row].Month}/{monthlyRevenue[row].Year}";
            //    worksheet.Cells[row, 4].Value = monthlyRevenue[row].TotalRevenue;
            //}

            //// Ghi dữ liệu tổng doanh số theo năm vào bảng tính Excel.
            //for (int row = 0; row < yearlyRevenue.Count; row++)
            //{
            //    worksheet.Cells[row, 6].Value = yearlyRevenue[row].Year;
            //    worksheet.Cells[row, 7].Value = yearlyRevenue[row].TotalRevenue;
            //}

            // Lưu tệp Excel.
            workbook.Save("RevenueReport.xlsx");

            Console.WriteLine("Kết quả đã được xuất thành công vào tệp Excel.");

        }

    }
}