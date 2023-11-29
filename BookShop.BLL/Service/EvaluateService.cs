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
using Microsoft.EntityFrameworkCore;
namespace BookShop.BLL.Service
{
    public class EvaluateService : IEvaluateService
    {
        private readonly IRepository<Evaluate> _evaluateRepository;
        private readonly IRepository<Userr> _userRepository;
        private readonly IRepository<Order> _OrDer;
        private readonly IRepository<OrderDetail> _OrDerDetails;

        public EvaluateService()
        {
            _evaluateRepository = new Repository<Evaluate>();
            _userRepository = new Repository<Userr>();
            _OrDer = new Repository<Order>();
            _OrDerDetails = new Repository<OrderDetail>();
        }
        public List<EvaluateViewModel> GetNestedComments(int parentId)
        {
            List<EvaluateViewModel> result = new List<EvaluateViewModel>();
            GetNestedCommentsRecursive(parentId, result);
            return result;
        }
        public async Task<bool> CheckUserExitsBill(int userId, int productId)
        {
            //var hasBillForUser = await (await _OrDer.GetAllAsync())
            //    .Include(x => x._Billdetails)
            //    .AnyAsync(x => x.IdUser == userId && x._Billdetails.Any(d => d.IdProduct == productId));


            var hasBillForUser = from x in await _OrDer.GetAllAsync()
                                 join d in await _OrDerDetails.GetAllAsync()
                                 on x.Id equals d.Id_Order
                                 where x.Id_User == userId && d.Id_Product == productId
                                 group d by x.Id_User into userGroup
                                 select new
                                 {
                                     IdUser = userGroup.Key,
                                     OrderDetails = userGroup.ToList()
                                 };
            return hasBillForUser.ToList().Any(x=>x.IdUser == userId&& x.OrderDetails.Any(x=>x.Id_Product==productId));
        }
        private async void GetNestedCommentsRecursive(int parentId, List<EvaluateViewModel> result)
        {
            var comments = (await _evaluateRepository.GetAllAsync())
                .Where(c => c.Id_Parents == parentId)
                .ToList();

            foreach (var comment in comments)
            {
                result.Add(new EvaluateViewModel()
                {
                    Point = comment.Point,
                    Content = comment.Content,
                    CreatedDate = comment.CreatedDate,
                    Id_Product = comment.Id_Product,
                    Id_User = comment.Id_User,
                    Id_Parents = comment.Id_Parents,
                });
                // Gọi đệ quy để lấy các comment con của comment hiện tại
                GetNestedCommentsRecursive(comment.Id, result);
            }
        }
        public async Task<bool> Add(CreateEvaluateModel model)
        {
           
            try
            {
                var obj = new Evaluate()
                {
                    Point = model.Point,
                    Content = model.Content,
                    CreatedDate = DateTime.Now,
                    Id_Product = model.Id_Product,
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
                               Id_Product = a.Id_Product,
                               Id_User = a.Id_User,
                               Id_Parents = a.Id_Parents,
                           }).ToList();
            return objlist;
        }

        public async Task<List<EvaluateViewModel>> GetByBook(int bookId)
        {
            var evaluates = (await _evaluateRepository.GetAllAsync()).Where(c => c.Id_Product == bookId && c.Id_Parents == null);
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
                               Id_Product = a.Id_Product,
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
                               Id_Product = a.Id_Product,
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
                               Id_Product = a.Id_Product,
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
