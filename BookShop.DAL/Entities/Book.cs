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
		public string Barcode { get; set; }
		public string Img { get; set; }
		public string Title { get; set; }
		public string? Description { get; set; }
		public string Reader { get; set; }
		public int CoverPrice { get; set; }
		public int ImportPrice { get; set; }
		public int Quantity { get; set; }
		public string PageSize { get; set; }
		public int Pages { get; set; }
		public string Cover { get; set; }
		public string PublicationDate { get; set; }
		public int Weight { get; set; }
		public int Widght { get; set; }
		public int Length { get; set; }
		public int Height { get; set; }
		public DateTime CreatedDate { get; set; }
		public int Status { get; set; }

		//foreign key
		public int Id_Supplier { get; set; }
		public virtual Supplier Supplier { get; set; }

		//reference
		public virtual List<BookAuthor> BookAuthors { get; set; }
		public virtual List<BookGenre> BookGenres { get; set; }
		public virtual List<ProductBook> ProductBooks { get; set; }
	}
}
