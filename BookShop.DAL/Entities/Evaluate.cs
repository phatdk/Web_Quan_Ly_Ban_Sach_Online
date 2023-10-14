using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DAL.Entities
{
	public class Evaluate
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public int? Point { get; set; }
		public string? Content { get; set; }
		public DateTime CreatedDate { get; set; }

		//foreign key
		public int? Id_Parents { get; set; }
		public int Id_User { get; set; }
		public int Id_Book { get; set; }
		public virtual Evaluate Parents { get; set; }
		public virtual Userr User { get; set; }
		public virtual OrderDetail OrderDetail { get; set; }

		//reference
		public virtual List<Evaluate> Evaluates { get; set; }
	}
}
