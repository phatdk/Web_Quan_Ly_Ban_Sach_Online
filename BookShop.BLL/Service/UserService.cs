using BookShop.BLL.ConfigurationModel.UserModel;
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
	public class UserService : IUserService
	{
		protected readonly IRepository<User> _Userrepository;
		protected readonly IRepository<Cart> _CartRepository;
		protected readonly IRepository<WishList> _WishListRepository;
        public UserService()
        {
			_WishListRepository = new Repository<WishList>();
			_CartRepository = new Repository<Cart>();
			_Userrepository = new Repository<User>();
        }
        public async Task<bool> add(CreateUserModel requet)
		{
			try
			{
				var obj = new User()
				{
					Name = requet.Name,
					Birth = requet.Birth,
					Gender = requet.Gender,
					Email = requet.Email,
					Phone = requet.Phone,
					UserName = requet.UserName,
					Password = requet.Password,
					CreatedDate = DateTime.Now,
					Status = 1,
				};
				await _Userrepository.CreateAsync(obj);

				var cart = new Cart()
				{
					Id_User = obj.Id,
				};

				
				await _CartRepository.CreateAsync(cart);
				return true;
			}
			catch (Exception)
			{

				return false;
			}
		}

		public Task<List<UserModel>> Getall()
		{
			throw new NotImplementedException();
		}

		public Task<UserModel> GetbyId(int id)
		{
			throw new NotImplementedException();
		}

		public Task<bool> remove(int id)
		{
			throw new NotImplementedException();
		}

		public Task<bool> update(int id, UpdateUserModel requet)
		{
			throw new NotImplementedException();
		}
	}
}
