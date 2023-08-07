using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DAL.Entities
{
	public class Book
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public string? ISBN { get; set; }
		public string Title { get; set; }
		public string? Description { get; set; }
		public string Reader { get; set; }
		public DateTime CreatedDate { get; set; }
		public int Status { get; set; }

		//foreign key
		public int Id_Collection { get; set; }
		public virtual CollectionBook CollectionBook { get; set; }

		//reference
		public virtual List<BookDetail> BookDetails { get; set; }
		public virtual List<BookAuthor> BookAuthors { get; set; }
		public virtual List<BookGenre> BookGenres { get; set; }
	}
}
