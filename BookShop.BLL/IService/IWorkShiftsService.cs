using BookShop.BLL.ConfigurationModel.WishListModel;
using BookShop.BLL.ConfigurationModel.WorkShiftsModel;
using BookShop.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.IService
{
	public interface IWorkShiftsService
	{
		public Task<List<WorkShiftsViewModel>> GetAllWorkShifts();
        public Task<WorkShiftsViewModel> GetByUserId(int workShiftId);
        public Task<bool> Add(CreateWorkShiftModel model);
        public Task<bool> Delete(int id);
        public Task<bool> Update(int id , UpdateWorkShiftModel model);
    }
}
