using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DAL.Entities
{
	public class CustomProperties
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public string propertyName { get; set; }
		
		//foreign key
		public int Id_Shop { get; set; }
		public virtual Shop Shop { get; set; }

		//reference
		public virtual List<PropertyValue> PropertyValues { get; set; }
	}
}
