using BookShop.BLL.ConfigurationModel.ShopModel;
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
	public class ShopService : IShopService
	{
		private readonly IRepository<Shop> _shopRepository;
		private readonly IRepository<CustomProperties> _customPropertiesRepository;
		private readonly IRepository<PropertyValue> _propertyValueRepository;
		public ShopService()
		{
			_shopRepository = new Repository<Shop>();
			_customPropertiesRepository = new Repository<CustomProperties>();
			_propertyValueRepository = new Repository<PropertyValue>();
		}
		public async Task<List<ShopViewModel>> GetShop()
		{
			var shop = await _shopRepository.GetAllAsync();
			var custom = await _customPropertiesRepository.GetAllAsync();
			var value = await _propertyValueRepository.GetAllAsync();
			var obj = (from a in shop
					   join b in custom on a.Id equals b.Id_Shop
					   join c in value on b.Id equals c.Id_Property
					   select new ShopViewModel()
					   {
						   Id = a.Id,
						   ShopName = a.ShopName,
						   About = a.About,
					   }).ToList();
			return obj;
		}

		public async Task<bool> Update(int id, UpdateShopModel model)
		{
			try
			{
				var obj = await _shopRepository.GetByIdAsync(id);
				obj.ShopName = model.ShopName;
				obj.About = model.About;
				await _shopRepository.UpdateAsync(id, obj);
				return true;
			}
			catch (Exception ex) { return false; }
		}
	}
}
