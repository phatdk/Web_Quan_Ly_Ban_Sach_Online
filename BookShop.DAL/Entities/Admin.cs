using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DAL.Entities
{
	public class Admin
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public string Name { get; set; }
		public string Phone { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public int Role { get; set; }
		public DateTime CreatedDate { get; set; }
		public int Status { get; set; }
	}
}
