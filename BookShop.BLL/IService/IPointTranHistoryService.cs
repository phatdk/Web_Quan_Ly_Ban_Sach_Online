using BookShop.BLL.ConfigurationModel.PointTranHistoryModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.IService
{
    public interface IPointTranHistoryService
    {
        public Task<List<PointTranHistoryViewModel>> GetByUser(int userId);
        public Task<bool> Add(CreatePointTranHistoryModel model);
    }
}
