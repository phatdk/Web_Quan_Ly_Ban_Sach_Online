using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.ConfigurationModel.NewsModel
{
	public class CreateNewsModel
	{
		public string Title { get; set; }
		public string Content { get; set; }
		public string? Description { get; set; }
		public int Status { get; set; }
		public string Img { get; set; }
		public DateTime DateTime { get; set; }
	}
}
