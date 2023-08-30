using BookShop.BLL.ConfigurationModel.PromotionTypeModel;
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
    public class PromotionTypeService : IPromotionTypeService
    {
        private readonly IRepository<PromotionType> _promotionTypeRepository;
        public PromotionTypeService()
        {
            _promotionTypeRepository = new Repository<PromotionType>();
        }
        public async Task<bool> Add(CreatePromotionTypeModel model)
        {
            try
            {
                var obj = new PromotionType()
                {
                    Name = model.Name,
                    CreatedDate = DateTime.Now,
                    Status = model.Status,
                };
                await _promotionTypeRepository.CreateAsync(obj);
                return true;
            }
            catch (Exception ex) { return false; }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                await _promotionTypeRepository.RemoveAsync(id);
                return true;
            }
            catch (Exception ex) { return false; }
        }

        public async Task<List<PromotionTypeViewModel>> GetAll()
        {
            var list = await _promotionTypeRepository.GetAllAsync();
            var objlist = new List<PromotionTypeViewModel>();
            foreach (var item in list)
            {
                var obj = new PromotionTypeViewModel()
                {
                    Name = item.Name,
                    Status = item.Status,
                    Id = item.Id,
                    CreatedDate = item.CreatedDate,
                };
                objlist.Add(obj);
            }
            return objlist;
        }

        public async Task<PromotionTypeViewModel> GetById(int id)
        {
            var obj = await _promotionTypeRepository.GetByIdAsync(id);
            return new PromotionTypeViewModel()
            {
                Id = obj.Id,
                Name = obj.Name,
                Status = obj.Status,
                CreatedDate = obj.CreatedDate,
            };
        }

        public async Task<bool> Update(int id, UpdatePromotiontypeModel model)
        {
            try
            {
                var obj = await _promotionTypeRepository.GetByIdAsync(id);
                obj.Name = model.Name;
                obj.Status = model.Status;
                await _promotionTypeRepository.UpdateAsync(id, obj);
                return true;
            }
            catch (Exception ex) { return false; }
        }
    }
}
