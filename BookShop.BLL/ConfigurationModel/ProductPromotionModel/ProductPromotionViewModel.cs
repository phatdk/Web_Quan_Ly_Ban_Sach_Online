﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.ConfigurationModel.ProductPromotionModel
{
	public class ProductPromotionViewModel
	{
		public int Id { get; set; }
		public int Index { get; set; }
		public DateTime CreatedDate { get; set; }
		public int Status { get; set; }

		// join properties
		public string NameProduct { get; set; }
		public string NamePromotion { get; set; }

		//foreign key
		public int Id_Product { get; set; }
		public int Id_Promotion { get; set; }
	}
}
