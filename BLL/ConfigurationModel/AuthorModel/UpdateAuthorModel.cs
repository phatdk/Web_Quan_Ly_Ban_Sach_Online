using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ConfigurationModel.AuthorModel
{
	public class UpdateAuthorModel
	{
		public string Name { get; set; }
		public string? Img { get; set; }
		public int Index { get; set; }
		public int Status { get; set; }
	}
}
