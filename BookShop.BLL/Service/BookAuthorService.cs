using BookShop.BLL.IService;
using BookShop.BookShop.BLL.ConfigurationModel.BookAuthorModel;
using BookShop.DAL.Entities;
using BookShop.DAL.Repositopy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.Service
{
    public class BookAuthorService : IBookAuthorService
    {
        protected readonly IRepository<BookAuthorModel> _repository;
        public BookAuthorService()
        {
            _repository = new Repository<BookAuthorModel>();
        }
        public async Task<List<BookAuthorModel>> GetAllBooksAuthor()
        {
            return await _repository.GetAllAsync();
        }


    }
}
