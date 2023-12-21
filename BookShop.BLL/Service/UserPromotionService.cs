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
                    CreatedDate = DateTime.Now,
                    Status = model.Status,
                };
                await _userPromotionRepository.CreateAsync(obj);
                return true;
            }
            catch (Exception ex) { return false; }
        }

		public async Task<UserPromotionViewModel> GetById(int userId, int promotionId)
		{
			var userpromotions = (await _userPromotionRepository.GetAllAsync()).Where(c => c.Id_User == userId && c.Id_Promotion == promotionId);
			var promotions = await _promotionRepository.GetAllAsync();
			var objlist = (from a in userpromotions
						   join b in promotions on a.Id_Promotion equals b.Id into t
						   from b1 in t.DefaultIfEmpty()
						   select new UserPromotionViewModel()
						   {
							   Id = a.Id,
							   Id_User = a.Id_User,
							   Id_Promotion = a.Id_Promotion,
							   EndDate = a.EndDate,
							   CreatedDate = a.CreatedDate,
							   Status = a.Status,
							   StorageTerm = b1 == null ? 0 : Convert.ToInt32(b1.StorageTerm),
							   Name = b1.Name,
							   Code = b1.Code,
						   }).FirstOrDefault();
			return objlist;
		}

		public async Task<List<UserPromotionViewModel>> GetByUser(int userId)
        {
            var userpromotions = (await _userPromotionRepository.GetAllAsync()).Where(c => c.Id_User == userId);
            var promotions = await _promotionRepository.GetAllAsync();
            var objlist = (from a in userpromotions
                           join b in promotions on a.Id_Promotion equals b.Id into t
                           from b1 in t.DefaultIfEmpty()
                           select new UserPromotionViewModel()
                           {
                               Id = a.Id,
                               Id_User = a.Id_User,
                               Id_Promotion = a.Id_Promotion,
                               EndDate = a.EndDate,
                               CreatedDate = a.CreatedDate,
                               Status = a.Status,
                               StorageTerm = b1 == null? 0 : Convert.ToInt32(b1.StorageTerm),
                               Name = b1.Name,
                               Code = b1.Code,
                           }).ToList();
            return objlist;
        }

        public async Task<bool> Update(UserPromotionViewModel model)
        {
            try
            {
                var obj = await _userPromotionRepository.GetByIdAsync(model.Id);
                obj.Status = model.Status;
                await _userPromotionRepository.UpdateAsync(model.Id, obj);
                return true;
            }
            catch (Exception ex) { return false; }
        }
    }
}
