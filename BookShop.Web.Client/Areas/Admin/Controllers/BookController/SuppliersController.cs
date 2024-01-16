using BookShop.BLL.ConfigurationModel.SupplierModel;
using BookShop.BLL.IService;
using BookShop.BLL.Service;
using BookShop.DAL.Entities;
using BookShop.Web.Client.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Web.Client.Areas.Admin.Controllers.BookController
{
    [Area("Admin")] // cấu hình cái này để nó biết đây là trang quan lí
	[Authorize(Roles = "Admin,Staff")]
	public class SuppliersController : Controller
    {
        private readonly ISupplierService _supplierService;
        List<SupplierViewModel> _supplierView = new List<SupplierViewModel>();
        public SuppliersController(ISupplierService supplierService)
        {
            _supplierView = new List<SupplierViewModel>();
            _supplierService = supplierService;
        }

        [HttpGet]
       
        public async Task<IActionResult> Index()
        {
            return View();
        }
		public async Task<IActionResult> Getdata(int page, int? status, string? keyWord)
		{

			_supplierView = await _supplierService.GetAll();
			if (keyWord != null)
			{
				_supplierView = _supplierView.Where(c => c.Name.Contains(keyWord)).ToList();
			}
			if (status != null)
			{
				_supplierView = _supplierView.Where(c => c.Status == Convert.ToInt32(status)).ToList();
			}
			var supplier = _supplierView.OrderByDescending(c => c.CreatedDate).ToList();
			int pageSize = 10;
			double totalPage = (double)supplier.Count / pageSize;
			supplier = supplier.Skip((page - 1) * pageSize).Take(pageSize).ToList();
			return Json(new { data = supplier, page = page, max = Math.Ceiling(totalPage) });
		}
		[HttpGet("Create/supplier")]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost("Create/supplier")]
        public async Task<IActionResult> Create(Supplier supplier)
        {
            if (!ModelState.IsValid)
            {
                var createSupplierModel = new CreateSupplierModel
                {
                    Name = supplier.Name,
                    Index = supplier.Index,
                    Status = supplier.Status
                };

                await _supplierService.Add(createSupplierModel);
                return RedirectToAction("Index");
            }
            return View(supplier);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var supplier = await _supplierService.GetById(id);
            return View(supplier);
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Author supplier)
        {
            if (!ModelState.IsValid)
            {
                var updateAuthorModel = new UpdateSuplierModel
                {
                    Name = supplier.Name,
                    Index = supplier.Index,
                    Status = supplier.Status
                };

                await _supplierService.Update(id, updateAuthorModel);
                return RedirectToAction("Index");
            }
            return View(supplier);
        }

        
        public async Task<IActionResult> Details(int id)
        {
            var supplier = await _supplierService.GetById(id);
            return View(supplier);
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (await _supplierService.Delete(id))
            {
                return RedirectToAction("Index");
            }
            else return BadRequest();
        }
    }
}
