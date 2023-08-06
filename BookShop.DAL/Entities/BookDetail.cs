using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DAL.Entities
{
	public class BookDetail
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public int Price { get; set; }
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
		public DateTime CreatedDate { get; set;}
		public int Status { get; set; }

		//foreign key
		public int Id_Supplier { get; set; }
		public int Id_Book { get; set; }
		public int Id_Language { get; set; }
		public Supplier Supplier { get; set; }
		public Book Book { get; set; }
		public Language Language { get; set; }

		//reference
		public List<ProductBook> ProductBooks { get; set; }
	}
}
