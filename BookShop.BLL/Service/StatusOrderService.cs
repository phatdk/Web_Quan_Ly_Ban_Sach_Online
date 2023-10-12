using BookShop.BLL.ConfigurationModel.StatusOrderModel;
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
	public class StatusOrderService : IStatusOrderService
	{
		private readonly IRepository<StatusOrder> _statusRepository;
		public StatusOrderService()
		{
			_statusRepository = new Repository<StatusOrder>();
		}
		public async Task<List<StatusViewModel>> GetAll()
		{
			var listvm = new List<StatusViewModel>();
			var list = await _statusRepository.GetAllAsync();
			foreach (var item in list)
			{
				var obj = new StatusViewModel()
				{
					Id = item.Id,
					StatusName = item.StatusName,
					CreatedDate = item.CreatedDate,
					Status = item.Status,
				};
				listvm.Add(obj);
			}
			return listvm;
		}
		public async Task<StatusOrder> CreateAsync(StatusOrder model)
		{
			return await _statusRepository.CreateAsync(model);

        }
	}
}
