using BookShop.BLL.ConfigurationModel.BookModel;
using BookShop.BLL.ConfigurationModel.ImageModel;
using BookShop.BLL.ConfigurationModel.ProductModel;
using BookShop.BLL.IService;
using BookShop.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Web.Client.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class ProductController : Controller
	{
		List<ProductViewModel> _listProduct;
		ProductViewModel _product;

		List<BookViewModel> _listBook;
		List<ImageViewModel> _listImage;

		IProductService _productService;
		IBookService _bookService;

		public ProductController(IProductService productService, IBookService bookService)
		{
			_listProduct = new List<ProductViewModel>();
			_product = new ProductViewModel();
			_listBook = new List<BookViewModel>();
			_listImage = new List<ImageViewModel>();
			_productService = productService;
			_bookService = bookService;
		}
		public async Task<List<BookViewModel>> LoadBook(int status)
		{
			var list = await _bookService.GetAll();
			if (status == 1)
			{
				return list.Where(x => x.Status == 1).ToList();
			}
			else
			{
				return list;
			}
		}

		// GET: ProductController
		public async Task<IActionResult> Index()
		{
			_listProduct.Clear();
			_listProduct = await _productService.GetAll();
			return View(_listProduct);
		}

		// GET: ProductController/Details/5
		[HttpGet("/product/detail/{type}/{id}")]
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
		[HttpGet]
		public async Task<IActionResult> Create()
		{
			ViewBag.listBook = await LoadBook(1);
			ViewBag.selectedBook = _listBook;
			return View();
		}

		// POST: ProductController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(string name, int quantity, int price, string description, string[] selectedOptions, IFormFile formData)
		{
			var product = new CreateProductModel()
			{
				Name = name,
				Quantity = quantity,
				Price = price,
				Description = description,
				Status = 1,
				Type = selectedOptions.Count() > 1 ? 2 : 1,
			};
			foreach (var item in selectedOptions)
			{
				var bookitem = new BookViewModel()
				{
					Id = int.Parse(item)
				};
				product.bookViewModels.Add(bookitem);
			}
			var image = new ImageViewModel()
			{
				ImageUrl = formData.Name,
				Index = 1,
				Status = 1,
			};
			product.imageViewModels.Add(image);
			return Json(new { success = true });
			try
			{
				// var result = await _productService.Add(product);
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
		public async Task<IActionResult> Delete(int id)
		{
			var user = (await _productService.GetAll()).FirstOrDefault(u => u.Id == id);
			if (user != null)
			{
				await _productService.Delete(id);
				return Ok();
			}
			return Json(new { success = "error" });
		}

		// POST: ProductController/Delete/5
		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public ActionResult Delete(int id, IFormCollection collection)
		//{
		//	try
		//	{
		//		return RedirectToAction(nameof(Index));
		//	}
		//	catch
		//	{
		//		return View();
		//	}
		//}
	}
}
