using BookShop.BLL.ConfigurationModel.UerPromotionModel;
using BookShop.BLL.IService;
using BookShop.DAL.Entities;
using BookShop.DAL.Repositopy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.Service
{
    public class UserPromotionService : IUserPromotionService
    {
        private readonly IRepository<UserPromotion> _userPromotionRepository;
        private readonly IRepository<Promotion> _promotionRepository;
        public UserPromotionService()
        {
            _promotionRepository = new Repository<Promotion>();
            _userPromotionRepository = new Repository<UserPromotion>();
        }
        public async Task<bool> Add(CreateUserPromotionModel model)
        {
            try
            {
                var obj = new UserPromotion()
                {
                    Id_User = model.Id_User,
                    Id_Promotion = model.Id_Promotion,
                    EndDate = model.EndDate,
                    ReduceMax = model.ReduceMax,
                    CreatedDate = DateTime.Now,
                    Status = model.Status,
                };
                await _userPromotionRepository.CreateAsync(obj);
                return true;
            }
            catch (Exception ex) { return false; }
        }

        public async Task<List<UserPromotionViewModel>> GetByUser(int userId)
        {
            var userpromotions = (await _userPromotionRepository.GetAllAsync()).Where(c => c.Id_User == userId);
            var promotions = await _promotionRepository.GetAllAsync();
            var objlist = (from a in userpromotions
                           join b in promotions on a.Id_Promotion equals b.Id
                           select new UserPromotionViewModel()
                           {
                               Id_User = a.Id_User,
                               Id_Promotion = a.Id_Promotion,
                               EndDate = a.EndDate,
                               ReduceMax = a.ReduceMax,
                               CreatedDate = a.CreatedDate,
                               Status = a.Status,
                               Name = b.Name,
                           }).ToList();
            return objlist;
        }

        public async Task<bool> Update(int id)
        {
            try
            {
                var obj = await _userPromotionRepository.GetByIdAsync(id);
                obj.Status = 1;
                await _userPromotionRepository.UpdateAsync(id, obj);
                return true;
            }
            catch (Exception ex) { return false; }
        }
    }
}
