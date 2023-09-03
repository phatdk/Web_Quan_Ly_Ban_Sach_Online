using BookShop.BLL.ConfigurationModel.BookGenreCategoryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.IService
{
    public interface ICategoryService
    {

        public Task<bool> add(CreateCategoryModel requet);
        public Task<bool> remove(int id);
        public Task<bool> update(int id, UpdateCategoryModel requet);
        public Task<List<CategoryModel>> Getall();
        public Task<CategoryModel> GetbyId(int id);


    }
}
