using BookShop.BLL.ConfigurationModel.AuthorModel;
using BookShop.BLL.ConfigurationModel.UserModel;
using BookShop.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.IService
{
    public interface IAuthorService
    {
        public Task<List<AuthorModel>> Getall();
        public Task<AuthorModel> GetById(int id);
        public Task<bool> Update(int id, AuthorModel requet);
        public Task<bool> Delete(int id);
        public Task<bool> Add(CreateAuthorModel requet);
    }
}
