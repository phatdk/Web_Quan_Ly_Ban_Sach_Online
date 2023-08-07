using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DAL.Entities
{
	public class CollectionBook
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[MaxLength(50)]
		public string Name { get; set; }
		public DateTime CreatedDate { get; set; }
		public int Status { get; set; }

		//reference 
		public virtual List<Book> Books { get; set; }
	}
}
