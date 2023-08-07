using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DAL.Entities
{
	public class Genre
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[MaxLength(50)]
		public string Name { get; set; }
		public DateTime CreatedDate { get; set; }
		public int Index { get; set; }
		
		//foreign key
		public int Id_Category { get; set; }
		public virtual Category Category { get; set; }

		//reference
		public virtual List<BookGenre> BookGenres { get; set; }
	}
}
