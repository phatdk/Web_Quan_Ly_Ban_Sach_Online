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
		public Task<Cart> GetByUser(int userId);
		public Task<bool> Add(Cart model);
		public Task<bool> Delete(int userId);
	}
}
