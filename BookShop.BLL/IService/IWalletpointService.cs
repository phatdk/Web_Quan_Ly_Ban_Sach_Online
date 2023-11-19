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
        public Task<bool> Add(WalletPoint model);
        public Task<bool> Update(int userId, WalletPoint model);
        public Task<WalletPoint> GetById(int userId);
    }
}
