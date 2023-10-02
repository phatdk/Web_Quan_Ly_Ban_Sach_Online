using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DAL.Entities
{
	public class WorkShift
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public string Shift { get; set; }
		public string Time { get; set; }
		public DateTime CreatedDate { get; set; }
		public int Status { get; set; }
		public virtual List<UserShift> UserShifts { get; set; }
		public virtual List<ShiftChange> ShiftChanges { get; set; }
	}
}
