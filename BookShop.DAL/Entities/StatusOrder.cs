using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DAL.Entities
{
	public class StatusOrder
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[MaxLength(256)]
		public string StatusName { get; set; }
		public DateTime CreatedDate { get; set; }
		public int Status { get; set; }

		public virtual List<Order> Orders { get; set; }
	}
}
