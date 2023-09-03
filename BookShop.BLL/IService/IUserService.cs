using BookShop.BLL.ConfigurationModel.BookGenreCategoryModel;
using BookShop.BLL.ConfigurationModel.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.IService
{
	public interface IUserService
	{
		public Task<bool> add(CreateUserModel requet);
		public Task<bool> remove(int id);
		public Task<bool> update(int id, UpdateUserModel requet);
		public Task<List<UserModel>> Getall();
		public Task<UserModel> GetbyId(int id);
	}
}
