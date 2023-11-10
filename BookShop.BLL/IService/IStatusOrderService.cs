using BookShop.BLL.ConfigurationModel.StatusOrderModel;
using BookShop.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.IService
{
	public interface IStatusOrderService
	{
		public Task<bool> Add(StatusOrder model);
		public Task<List<StatusViewModel>> GetAll();
	}
}
