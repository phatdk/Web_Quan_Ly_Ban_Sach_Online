using BookShop.BLL.ConfigurationModel.SupplierModel;
using BookShop.BLL.IService;
using BookShop.DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Web.Client.Areas.Admin.Controllers.BookController
{
    [Area("Admin")] // cấu hình cái này để nó biết đây là trang quan lí
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
        public async Task<IActionResult> Index()
        {
            var suppliers = await _supplierService.GetAll();
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
