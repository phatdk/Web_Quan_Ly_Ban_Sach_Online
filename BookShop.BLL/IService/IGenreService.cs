using BookShop.BLL.ConfigurationModel.GenreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.IService
{
    public interface IGenreService
    {
        public Task<bool> Add(CreateGenreModel requet);
        public Task<bool> Delete(int id);
        public Task<bool> Update(int id, updateGenreModel requet);
        public Task<List<GenreModel>> GetAll();
        public Task<List<GenreModel>> GetByCategory(int categoryId);
        public Task<GenreModel> GetById(int id);
    }
}
