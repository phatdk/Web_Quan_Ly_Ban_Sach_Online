using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DAL.Entities
{
	public class Shop
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public string ShopName { get; set; }
		public string About { get; set; }

		//reference
		public virtual List<CustomProperties> CustomProperties { get; set; }
	}
}
