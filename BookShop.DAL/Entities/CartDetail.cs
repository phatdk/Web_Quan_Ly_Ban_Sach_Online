using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DAL.Entities
{
	public class CartDetail
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public int Quantity { get; set; }
		public DateTime CreatedDate { get; set; }

		//foreign key
		public int Id_User { get; set; }
		public int Id_Product { get; set; }
		public Cart Cart { get; set; }
		public Product Product { get; set; }
	}
}
