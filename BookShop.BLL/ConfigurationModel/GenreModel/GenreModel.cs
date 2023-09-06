using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.ConfigurationModel.BookGenreCategoryModel
{
	public class GenreModel
	{
		public int Id { get; set; }
		
		public string Name { get; set; }
		public DateTime CreatedDate { get; set; }
		public int Index { get; set; }

		//foreign key
		public int Id_Category { get; set; }
	}
}
