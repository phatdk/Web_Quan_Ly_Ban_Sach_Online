using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DAL.Entities
{
	public class WalletPoint
	{
		[Key]
		public int Id_User { get; set; }
		public int Point { get; set; }
		public DateTime CreatedDate { get; set; }
		public int Status { get; set; }

		//foreignkey
		public User User { get; set; }

		//reference
		public virtual List<PointTransactionsHistory> PointTransactionsHistories { get; set; }
	}
}
