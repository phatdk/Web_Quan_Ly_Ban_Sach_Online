﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DAL.Entities
{
	public class Order
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public string Code { get; set; }
		public string Receiver { get; set; }
		public string Phone { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime? AcceptDate { get; set; }
		public DateTime? DeliveryDate { get; set; }
		public DateTime? ReceiveDate { get; set; }
		public DateTime? PaymentDate { get; set; }
		public DateTime? CompleteDate { get; set; }
		public DateTime? ModifiDate { get; set; }
		public string? ModifiNotes { get; set; }
		public string? Description { get; set; }
		public int Status { get; set; }
		public int City { get; set; }
		public int District { get; set; }
		public int Commune { get; set; }

		//foreign key
		public int Id_User { get; set; }
		public User User { get; set; }

		//reference
		public List<OrderDetail> OrderDetails { get; set; }
		public List<OrderPayment> OrderPayments { get; set; }
		public List<ReturnOrder> ReturnOrders { get; set; }
	}
}
