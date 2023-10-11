using BookShop.BLL.ConfigurationModel.WorkShiftsModel;
using BookShop.BLL.IService;
using BookShop.BLL.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace BookShop.Web.Client.Areas.Admin.Controllers.QuanLiBanHangOffline
{
    [Area("Admin")]
    [Route("admin/WorkShifts")]
    public class WorkShiftsController : Controller
    {
        protected readonly IWorkShiftsService _workShiftsService;
        public WorkShiftsController()
        {
            _workShiftsService = new WorkShiftsService();

		}
        [Route("List")]
        public async Task<IActionResult> Index()
        {
            var list = await _workShiftsService.GetAllWorkShifts();
            return View(list);
        }
        [HttpGet]
        [Route("add")]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Create(CreateWorkShiftModel model)
        {
           var  obj =  await _workShiftsService.Add(model);
            if (obj == true)
            {
                return RedirectToAction("Index");
            }
            return Redirect("thất bại");
        }

        public async Task<IActionResult> Delete(int id)
        {
           await _workShiftsService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
