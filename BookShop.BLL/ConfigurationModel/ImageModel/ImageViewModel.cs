using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.ConfigurationModel.ImageModel
{
    public class ImageViewModel
    {
        public int Id { get; set; }
        public string? ImageUrl { get; set; }
        public int Index { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? Status { get; set; }

        //foreign key
        public int? Id_Product { get; set; }
    }
}
