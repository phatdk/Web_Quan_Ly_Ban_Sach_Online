using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.ConfigurationModel.AuthorModel
{
	public class AuthorModel
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		public string? Img { get; set; }
		public int? Index { get; set; }
		public DateTime? CreatedDate { get; set; }
		public int? Status { get; set; }
	}
}
