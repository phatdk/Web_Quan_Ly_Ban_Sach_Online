using BookShop.BLL.ConfigurationModel.CartDetailModel;
using BookShop.DAL.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.IService
{
	public interface ICartService
	{
		public Task<CartViewModel> GetByUser(int userId);
		public Task<bool> Add(CartViewModel model);
		public Task<bool> Delete(int userId);
	}
}
