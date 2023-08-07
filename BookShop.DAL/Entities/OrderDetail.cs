using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DAL.Entities
{
	public class OrderDetail
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public int Quantity { get; set; }
		public int Price { get; set; }

		//foreign key
		public int Id_Order { get; set; }
		public int Id_Product { get; set; }
		public virtual Order Order { get; set; }
		public virtual Product Product { get; set; }

		//reference
		public virtual List<Evaluate> Evaluates { get; set; }
	}
}
