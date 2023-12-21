using BookShop.BLL.ConfigurationModel.OrderPromotionModel;
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
	public class OrderPromotionService : IOrderPromotionService
	{
		public IRepository<OrderPromotion> _repository;
		public IRepository<Promotion> _promotionRepository;
		public OrderPromotionService()
		{
			_repository = new Repository<OrderPromotion>();
			_promotionRepository = new Repository<Promotion>();
		}
		public async Task<bool> Add(OrderPromotionViewModel model)
		{
			try
			{
				var obj = new OrderPromotion()
				{
					Id_Order = model.Id_Order,
					Id_Promotion = model.Id_Promotion,
				};
				await _repository.CreateAsync(obj);
				return true;
			}
			catch (Exception ex) { return false; }
		}

		public async Task<bool> Delete(int id)
		{
			try
			{
				await _repository.RemoveAsync(id);
				return true;
			}
			catch (Exception ex) { return false; }
		}

		public async Task<List<OrderPromotionViewModel>> GetAll()
		{
			var op = await _repository.GetAllAsync();
			var promotion = await _promotionRepository.GetAllAsync();
			var objlist = (from a in op
						   join b in promotion on a.Id_Promotion equals b.Id
						   select new OrderPromotionViewModel()
						   {
							   Id = a.Id,
							   Id_Order = a.Id_Order,
							   Id_Promotion = a.Id_Promotion,
							   NamePromotion = b.Name,
							   AmountReduct = b.AmountReduct,
							   PercentReduct = b.PercentReduct,
							   ReductMax = b.ReductMax,
						   }).ToList();
			return objlist;
		}

		public async Task<OrderPromotionViewModel> GetById(int id)
		{
			var op = (await _repository.GetAllAsync()).Where(x=>x.Id == id);
			var promotion = await _promotionRepository.GetAllAsync();
			var objlist = (from a in op
						   join b in promotion on a.Id_Promotion equals b.Id
						   select new OrderPromotionViewModel()
						   {
							   Id = a.Id,
							   Id_Order = a.Id_Order,
							   Id_Promotion = a.Id_Promotion,
							   NamePromotion = b.Name,
							   AmountReduct = b.AmountReduct,
							   PercentReduct = b.PercentReduct,
							   ReductMax = b.ReductMax,
						   }).FirstOrDefault();
			return objlist;
		}

		public async Task<List<OrderPromotionViewModel>> GetByOrder(int orderId)
		{
			var op = (await _repository.GetAllAsync()).Where(x=>x.Id_Order == orderId);
			var promotion = await _promotionRepository.GetAllAsync();
			var objlist = (from a in op
						   join b in promotion on a.Id_Promotion equals b.Id
						   select new OrderPromotionViewModel()
						   {
							   Id = a.Id,
							   Id_Order = a.Id_Order,
							   Id_Promotion = a.Id_Promotion,
							   NamePromotion = b.Name,
							   AmountReduct = b.AmountReduct,
							   PercentReduct = b.PercentReduct,
							   ReductMax = b.ReductMax,
						   }).ToList();
			return objlist;
		}

		public async Task<bool> Update(OrderPromotionViewModel model)
		{
			try
			{
				var obj = await _repository.GetByIdAsync(model.Id);
				obj.Id_Promotion = model.Id_Promotion;
				obj.Id_Order = model.Id_Order;
				await _repository.UpdateAsync(model.Id, obj);
				return true;
			}
			catch (Exception ex) { return false; }
		}
	}
}
