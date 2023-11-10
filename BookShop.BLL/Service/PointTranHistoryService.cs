using BookShop.BLL.ConfigurationModel.PointTranHistoryModel;
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
    public class PointTranHistoryService : IPointTranHistoryService
    {
        private readonly IRepository<PointTransactionsHistory> _historyRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<Promotion> _promotionRepository;
        private readonly IRepository<WalletPoint> _WalletPointRepository;

        public PointTranHistoryService()
        {
            _historyRepository = new Repository<PointTransactionsHistory>();
            _orderRepository = new Repository<Order>();
            _promotionRepository = new Repository<Promotion>();
            _WalletPointRepository = new Repository<WalletPoint>();
        }

        public async Task<bool> Add(CreatePointTranHistoryModel model)
        {
            try
            {
                var wallpoint = await _WalletPointRepository.GetByIdAsync(model.Id_User);
                if (wallpoint == null)
                {
                    return false;
                }
                var obj = new PointTransactionsHistory()
                {
                    Point_Amount_Userd = model.PointUserd,
                    Remaining = wallpoint.Point - model.PointUserd,
                    CreatedDate = DateTime.Now,
                    Id_User = model.Id_User,
                    Id_Parents = model.Id_Parents,
                };
                var PointTransaction = await _historyRepository.CreateAsync(obj);
                if (PointTransaction == null)
                {
                    return false;
                }
                wallpoint.Point = PointTransaction.Remaining;
                await _WalletPointRepository.UpdateAsync(wallpoint.Id_User, wallpoint);
                return true;
            }
            catch (Exception ex) { return false; }
        }

        public async Task<List<PointTranHistoryViewModel>> GetByUser(int userId)
        {
            var histories = (await _historyRepository.GetAllAsync()).Where(c => c.Id_User == userId);
            var orders = await _orderRepository.GetAllAsync();
            var promotions = await _promotionRepository.GetAllAsync();
            var objlist = (from a in histories
                           join b in orders on a.Id_Parents equals b.Id into t
                           from b in t.DefaultIfEmpty()
                           join c in promotions on a.Id_Parents equals c.Id into i
                           from c in i.DefaultIfEmpty()
                           select new PointTranHistoryViewModel()
                           {
                               Id = a.Id,
                               PointUserd = a.Point_Amount_Userd,
                               Remaining = a.Remaining,
                               CreatedDate = a.CreatedDate,
                               Id_User = a.Id_User,
                               Id_Parents = a.Id_Parents,
                               Notif = orders != null ? "giao dịch mua hàng" : "giao dịch đổi phiếu giảm giá" + c.Name,
                           }).ToList();
            return objlist;
        }
    }
}
