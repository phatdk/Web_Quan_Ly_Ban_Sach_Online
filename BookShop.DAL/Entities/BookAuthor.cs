using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DAL.Entities
{
	public class BookAuthor
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		//foreign key
		public int Id_Book { get; set; }
		public int Id_Author { get; set; }
		public virtual Book Book { get; set; }
		public virtual Author Author { get; set; }
	}
}
