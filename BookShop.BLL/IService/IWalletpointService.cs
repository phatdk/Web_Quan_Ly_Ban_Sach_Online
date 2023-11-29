using BookShop.BLL.ConfigurationModel.PointTranHistoryModel;
using BookShop.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.IService
{
    public interface IWalletpointService
    {
        public Task<bool> Add(WalletPointViewModel model);
        public Task<bool> Update(int userId, WalletPointViewModel model);
        public Task<WalletPointViewModel> GetById(int userId);
    }
}
