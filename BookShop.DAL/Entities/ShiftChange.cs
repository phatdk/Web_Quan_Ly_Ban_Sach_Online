using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DAL.Entities
{
	public class ShiftChange
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public string Code { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime? EndTime { get; set; }
		public int? IntialAmount { get; set; }
		public int? TotalMoneyEarn { get; set; }
		public int? TotalCash { get; set; }
		public int? TotalCredit { get; set; }
		public int? CostIncurred { get; set; }
		public string? IncurredNote { get; set; }
		public int? TotalCashPreShift { get; set; }
		public DateTime? ResetTime { get; set; }
		public int? TotalWithDrawn { get; set; }
		public string? Note { get; set; }
		public DateTime CreatedDate { get; set; }
		public int Status { get; set; }

		public int? Id_UserReset { get; set; }
		public int? Id_UserInShift { get; set; }
		public int? Id_UserNxShift { get; set; }
		public int Id_Shift { get; set; }
		public virtual Userr? UserReset { get; set; }
		public virtual Userr? UserIn { get; set; }
		public virtual Userr? UserNx { get; set; }
		public virtual WorkShift WorkShift { get; set; }
	}
}
