using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DAL.Entities
{
	public class Product
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public string Name { get; set; }	
		public int Quantity { get; set; }
		public int Price { get; set; }
		public DateTime CreatedDate { get; set; }
		public int Status { get; set; }
		
		//refenrence
		public List<Image> Images { get; set; }
		public List<ProductBook> ProductBooks { get; set; }
		public List<ProductPromotion> ProductPromotions { get; set; }
		public List<WishList> WishLists { get; set; }
		public List<CartDetail> CartDetails { get; set; }
		public List<OrderDetail> OrderDetails { get; set; }
	}
}
