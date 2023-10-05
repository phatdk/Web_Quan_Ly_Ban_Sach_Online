using BookShop.BLL.ConfigurationModel.ProductModel;
using BookShop.BLL.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Web.Client.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        List<ProductViewModel> _list;
        ProductViewModel _product;

        IProductService _productService;

        public ProductController(IProductService productService)
        {
            _list = new List<ProductViewModel>();
            _product = new ProductViewModel();
            _productService = productService;
        }

        // GET: ProductController
        public async Task<IActionResult> Index()
        {
            _list.Clear();
            _list = await _productService.GetAll();
            return View(_list);
        }

        // GET: ProductController/Details/5
        [HttpGet("/detail/{type}/{id}")]
        public async Task<IActionResult> Details(int id, int type)
        {
            if (type == 2)
            {
                _product = await _productService.GetProductComboById(id);
            }
            else
            {
                _product = await _productService.GetById(id);
            }

            return View(_product);
        }

        // GET: ProductController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
