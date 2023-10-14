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
		 Task<List<StatusViewModel>> GetAll();
		Task<StatusOrder> CreateAsync(StatusOrder model);

    }
}
