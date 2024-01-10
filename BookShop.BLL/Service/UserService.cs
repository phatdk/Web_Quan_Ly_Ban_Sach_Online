using BookShop.BLL.ConfigurationModel.CollectionBookModel;
using BookShop.BLL.ConfigurationModel.UserModel;
using BookShop.BLL.IService;
using BookShop.DAL.Entities;
using BookShop.DAL.Repositopy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
		protected readonly IRepository<Userr> _Userrepository;
		protected readonly IRepository<Cart> _CartRepository;
		protected readonly IRepository<WalletPoint> _WalletPointRepository;
		protected readonly IRepository<WishList> _WishListRepository;
		private readonly UserManager<Userr> _UserManager;
        public UserService(UserManager<Userr> userManager = null)
        {
            _WishListRepository = new Repository<WishList>();
            _CartRepository = new Repository<Cart>();
            _Userrepository = new Repository<Userr>();
            _WalletPointRepository = new Repository<WalletPoint>();
            _UserManager = userManager;
        }
		
		public async Task<List<string>> GetRoleUserById(int Id)
		{
			var user = await _UserManager.Users.FirstOrDefaultAsync(x => x.Id == Id);
			if (user==null)
			{
				return null;
			}
			var RoleUSer=await _UserManager.GetRolesAsync(user);
            if (RoleUSer.Count <0)
            {
                return null;
            }
			return RoleUSer.ToList();
        }
        public async Task<bool> Add(CreateUserModel requet)
		{
			try
			{
				var obj = new Userr()
				{
					Name = requet.Name,
					Birth = requet.Birth,
					Gender = requet.Gender,
					Email = requet.Email,
					//  Phone = requet.Phone,
					UserName = requet.UserName,
					//Password = requet.Password,
					CreatedDate = DateTime.Now,
					Status = 1,
				};
				var userstatus = await _Userrepository.CreateAsync(obj);

				var cart = new Cart()
				{
					Id_User = userstatus.Id,
					CreatedDate = DateTime.Now,
					Status = 1,
				};
				await _CartRepository.CreateAsync(cart);
				var walletPoint = new WalletPoint()
				{
					Id_User = userstatus.Id,
					CreatedDate = DateTime.Now,
					Status = 1,
					Point = 0,
				};
				await _WalletPointRepository.CreateAsync(walletPoint);
				return true;
			}
			catch (Exception)
			{

				return false;
			}
		}

		public async Task<List<UserModel>> GetAll()
		{
			var obj = await _Userrepository.GetAllAsync();
			var wallet = await _WalletPointRepository.GetAllAsync();
			var query = from c in obj
						join a in wallet on c.Id equals a.Id_User into i
						from a1 in i.DefaultIfEmpty()
						select new UserModel()
						{
							Code = c.Code,
							Gender = c.Gender,
							Id = c.Id,
							Name = c.Name,
							Status = c.Status,
							CreatedDate = c.CreatedDate,
							Email = c.Email,
							Phone = c.PhoneNumber,
							UserName = c.UserName,
							// Password = c.Password,
							Point = (a1 == null) ? 0 : a1.Point,
						};
			return query.ToList();
		}

		public async Task<UserModel> GetById(int id)
		{
			var obj = (await _Userrepository.GetAllAsync()).Where(x=>x.Id == id);
			var wallet = await _WalletPointRepository.GetAllAsync();
			var query = (from c in obj
						join a in wallet on c.Id equals a.Id_User into i
						from a1 in i.DefaultIfEmpty()
						select new UserModel()
						{
							Code = c.Code,
							Gender = c.Gender,
							Id = c.Id,
							Name = c.Name,
							Status = c.Status,
							CreatedDate = c.CreatedDate,
							Email = c.Email,
							// Phone = c.Phone,
							UserName = c.UserName,
							// Password = c.Password,
							Point = (a1 == null)? 0 : a1.Point,
						}).FirstOrDefault();
			return query;
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
					//obj.Phone = requet.Phone;
					// obj.Password = requet.Password;
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
