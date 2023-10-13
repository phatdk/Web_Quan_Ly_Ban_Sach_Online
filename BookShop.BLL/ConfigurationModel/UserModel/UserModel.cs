using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.ConfigurationModel.UserModel
{
	public class UserModel
	{
		public int Id { get; set; }
		public string Code { get; set; }
		public string? Img { get; set; }
		public string Name { get; set; }
		public DateTime? Birth { get; set; }
		public int? Gender { get; set; }
		public string? Email { get; set; }
		public string? Phone { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public DateTime CreatedDate { get; set; }
		public int Status { get; set; }
	}
}
