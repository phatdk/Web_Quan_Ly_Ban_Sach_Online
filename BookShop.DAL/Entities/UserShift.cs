using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DAL.Entities
{
	public class UserShift
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public DateTime CreatedDate { get; set; }
		public int Status { get; set; }
		public string Note { get; set; }

		public int Id_Shift { get; set; }
		public int Id_User { get; set; }
		public virtual User User { get; set; }
		public virtual WorkShift WorkShift { get; set; }
	}
}
