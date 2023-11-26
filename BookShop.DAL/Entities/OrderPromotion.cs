using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DAL.Entities
{
	public class OrderPromotion
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		// foreign key
		public int Id_Order { get; set; }
		public int Id_Promotion { get; set; }
		public virtual Order Order { get; set; }
		public virtual Promotion Promotion { get; set; }
	}
}
