using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.ConfigurationModel.GenreModel
{
	public class updateGenreModel
	{
		public string Name { get; set; }
		public int Index { get; set; }

		//foreign key
		public int Id_Category { get; set; }
	}
}
