using BookShop.BLL.ConfigurationModel.CartDetailModel;
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
    public class CartDetailService : ICartDetailService
    {
        private readonly IRepository<CartDetail> _cartRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Image> _imageRepository;
        public CartDetailService()
        {
            _cartRepository = new Repository<CartDetail>();
            _productRepository = new Repository<Product>();
            _imageRepository = new Repository<Image>();
        }
        public async Task<bool> Add(CreateCartDetailModel model)
        {
            try
            {
                var obj = new CartDetail()
                {
                    Id_Product = model.Id_Product,
                    Id_User = model.Id_User,
                    CreatedDate = DateTime.Now,
                    Quantity = model.Quantity,
                };
                await _cartRepository.CreateAsync(obj);
                return true;
            }
            catch (Exception ex) { return false; }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                await _cartRepository.RemoveAsync(id);
                return true;
            }
            catch (Exception ex) { return false; }
        }

        public async Task<bool> DeleteByCart(int userId)
        {
            try
            {
                var cds = (await _cartRepository.GetAllAsync()).Where(x => x.Id_User == userId);
                foreach (var item in cds)
                {
                    await _cartRepository.RemoveAsync(item.Id);
                }
                return true;
            }
            catch (Exception ex) { return false; }
        }

        public async Task<List<CartDetailViewModel>> GetByCart(int userId)
        {
            var carts = (await _cartRepository.GetAllAsync()).Where(c => c.Id_User == userId);
            var products = await _productRepository.GetAllAsync();
            var images = await _imageRepository.GetAllAsync();
            var objlist = (from a in carts
                           join b in products on a.Id_Product equals b.Id
                           join c in images on b.Id equals c.Id_Product into imageGroup
                           select new CartDetailViewModel()
                           {
                               Id = a.Id,
                               Id_Product = a.Id_Product,
                               Id_User = a.Id_User,
                               CreatedDate = a.CreatedDate,
                               Quantity = a.Quantity,
                               ProductName = b.Name,
                               ProductPrice = b.Price,
                               TotalPrice = a.Quantity * b.Price,
                               ImgProductCartDetail = imageGroup.FirstOrDefault()?.ImageUrl,
                               SoLuongKho = b.Quantity,
                               Status = b.Quantity > 0 && b.Status == 1 ? 1 : 0,
                           }).ToList();
            return objlist;
        }

        public async Task<CartDetailViewModel> GetById(int id)
        {
            var obj = await _cartRepository.GetByIdAsync(id);
            return new CartDetailViewModel()
            {
                Id = obj.Id,
                Id_Product = obj.Id_Product,
                Id_User = obj.Id_User,
                CreatedDate = obj.CreatedDate,
                Quantity = obj.Quantity,
            };
        }

        public async Task<bool> Update(int id, UpdateCartDetailModel model)
        {
            try
            {
                var obj = await _cartRepository.GetByIdAsync(id);
                obj.Quantity = model.Quantity;
                await _cartRepository.UpdateAsync(id, obj);
                return true;
            }
            catch (Exception ex) { return false; }
        }
    }
}
