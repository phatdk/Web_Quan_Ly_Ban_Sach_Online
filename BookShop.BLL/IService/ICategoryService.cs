using BookShop.BLL.ConfigurationModel.CategoryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.IService
{
    public interface ICategoryService
    {

        public Task<bool> Add(CreateCategoryModel requet);
        public Task<bool> Delete(int id);
        public Task<bool> Update(int id, UpdateCategoryModel requet);
        public Task<List<CategoryModel>> GetAll();
        public Task<CategoryModel> GetById(int id);


    }
}
