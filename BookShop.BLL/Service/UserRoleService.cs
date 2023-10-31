using BookShop.BLL.ConfigurationModel.GenreModel;
using BookShop.BLL.ConfigurationModel.UserModel;
using BookShop.BLL.IService;
using BookShop.DAL.Entities;
using BookShop.DAL.Entities.Identity;
using BookShop.DAL.Repositopy;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.Service
{
    public class UserRoleService : IUserRoleService
    {
        protected readonly IRepository<UserRoles> _repository;

        public UserRoleService()
        {
            _repository = new Repository<UserRoles>();
        }

        public async Task<bool> Add(UserRoleModel requet)
        {
            try
            {
                var obj = new UserRoles()
                {
                    
                    RoleId = requet.RoleId,
                    UserId = requet.UserId
                    
                };
                await _repository.CreateAsync(obj);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<List<UserRoleModel>> GetAll()
        {
            var obj = await _repository.GetAllAsync();
            var query = from g in obj
                        select new UserRoleModel()
                        {
                            RoleId = g.RoleId,
                            UserId = g.UserId
                        };
            return query.ToList();
        }

        public async Task<UserRoleModel> GetById(int id)
        {
            var obj = await _repository.GetByIdAsync(id);

            return new UserRoleModel()
            {
                RoleId = obj.RoleId,
                UserId = obj.UserId
            };
        }

        public async Task<bool> Delete(UserRoleModel model)
        {
            try
            {
                var obj = (await _repository.GetAllAsync()).FirstOrDefault(x=>x.RoleId==model.RoleId&&x.UserId==model.UserId);
                if (obj != null)
                {
                    await _repository.RemoveAsync(obj);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<bool> Update(int id, UserRoleModel requet)
        {
            try
            {
                var obj = await _repository.GetByIdAsync(id);
                if (obj != null)
                {

                    obj.RoleId = requet.RoleId;
                    obj.UserId = requet.UserId;
                    await _repository.UpdateAsync(id, obj);
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
