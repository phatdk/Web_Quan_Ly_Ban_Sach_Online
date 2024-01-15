using BookShop.BLL.ConfigurationModel.GenreModel;
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
		public Task<bool> Add(CreateUserModel requet);
		public Task<bool> Delete(int id);
		public Task<bool> Update(int id, UpdateUserModel requet);
		public Task<List<UserModel>> GetAll();
		public Task<UserModel> GetById(int id);
		public Task<UserModel> GetLog(string userNameOrEmail);
	}
}
