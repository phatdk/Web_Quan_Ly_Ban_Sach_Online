using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DAL.Entities
{
	public class Promotion
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
		public int Condition { get; set; }
		public int AmountReduct { get; set; }
		public int PercentReduct { get; set; }
		public int ReductMax { get; set; }
		public int Quantity { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public string? Description { get; set; }
		public int Status { get; set; }

		//foreign key
		public int Id_Type { get; set; }
		public virtual PromotionType PromotionType { get; set; }

		//reference
		public virtual List<Order> Orders { get; set; }
		public virtual List<ProductPromotion> ProductPromotions { get; set; }
		public virtual List<UserPromotion> UserPromotions { get; set; }	
		public virtual List<PointTransactionsHistory> PointTransactionsHistories { get; set;}
	}
}
