using BookShop.BLL.ConfigurationModel.ProductPromotionModel;
using BookShop.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.IService
{
    public interface IProductPromotionService
    {
        public Task<List<ProductPromotionViewModel>> GetByProduct(int productId);
        public Task<List<ProductPromotionViewModel>> GetByPromotion(int promorionId);
        public Task<ProductPromotionViewModel> GetById(int id);
        public Task<bool> Add(ProductPromotion model);
        public Task<bool> Update(int id, UpdateProductPromotionModel model);
        public Task<bool> Delete(int id);
        //thêm
        public Task<bool> AddProductToPromotions(int productId, List<int> promotionId, ProductPromotion model);//1sp -n km
        public Task<bool> AddPromotionToProducts(int promotionId, List<int> productId, ProductPromotion model);//1km - n sp
    }
}
