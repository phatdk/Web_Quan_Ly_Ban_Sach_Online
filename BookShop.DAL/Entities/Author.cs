using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DAL.Entities
{
	public class Author
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public string Name { get; set; }
		public string? Img { get; set; }
		public int Index { get; set; }
		public DateTime CreatedDate { get; set; }
		public int Status { get; set; }

		//reference
		public virtual List<BookAuthor> BookAuthors { get; set; }
	}
}
