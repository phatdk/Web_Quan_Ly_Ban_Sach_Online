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
		public string? Description { get; set; }
		public DateTime CreatedDate { get; set; }
		public int Status { get; set; }
		public int Type { get; set; } // 0 là bán lẻ, 1 là bán combo
		// foreign key
		public int? Id_Collection { get; set; }
		public virtual CollectionBook? CollectionBook { get; set; }
		//thêm 
		public int TotalQuantity { get; set; }

        //refenrence

        public virtual List<Image> Images { get; set; }
		public virtual List<ProductBook> ProductBooks { get; set; }
		public virtual List<ProductPromotion> ProductPromotions { get; set; }
		public virtual List<WishList> WishLists { get; set; }
		public virtual List<CartDetail> CartDetails { get; set; }
		public virtual List<OrderDetail> OrderDetails { get; set; }
	}
}
