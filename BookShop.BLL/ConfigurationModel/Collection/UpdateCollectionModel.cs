﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.ConfigurationModel.Collection
{
	public class UpdateCollectionModel
	{
		public string Name { get; set; }
		
		public int Status { get; set; }
	}
}
