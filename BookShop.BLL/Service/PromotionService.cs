using BookShop.BLL.ConfigurationModel.PromotionModel;
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
    public class PromotionService : IPromotionService
    {
        private readonly IRepository<Promotion> _promotionRepository;
        private readonly IRepository<PromotionType> _typeRepository;
        public PromotionService()
        {
            _promotionRepository = new Repository<Promotion>();
            _typeRepository = new Repository<PromotionType>();
        }

        public async Task<string> GenerateCode(int length)
        {
            // Khởi tạo đối tượng Random
            Random random = new Random();

            // Tạo một chuỗi các ký tự ngẫu nhiên
            string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            string code = "";
            for (int i = 0; i < length; i++)
            {
                code += characters[random.Next(characters.Length)];
            }
            var duplicate = (await _promotionRepository.GetAllAsync()).Where(c => c.Code.Equals(code));
            if (!duplicate.Any())
            {
                return code;
            }
            return GenerateCode(length).ToString();
        }

        public async Task<bool> Add(CreatePromotionModel model)
        {
            try
            {
                if (model.Code == null)
                {
                    model.Code = await GenerateCode(13);
                }
                var obj = new Promotion()
                {
                    Name = model.Name,
                    Code = model.Code,
                    Condition = model.Condition,
                    StorageTerm = model.StorageTerm,
                    AmountReduct = model.AmountReduct,
                    PercentReduct = model.PercentReduct,
                    ReductMax = model.ReductMax,
                    Quantity = model.Quantity,
                    CreatedDate = DateTime.Now,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    Description = model.Description,
                    Status = model.Status,
                    Id_Type = model.Id_Type,
                };
                await _promotionRepository.CreateAsync(obj);
                return true;
            }
            catch (Exception ex) { return false; }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                await _promotionRepository.RemoveAsync(id);
                return true;
            }
            catch (Exception ex) { return false; }
        }

        public async Task<List<PromotionViewModel>> GetAll()
        {
            var promotions = await _promotionRepository.GetAllAsync();
            var types = await _typeRepository.GetAllAsync();
            var objlist = (from a in promotions
                           join b in types on a.Id_Type equals b.Id
                           select new PromotionViewModel()
                           {
                               Id = a.Id,
                               Name = a.Name,
                               Code = a.Code,
                               Condition = a.Condition,
                               StorageTerm = a.StorageTerm,
                               PercentReduct = a.PercentReduct,
                               AmountReduct = a.AmountReduct,
                               ReductMax = a.ReductMax,
                               Quantity = a.Quantity,
                               CreatedDate = a.CreatedDate,
                               StartDate = a.StartDate,
                               EndDate = a.EndDate,
                               Description = a.Description,
                               Status = a.Status,
                               Id_Type = a.Id_Type,
                               NameType = b.Name,
                           }).ToList();
            return objlist;
        }

        public async Task<PromotionViewModel> GetByCode(string code)
        {
            var promotions = (await _promotionRepository.GetAllAsync()).Where(c => c.Code.Equals(code));
            var types = await _typeRepository.GetAllAsync();
            var objlist = (from a in promotions
                           join b in types on a.Id_Type equals b.Id
                           select new PromotionViewModel()
                           {
                               Id = a.Id,
                               Name = a.Name,
                               Code = a.Code,
                               Condition = a.Condition,
                               StorageTerm = a.StorageTerm,
                               PercentReduct = a.PercentReduct,
                               AmountReduct = a.AmountReduct,
                               ReductMax = a.ReductMax,
                               Quantity = a.Quantity,
                               CreatedDate = a.CreatedDate,
                               StartDate = a.StartDate,
                               EndDate = a.EndDate,
                               Description = a.Description,
                               Status = a.Status,
                               Id_Type = a.Id_Type,
                               NameType = b.Name,
                           }).FirstOrDefault();
            return objlist;
        }

        public async Task<PromotionViewModel> GetById(int id)
        {
            var promotions = (await _promotionRepository.GetAllAsync()).Where(c => c.Id == id);
            var types = await _typeRepository.GetAllAsync();
            var objlist = (from a in promotions
                           join b in types on a.Id_Type equals b.Id
                           select new PromotionViewModel()
                           {
                               Id = a.Id,
                               Name = a.Name,
                               Code = a.Code,
                               Condition = a.Condition,
                               StorageTerm = a.StorageTerm,
                               PercentReduct = a.PercentReduct,
                               AmountReduct = a.AmountReduct,
                               ReductMax = a.ReductMax,
                               Quantity = a.Quantity,
                               CreatedDate = a.CreatedDate,
                               StartDate = a.StartDate,
                               EndDate = a.EndDate,
                               Description = a.Description,
                               Status = a.Status,
                               Id_Type = a.Id_Type,
                               NameType = b.Name,
                           }).FirstOrDefault();
            return objlist;
        }

        public async Task<List<PromotionViewModel>> GetByStatus(int status)
        {
            var promotions = (await _promotionRepository.GetAllAsync()).Where(c => c.Status == status);
            var types = await _typeRepository.GetAllAsync();
            var objlist = (from a in promotions
                           join b in types on a.Id_Type equals b.Id
                           select new PromotionViewModel()
                           {
                               Id = a.Id,
                               Name = a.Name,
                               Code = a.Code,
                               Condition = a.Condition,
                               StorageTerm = a.StorageTerm,
                               PercentReduct = a.PercentReduct,
                               AmountReduct = a.AmountReduct,
                               ReductMax = a.ReductMax,
                               Quantity = a.Quantity,
                               CreatedDate = a.CreatedDate,
                               StartDate = a.StartDate,
                               EndDate = a.EndDate,
                               Description = a.Description,
                               Status = a.Status,
                               Id_Type = a.Id_Type,
                               NameType = b.Name,
                           }).ToList();
            return objlist;
        }

        public async Task<List<PromotionViewModel>> GetByType(int typeId)
        {
            var promotions = (await _promotionRepository.GetAllAsync()).Where(c => c.Id_Type == typeId);
            var types = await _typeRepository.GetAllAsync();
            var objlist = (from a in promotions
                           join b in types on a.Id_Type equals b.Id
                           select new PromotionViewModel()
                           {
                               Id = a.Id,
                               Name = a.Name,
                               Code = a.Code,
                               Condition = a.Condition,
                               StorageTerm = a.StorageTerm,
                               PercentReduct = a.PercentReduct,
                               AmountReduct = a.AmountReduct,
                               ReductMax = a.ReductMax,
                               Quantity = a.Quantity,
                               CreatedDate = a.CreatedDate,
                               StartDate = a.StartDate,
                               EndDate = a.EndDate,
                               Description = a.Description,
                               Status = a.Status,
                               Id_Type = a.Id_Type,
                               NameType = b.Name,
                           }).ToList();
            return objlist;
        }

        public async Task<bool> Update(int id, UpdatePromotionModel model)
        {
            try
            {
                var obj = await _promotionRepository.GetByIdAsync(id);
                obj.Name = model.Name;
                obj.Code = model.Code;
                obj.Condition = model.Condition;
                obj.StorageTerm = model.StorageTerm;
                obj.AmountReduct = model.AmountReduct;
                obj.PercentReduct = model.PercentReduct;
                obj.ReductMax = model.ReductMax;
                obj.Quantity = model.Quantity;
                obj.StartDate = model.StartDate;
                obj.EndDate = model.EndDate;
                obj.Description = model.Description;
                obj.Status = model.Status;
                obj.Id_Type = model.Id_Type;
                await _promotionRepository.UpdateAsync(id, obj);
                return true;
            }
            catch (Exception ex) { return false; }
        }
    }
}
