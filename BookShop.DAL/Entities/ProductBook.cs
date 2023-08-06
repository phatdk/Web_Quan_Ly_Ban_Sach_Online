﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DAL.Entities
{
	public class ProductBook
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public int Status { get; set; }
		
		//foreign key
		public int Id_Product { get; set; }
		public int Id_Book { get; set; }
		public Product Product { get; set; }
		public Book Book { get; set; }
	}
}
