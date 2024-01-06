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
			var worksheet = workbook.Worksheets.Add("Thống kê sản phẩm");

			
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

			// Return the PDF file as a response
			
			workbook.Save(@"D:\Study File\DATN\BookShop.Web.Client\wwwroot\exel\demothongke.xlsx");

		}
	}
}
