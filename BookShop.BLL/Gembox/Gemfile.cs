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

		public Gen()
		{
			_OrderService = new OrderService();
			_OrderDetailService = new OrderDetailService();
		}
		public async void xuatExel()
		{

			SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");


			var workbook = new ExcelFile();
			var worksheet = workbook.Worksheets.Add("OrderDetails");


			// Đặt tiêu đề cho các cột
			worksheet.Cells["A1"].Value = "Code";
			worksheet.Cells["B1"].Value = "Receiver";
			worksheet.Cells["C1"].Value = "PhoneNumt_erQ";
			worksheet.Cells["D1"].Value = "Email";
			worksheet.Cells["E1"].Value = "Shipping_Fee";
			worksheet.Cells["F1"].Value = "CreateDate";
			worksheet.Cells["G1"].Value = "AcceptDate";
			worksheet.Cells["H1"].Value = "DeliveryDate";
			worksheet.Cells["I1"].Value = "ReceiveDate";
			worksheet.Cells["J1"].Value = "PaymentDate";
			worksheet.Cells["K1"].Value = "CompleteDate";
			worksheet.Cells["L1"].Value = "Modify Date";
			worksheet.Cells["M1"].Value = "Modify Notes";
			worksheet.Cells["N1"].Value = "Description";
			worksheet.Cells["O1"].Value = "Isonlineorder";
			worksheet.Cells["P1"].Value = "lsLJsePoint";
			worksheet.Cells["Q1"].Value = "PointUsed";
			worksheet.Cells["R1"].Value = "PointAmount";
			worksheet.Cells["S1"].Value = "City";
			worksheet.Cells["T1"].Value = "District";
			worksheet.Cells["U1"].Value = "Commune";
			worksheet.Cells["V1"].Value = "Address";
			worksheet.Cells["W1"].Value = "ld User";
			worksheet.Cells["X1"].Value = "ld_Staff";
			worksheet.Cells["Y1"].Value = "ld Promotion";
			worksheet.Cells["Z1"].Value = "ld_Status";

			int rowIndex = 2; // Bắt đầu từ dòng thứ hai sau tiêu đề

			foreach (var order in await _OrderService.GetAll())
			{
				worksheet.Cells[$"A{rowIndex}"].Value = order.Code;
				worksheet.Cells[$"B{rowIndex}"].Value = order.Receiver;
				worksheet.Cells[$"C{rowIndex}"].Value = order.Phone;
				//worksheet.Cells[$"D{rowIndex}"].Value = order.E;
				//worksheet.Cells[$"E{rowIndex}"].Value = order.Shipfee;
				worksheet.Cells[$"F{rowIndex}"].Value = order.CreatedDate;
				worksheet.Cells[$"G{rowIndex}"].Value = order.AcceptDate;
				worksheet.Cells[$"H{rowIndex}"].Value = order.DeliveryDate;
				worksheet.Cells[$"I{rowIndex}"].Value = order.ReceiveDate;
				worksheet.Cells[$"J{rowIndex}"].Value = order.PaymentDate;
				worksheet.Cells[$"K{rowIndex}"].Value = order.CompleteDate;
				worksheet.Cells[$"L{rowIndex}"].Value = order.ModifiDate;
				worksheet.Cells[$"M{rowIndex}"].Value = order.ModifiNotes;
				worksheet.Cells[$"N{rowIndex}"].Value = order.Description;
				//worksheet.Cells[$"O{rowIndex}"].Value = order.IsOnlineOrder;
				//worksheet.Cells[$"P{rowIndex}"].Value = order.IsUsePoint;
				//worksheet.Cells[$"Q{rowIndex}"].Value = order.PointUsed;
				//worksheet.Cells[$"R{rowIndex}"].Value = order.PointAmount;
				worksheet.Cells[$"S{rowIndex}"].Value = order.City;
				worksheet.Cells[$"T{rowIndex}"].Value = order.District;
				worksheet.Cells[$"U{rowIndex}"].Value = order.Commune;
				//worksheet.Cells[$"V{rowIndex}"].Value = order.;
				worksheet.Cells[$"W{rowIndex}"].Value = order.Id_User;
				//worksheet.Cells[$"X{rowIndex}"].Value = order.Id_Staff;
				//worksheet.Cells[$"Z{rowIndex}"].Value = order.Id_Status;

				rowIndex++;


			};

			// Save the document as a PDF
			//var pdfFilePath = @"wwwroot\PDF\XinheMau.pdf";
			//workbook.Save(pdfFilePath);

			// Return the PDF file as a response
			workbook.Save(@"wwwroot\exel\Spreadsheet.xlsx");

		}
	}
}
