using BookShop.BLL.ConfigurationModel.EvaluateModel;
using BookShop.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.IService
{
    public interface IEvaluateService
    {
        public Task<List<EvaluateViewModel>> GetAll();
        public Task<List<EvaluateViewModel>> GetByUser(int userId);
        public Task<List<EvaluateViewModel>> GetByBook(int bookId);
        public Task<List<EvaluateViewModel>> GetChild(int parentsId);
        public Task<bool> Add(CreateEvaluateModel model);
        public Task<bool> Update(int id, UpdateEvaluateModel model);
        public Task<bool> Delete(int id);
    }
}
