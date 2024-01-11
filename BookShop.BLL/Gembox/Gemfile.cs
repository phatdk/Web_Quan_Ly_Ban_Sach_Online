using BookShop.BLL.IService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemBox.Spreadsheet;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using BookShop.BLL.Service;
using BookShop.BLL.ConfigurationModel.ProductBooklModel;
using BookShop.DAL.Entities;
using MailKit.Search;

namespace BookShop.BLL.Gembox
{
	public class Gen
	{
		public IOrderDetailService _OrderDetailService;
		public IOrderService _OrderService;
		public IProductService _ProductService;


		public Gen()
		{
			_OrderService = new OrderService();
			_OrderDetailService = new OrderDetailService();
			_ProductService = new ProductService();
		}
		public async void xuatExel()
		{

            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");



            var workbook = new ExcelFile();
			var worksheet = workbook.Worksheets.Add("Toàn bộ hóa đơn");
			// Đặt tiêu đề cho các cột
			worksheet.Cells["A1"].Value = "Code";
			worksheet.Cells["B1"].Value = "Người nhận";
			worksheet.Cells["C1"].Value = "Điện thoại";
			worksheet.Cells["D1"].Value = "Email";
			worksheet.Cells["E1"].Value = "Phí vận chuyển";
			worksheet.Cells["F1"].Value = "Ngày tạo";
			worksheet.Cells["G1"].Value = "Ngày xác nhận";
			worksheet.Cells["H1"].Value = "Ngày giao hàng";
			worksheet.Cells["I1"].Value = "Ngày nhận";
			worksheet.Cells["J1"].Value = "Ngày thanh toán";
			worksheet.Cells["K1"].Value = "Ngày hoàn thành";
			worksheet.Cells["L1"].Value = "Sửa đổi ngày";
			worksheet.Cells["M1"].Value = "Sửa đổi ghi chú";
			worksheet.Cells["N1"].Value = "Miêu tả";
			worksheet.Cells["O1"].Value = "Đặt hàng trực tuyến";
			worksheet.Cells["P1"].Value = "điểm sử dụng";
			worksheet.Cells["Q1"].Value = "Điểm đã sử dụng";
			worksheet.Cells["R1"].Value = "Số điểm";
			worksheet.Cells["S1"].Value = "Thành phố";
			worksheet.Cells["T1"].Value = "Huyện";
			worksheet.Cells["U1"].Value = "Xã";
			worksheet.Cells["V1"].Value = "Địa chỉ";
			worksheet.Cells["W1"].Value = "Tên Sản Phẩm";
			worksheet.Cells["X1"].Value = "ld_Staff";
			worksheet.Cells["Y1"].Value = "ld Promotion";
			worksheet.Cells["Z1"].Value = "Giá sản phẩm";

            int rowIndex = 2; // Bắt đầu từ dòng thứ hai sau tiêu đề

		
			foreach (var order in await _OrderService.GetAll())
			{
                foreach (var orderdelta in await _OrderDetailService.GetByOrder(order.Id))
                {
					var product =await _ProductService.GetById(orderdelta.Id_Product);
                        worksheet.Cells[$"A{rowIndex}"].Value = order.Code;
                        worksheet.Cells[$"B{rowIndex}"].Value = order.Receiver;
                        worksheet.Cells[$"C{rowIndex}"].Value = order.Phone;
                        worksheet.Cells[$"D{rowIndex}"].Value = order.Email;
                        worksheet.Cells[$"E{rowIndex}"].Value = order.Shipfee;
                        worksheet.Cells[$"F{rowIndex}"].Value = order.CreatedDate.ToString();
                        worksheet.Cells[$"G{rowIndex}"].Value = order.AcceptDate.ToString();
                        worksheet.Cells[$"H{rowIndex}"].Value = order.DeliveryDate.ToString();
                        worksheet.Cells[$"I{rowIndex}"].Value = order.ReceiveDate.ToString();
                        worksheet.Cells[$"J{rowIndex}"].Value = order.PaymentDate.ToString();
                        worksheet.Cells[$"K{rowIndex}"].Value = order.CompleteDate.ToString();
                        worksheet.Cells[$"L{rowIndex}"].Value = order.ModifiDate.ToString();
                        worksheet.Cells[$"M{rowIndex}"].Value = order.ModifiNotes;
                        worksheet.Cells[$"N{rowIndex}"].Value = order.Description;
                        worksheet.Cells[$"O{rowIndex}"].Value = order.IsOnlineOrder;
                        worksheet.Cells[$"P{rowIndex}"].Value = order.IsUsePoint;
                        worksheet.Cells[$"Q{rowIndex}"].Value = order.PointUsed;
                        worksheet.Cells[$"R{rowIndex}"].Value = order.PointAmount;
                        worksheet.Cells[$"S{rowIndex}"].Value = order.City;
                        worksheet.Cells[$"T{rowIndex}"].Value = order.District;
                        worksheet.Cells[$"U{rowIndex}"].Value = order.Commune;
                        worksheet.Cells[$"V{rowIndex}"].Value = order.Address;
                        worksheet.Cells[$"W{rowIndex}"].Value = product.Name;
                        worksheet.Cells[$"X{rowIndex}"].Value = order.StatusName;
                        worksheet.Cells[$"Y{rowIndex}"].Value = order.Id_Promotions;
                        worksheet.Cells[$"Z{rowIndex}"].Value = orderdelta.Price;

                        rowIndex++;

                    
              
			}
            };
        
            var worksheet1 = workbook.Worksheets.Add("Hóa đơn tháng này");

            worksheet1.Cells["A1"].Value = "Code";
            worksheet1.Cells["B1"].Value = "Người nhận";
            worksheet1.Cells["C1"].Value = "Điện thoại";
            worksheet1.Cells["D1"].Value = "Email";
            worksheet1.Cells["E1"].Value = "Phí vận chuyển";
            worksheet1.Cells["F1"].Value = "Ngày tạo";
            worksheet1.Cells["G1"].Value = "Ngày xác nhận";
            worksheet1.Cells["H1"].Value = "Ngày giao hàng";
            worksheet1.Cells["I1"].Value = "Ngày nhận";
            worksheet1.Cells["J1"].Value = "Ngày thanh toán";
            worksheet1.Cells["K1"].Value = "Ngày hoàn thành";
            worksheet1.Cells["L1"].Value = "Sửa đổi ngày";
            worksheet1.Cells["M1"].Value = "Sửa đổi ghi chú";
            worksheet1.Cells["N1"].Value = "Miêu tả";
            worksheet1.Cells["O1"].Value = "Đặt hàng trực tuyến";
            worksheet1.Cells["P1"].Value = "điểm sử dụng";
            worksheet1.Cells["Q1"].Value = "Điểm đã sử dụng";
            worksheet1.Cells["R1"].Value = "Số điểm";
            worksheet1.Cells["S1"].Value = "Thành phố";
            worksheet1.Cells["T1"].Value = "Huyện";
            worksheet1.Cells["U1"].Value = "Xã";
            worksheet1.Cells["V1"].Value = "Địa chỉ";
            worksheet1.Cells["W1"].Value = "Tên Sản Phẩm";
            worksheet1.Cells["X1"].Value = "ld_Staff";
            worksheet1.Cells["Y1"].Value = "ld Promotion";
            worksheet1.Cells["Z1"].Value = "Giá sản phẩm";

            int rowIndex1 = 2; // Bắt đầu từ dòng thứ hai sau tiêu 
            var od = await _OrderService.GetAll();
            od.Where(x => x.CreatedDate.Month == DateTime.Now.Month && x.CreatedDate.Year == DateTime.Now.Year);
            foreach (var order in od)
            {
                foreach (var orderdelta in await _OrderDetailService.GetByOrder(order.Id))
                {
                    var product = await _ProductService.GetById(orderdelta.Id_Product);
                    worksheet1.Cells[$"A{rowIndex1}"].Value = order.Code;
                    worksheet1.Cells[$"B{rowIndex1}"].Value = order.Receiver;
                    worksheet1.Cells[$"C{rowIndex1}"].Value = order.Phone;
                    worksheet1.Cells[$"D{rowIndex1}"].Value = order.Email;
                    worksheet1.Cells[$"E{rowIndex1}"].Value = order.Shipfee;
                    worksheet1.Cells[$"F{rowIndex1}"].Value = order.CreatedDate.ToString();
                    worksheet1.Cells[$"G{rowIndex1}"].Value = order.AcceptDate.ToString();
                    worksheet1.Cells[$"H{rowIndex1}"].Value = order.DeliveryDate.ToString();
                    worksheet1.Cells[$"I{rowIndex1}"].Value = order.ReceiveDate.ToString();
                    worksheet1.Cells[$"J{rowIndex1}"].Value = order.CompleteDate.ToString();
                    worksheet1.Cells[$"L{rowIndex1}"].Value = order.ModifiDate.ToString();
                    worksheet1.Cells[$"M{rowIndex1}"].Value = order.ModifiNotes;
                    worksheet1.Cells[$"N{rowIndex1}"].Value = order.Description;
                    worksheet1.Cells[$"O{rowIndex1}"].Value = order.IsOnlineOrder;
                    worksheet1.Cells[$"P{rowIndex1}"].Value = order.IsUsePoint;
                    worksheet1.Cells[$"Q{rowIndex1}"].Value = order.PointUsed;
                    worksheet1.Cells[$"R{rowIndex1}"].Value = order.PointAmount;
                    worksheet1.Cells[$"S{rowIndex1}"].Value = order.City;
                    worksheet1.Cells[$"T{rowIndex1}"].Value = order.District;
                    worksheet1.Cells[$"U{rowIndex1}"].Value = order.Commune;
                    worksheet1.Cells[$"V{rowIndex1}"].Value = order.Address;
                    worksheet1.Cells[$"W{rowIndex1}"].Value = product.Name;
                    worksheet1.Cells[$"X{rowIndex1}"].Value = order.StatusName;
                    worksheet1.Cells[$"Y{rowIndex1}"].Value = order.Id_Promotions;
                    worksheet1.Cells[$"Z{rowIndex1}"].Value = orderdelta.Price;

                    rowIndex1++;



                }
            };
           
            // Return the PDF file as a response

            workbook.Save(@"D:\Study File\DATN\BookShop.Web.Client\wwwroot\exel\demothongke.xlsx");

		}
	}
}
