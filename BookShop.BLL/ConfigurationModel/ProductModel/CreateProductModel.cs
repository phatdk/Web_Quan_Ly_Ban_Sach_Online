﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.ConfigurationModel.ProductModel
{
	public class CreateProductModel
	{
		public string Name { get; set; }
		public int Quantity { get; set; }
		public int Price { get; set; }
		public string? Description { get; set; }
		public int Status { get; set; }
		public int Type { get; set; }
	}
}