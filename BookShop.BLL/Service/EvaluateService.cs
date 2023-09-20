using BookShop.BLL.ConfigurationModel.EvaluateModel;
using BookShop.BLL.IService;
using BookShop.DAL.Entities;
using BookShop.DAL.Repositopy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.Service
{
    public class EvaluateService : IEvaluateService
    {
        private readonly IRepository<Evaluate> _evaluateRepository;
        private readonly IRepository<User> _userRepository;

        public EvaluateService()
        {
            _evaluateRepository = new Repository<Evaluate>();
            _userRepository = new Repository<User>();
        }

        public async Task<bool> Add(CreateEvaluateModel model)
        {
            if (model != null) { return false; }
            try
            {
                var obj = new Evaluate()
                {
                    Point = model.Point,
                    Content = model.Content,
                    CreatedDate = DateTime.Now,
                    Id_Book = model.Id_Book,
                    Id_User = model.Id_User,
                    Id_Parents = model.Id_Parents,
                };
                await _evaluateRepository.CreateAsync(obj);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Delete(int id)
        {
            if (id != null) return false;
            try
            {
                await _evaluateRepository.RemoveAsync(id);
                return true;
            }
            catch (Exception ex) { return false; }
        }

        public async Task<List<EvaluateViewModel>> GetAll()
        {
            var evaluates = await _evaluateRepository.GetAllAsync();
            var users = await _userRepository.GetAllAsync();
            var objlist = (from a in evaluates
                           join b in users on a.Id_User equals b.Id into t
                           from b in t.DefaultIfEmpty()                          
                           where a.Id_Parents == null //điều kiện: id_parents = null sẽ lấy đánh giá chính
                           select new EvaluateViewModel()
                           {
                               Id = a.Id,
                               Point = a.Point,
                               Content = a.Content,
                               NameUser = b.Name,
                               Id_Book = a.Id_Book,
                               Id_User = a.Id_User,
                               Id_Parents = a.Id_Parents,
                           }).ToList();
            return objlist;
        }

        public async Task<List<EvaluateViewModel>> GetByBook(int bookId)
        {
            var evaluates = (await _evaluateRepository.GetAllAsync()).Where(c => c.Id_Book == bookId && c.Id_Parents == null);
            var users = await _userRepository.GetAllAsync();
            var objlist = (from a in evaluates
                           join b in users on a.Id_User equals b.Id into t
                           from b in t.DefaultIfEmpty()
                           select new EvaluateViewModel()
                           {
                               Id = a.Id,
                               Point = a.Point,
                               Content = a.Content,
                               NameUser = b.Name,
                               Id_Book = a.Id_Book,
                               Id_User = a.Id_User,
                               Id_Parents = a.Id_Parents,
                           }).ToList();
            return objlist;
        }

        public async Task<List<EvaluateViewModel>> GetByUser(int userId)
        {
            var evaluates = (await _evaluateRepository.GetAllAsync()).Where(c => c.Id_User == userId);
            var users = await _userRepository.GetAllAsync();
            var objlist = (from a in evaluates
                           join b in users on a.Id_User equals b.Id into t
                           from b in t.DefaultIfEmpty()
                           select new EvaluateViewModel()
                           {
                               Id = a.Id,
                               Point = a.Point,
                               Content = a.Content,
                               NameUser = b.Name,
                               Id_Book = a.Id_Book,
                               Id_User = a.Id_User,
                               Id_Parents = a.Id_Parents,
                           }).ToList();
            return objlist;
        }

        public async Task<List<EvaluateViewModel>> GetChild(int parentsId)
        {
            
            var evaluates = (await _evaluateRepository.GetAllAsync()).Where(c => c.Id_Parents == parentsId);
            var users = await _userRepository.GetAllAsync();
            var objlist = (from a in evaluates
                           join b in users on a.Id_User equals b.Id into t
                           from b in t.DefaultIfEmpty()
                           
                           select new EvaluateViewModel()
                           {
                               Id = a.Id,
                               Point = a.Point,
                               Content = a.Content,
                               NameUser = b.Name,
                               Id_Book = a.Id_Book,
                               Id_User = a.Id_User,
                               Id_Parents = a.Id_Parents,
                           }).ToList();
            return objlist;
        }

        public async Task<bool> Update(int id, UpdateEvaluateModel model)
        {
            if (model != null) return false;
            try
            {
                var obj = await _evaluateRepository.GetByIdAsync(id);
                obj.Point = model.Point;
                obj.Content = model.Content;
                await _evaluateRepository.UpdateAsync(id, obj);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
