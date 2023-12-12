using BookShop.BLL.ConfigurationModel.WorkShiftsModel;
using BookShop.BLL.IService;
using BookShop.BLL.Service;
using BookShop.DAL.Entities;
using BookShop.Web.Client.Models;
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
        public async Task<IActionResult> Index([FromQuery(Name = "p")] int currentPages)
        {
            var list = await _workShiftsService.GetAllWorkShifts();
            int pagesize = 10;
            if (pagesize <= 0)
            {
                pagesize = 10;
            }
            int countPages = (int)Math.Ceiling((double)list.Count() / pagesize);
            if (currentPages > countPages)
            {
                currentPages = countPages;
            }
            if (currentPages < 1)
            {
                currentPages = 1;
            }

            var pagingmodel = new PagingModel()
            {
                currentpage = currentPages,
                countpages = countPages,
                generateUrl = (int? p) => Url.Action("Index", " WorkShifts", new { areas = "Admin", p = p, pagesize = pagesize })
            };
            ViewBag.pagingmodel = pagingmodel;
            list = list.Skip((pagingmodel.currentpage - 1) * pagesize).Take(pagesize).ToList();
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
