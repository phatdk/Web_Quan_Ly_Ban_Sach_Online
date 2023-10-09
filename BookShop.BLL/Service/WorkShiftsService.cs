using BookShop.BLL.ConfigurationModel.WorkShiftsModel;
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
	public class WorkShiftsService : IWorkShiftsService
	{
		private readonly IRepository<WorkShift> _WorkShiftrepository;
		public WorkShiftsService()
		{
			_WorkShiftrepository = new Repository<WorkShift>();
		}

        public async Task<bool> Add(CreateWorkShiftModel model)
        {
            try
            {
                var obj = new WorkShift()
                {
                    Shift = model.Shift,
                    Time = model.Time,
                    CreatedDate = DateTime.Now,
                    Status = 1,
                    
                };
                await _WorkShiftrepository.CreateAsync(obj);
                return true;
            }
            catch (Exception ex) { return false; }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                await _WorkShiftrepository.RemoveAsync(id);
                return true;
            }
            catch (Exception ex) { return false; }
        }

        public async Task<List<WorkShiftsViewModel>> GetAllWorkShifts()
		{
			var list = await _WorkShiftrepository.GetAllAsync();
			var query = from c in list
						select new WorkShiftsViewModel()
						{
							CreatedDate = c.CreatedDate,
							Status = c.Status,
							Id = c.Id,
							Time = c.Time,
							Shift = c.Shift,
						};
			return query.ToList();
		}

        public Task<WorkShiftsViewModel> GetByUserId(int workShiftId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(int id, UpdateWorkShiftModel model)
        {
            throw new NotImplementedException();
        }
    }
}
