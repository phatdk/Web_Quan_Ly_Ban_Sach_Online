using BookShop.BLL.ConfigurationModel.SupplierModel;
using BookShop.BLL.IService;
using BookShop.DAL.Entities;
using BookShop.Web.Client.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Web.Client.Areas.Admin.Controllers.BookController
{
    [Area("Admin")] // cấu hình cái này để nó biết đây là trang quan lí
    [Authorize(Roles = "Admin")]
    [Route("admin/Supplier")] // thêm đường dẫn ở đây để tránh bị trùng với với các from khác ( bắt buộc ) đặt tên giống Bảng code u
    public class SuppliersController : Controller
    {
        private readonly ISupplierService _supplierService;
        public SuppliersController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        [HttpGet]
        [Route("listsupplier")]
        public async Task<IActionResult> Index([FromQuery(Name = "p")] int currentPages)
        {
            var suppliers = await _supplierService.GetAll();
            int pagesize = 10;
            if (pagesize <= 0)
            {
                pagesize = 10;
            }
            int countPages = (int)Math.Ceiling((double)suppliers.Count() / pagesize);
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
                generateUrl = (int? p) => Url.Action("Index", "Suppliers", new { areas = "Admin", p = p, pagesize = pagesize })
            };
            ViewBag.pagingmodel = pagingmodel;
            suppliers = suppliers.Skip((pagingmodel.currentpage - 1) * pagesize).Take(pagesize).ToList();
            return View(suppliers);
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

        [HttpGet("Edit/supplier/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var supplier = await _supplierService.GetById(id);
            return View(supplier);
        }
        
        [HttpPost("Edit/supplier/{id}")]
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

        [HttpGet("Detail/supplier/{id}")]
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
