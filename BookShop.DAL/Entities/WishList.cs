﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DAL.Entities
{
	public class WishList
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public DateTime CreatedDate { get; set; }

		//foreign key
		public int Id_User { get; set; }
		public int Id_Product { get; set; }
		public virtual Userr User { get; set; }
		public virtual Product Product { get; set; }
	}
}
