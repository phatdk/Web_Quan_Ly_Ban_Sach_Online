using BookShop.BLL.ConfigurationModel.BookGenreCategoryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.IService
{
    public interface IGenreService
    {
        public Task<bool> add(CreateGenreModel requet);
        public Task<bool> remove(int id);
        public Task<bool> update(int id, updateGenreModel requet);
        public Task<List<GenreModel>> Getall();
        public Task<GenreModel> GetbyId(int id);
    }
}
