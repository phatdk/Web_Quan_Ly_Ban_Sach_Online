using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.ConfigurationModel.UserModel
{
	public class UpdateUserModel
	{
		
		public string Name { get; set; }
		public DateTime? Birth { get; set; }
		public int? Gender { get; set; }
		public string? Email { get; set; }
		public string? Phone { get; set; }
		public string Password { get; set; }
		public int Status { get; set; }
	}
}
