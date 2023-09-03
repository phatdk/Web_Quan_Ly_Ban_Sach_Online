using BookShop.BookShop.BLL.ConfigurationModel.BookAuthorModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.IService
{
    public interface IBookAuthorService
    {
        public Task<List<BookAuthorModel>> GetAllBooksAuthor();
    }
}
