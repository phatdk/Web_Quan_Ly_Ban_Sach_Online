using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DAL.Entities
{
	public class Cart
	{
		[Key]
		public int Id_User { get; set; }
		public DateTime CreatedDate { get; set; }
		public int Status { get; set; }
		
		//foreign key 
		public virtual User User { get; set; }

		//reference
		public virtual List<CartDetail> CartDetails { get; set; }
	}
}
