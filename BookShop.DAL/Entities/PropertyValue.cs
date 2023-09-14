using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DAL.Entities
{
	public class PropertyValue
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public string Value1 { get; set; }
		public string? Value2 { get; set; }
		public int Status { get; set; }

		//foreign key 
		public int Id_Property { get; set; }
		public virtual CustomProperties CustomProperties { get; set; }
	}
}
