using BookShop.BLL.ConfigurationModel.Collection;
using BookShop.BLL.ConfigurationModel.UserModel;
using BookShop.BLL.IService;
using BookShop.DAL.Entities;
using BookShop.DAL.Repositopy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
        public async Task<bool> Add(CreateUserModel requet)
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

		public async Task<List<UserModel>> Getall()
		{
			var obj = await _Userrepository.GetAllAsync();
			var query = from c in obj

						select new UserModel()
						{
							Gender = c.Gender,
							Id = c.Id,
							Name = c.Name,
							Status = c.Status,
							CreatedDate = c.CreatedDate,
							Email = c.Email,
							Phone = c.Phone,
							UserName = c.UserName,
							Password = c.Password,
						};
			return query.ToList();
		}

		public async Task<UserModel> GetById(int id)
		{
			var obj = await _Userrepository.GetByIdAsync(id);
			return new UserModel()
			{
				Gender = obj.Gender,
				Id = obj.Id,
				Name = obj.Name,
				Status = obj.Status,
				CreatedDate = obj.CreatedDate,
				Email = obj.Email,
				Phone = obj.Phone,
				UserName = obj.UserName,
				Password = obj.Password,
			};
		}

		public async Task<bool> Delete(int id)
		{
			try
			{
				var obj = await _Userrepository.GetByIdAsync(id);
				if (obj != null)
				{
					await _Userrepository.RemoveAsync(id);
					return true;
				}
				return false;
			}
			catch (Exception)
			{

				return false;
			}
		}

		public async Task<bool> Update(int id, UpdateUserModel requet)
		{
			try
			{
				var obj = await _Userrepository.GetByIdAsync(id);
				if (obj != null)
				{
					obj.Name = requet.Name;
					obj.Birth = requet.Birth;
					obj.Gender = requet.Gender;
					obj.Email = requet.Email;
					obj.Phone = requet.Phone;
					obj.Password = requet.Password;
					obj.CreatedDate = DateTime.Now;
					obj.Status = requet.Status;
					await _Userrepository.UpdateAsync(id, obj);
					return true;
				}
				return false;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}
