using BookShop.BLL.ConfigurationModel.UserModel;

namespace BookShop.BLL.IService
{
    public interface IUserRoleService
    {
        Task<bool> Add(UserRoleModel requet);
        Task<bool> Delete(UserRoleModel requet);
        Task<List<UserRoleModel>> GetAll();
        Task<UserRoleModel> GetById(int id);
        Task<bool> Update(int id, UserRoleModel requet);
    }
}