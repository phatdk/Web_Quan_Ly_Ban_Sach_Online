using BookShop.BLL.ConfigurationModel.BookModel;
using BookShop.BLL.ConfigurationModel.CollectionBookModel;
using BookShop.BLL.ConfigurationModel.ImageModel;
using BookShop.BLL.ConfigurationModel.ProductModel;
using BookShop.BLL.ConfigurationModel.PromotionModel;
using BookShop.BLL.IService;
using BookShop.BLL.Service;
using BookShop.DAL.Entities;
using BookShop.Web.Client.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static NuGet.Packaging.PackagingConstants;

namespace BookShop.Web.Client.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class ProductController : Controller
	{
		List<ProductViewModel> _listProduct;
		ProductViewModel _product;
		List<BookViewModel> _listBook;
		List<ImageViewModel> _listImage;
		List<CollectionModel> _listCollections;

		IProductService _productService;
		IBookService _bookService;
		ICollectionService _collectionService;
		IImageService _imageService;
		IPromotionService _promotionService;
		IProductPromotionService _productPromotionService;

		public ProductController(IProductService productService, IBookService bookService, ICollectionService collectionService, IImageService imageService, IPromotionService promotionService, IProductPromotionService productPromotionService)
		{
			_listProduct = new List<ProductViewModel>();
			_product = new ProductViewModel();
			_listBook = new List<BookViewModel>();
			_listImage = new List<ImageViewModel>();
			_listCollections = new List<CollectionModel>();
			_productService = productService;
			_bookService = bookService;
			_collectionService = collectionService;
			_imageService = imageService;
			_promotionService = promotionService;
			_productPromotionService = productPromotionService;
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
		public async Task<List<CollectionModel>> LoadCollection(int status)
		{
			var list = await _collectionService.GetAll();
			if (status == 1)
			{
				return list.Where(x => x.Status == 1).ToList();
			}
			else
			{
				return list;
			}
		}

		public async Task<string> UpLoadImage(IFormFile file, int productId, int index)
		{
			if (file != null)
			{
				var extension = Path.GetExtension(file.FileName);
				var filename = "Product_" + productId + "_" + index + "_" + extension;
				var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\product", filename);
				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await file.CopyToAsync(stream);
				}
				return filename;
			}
			return string.Empty;
		}

		// GET: ProductController
		public async Task<IActionResult> Index()
		{
			return View();
		}

		public async Task<IActionResult> GetProduct(int page, int? type, string? keyWord)
		{
			var dataList = await _productService.GetAll();
			if (type != null)
			{
				if (type >= 1)
				{
					dataList = dataList.Where(x => x.Type == Convert.ToInt32(type)).ToList();
				}
			}
			if (!string.IsNullOrEmpty(keyWord))
			{
				dataList = dataList.Where(
					x => x.Name.ToLower().Contains(keyWord.ToLower())
					|| x.CollectionName.ToLower().Contains(keyWord.ToLower())
					).ToList();
			}
			dataList = dataList.OrderByDescending(x => x.CreatedDate).ToList();
			int pageSize = 10;
			double totalPage = (double)dataList.Count / pageSize;
			dataList = dataList.Skip((page - 1) * pageSize).Take(pageSize).ToList();
			return Json(new { data = dataList, page = page, max = Math.Ceiling(totalPage) });
		}

		// GET: ProductController/Details/5
		public async Task<IActionResult> Details(int id)
		{
			_product = await _productService.GetById(id);
			_product.promotionViewModels = new List<PromotionViewModel>();
			var pp = await _productPromotionService.GetByProduct(_product.Id);
			foreach(var item in pp)
			{
				var promotion = await _promotionService.GetById(item.Id_Promotion);
				if(promotion != null) _product.promotionViewModels.Add(promotion);
			}
			return View(_product);
		}

		// GET: ProductController/Create
		[HttpGet]
		public async Task<IActionResult> Create()
		{
			ViewBag.listBook = await LoadBook(1);
			ViewBag.listCollection = await LoadCollection(1);
			return View();
		}

		// POST: ProductController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreateProductModel request)
		{
			try
			{
				var product = new CreateProductModel()
				{
					Name = request.Name,
					Quantity = request.Quantity,
					Price = request.Price,
					Description = request.Description,
					Status = request.Quantity == 0 ? 0 : 1,
					Type = request.bookSelected.Count() > 1 ? 2 : 1,
					CollectionId = request.CollectionId == 0 ? null : request.CollectionId,
					bookSelected = request.bookSelected,
				};
				var result = await _productService.Add(product);
				if (request.fileCollection != null && result.Id != 0)
				{
					var index = 0;
					foreach (var file in request.fileCollection)
					{
						var img = (await UpLoadImage(file, result.Id, index));
						var imageProduct = new CreateImageModel()
						{
							ImageUrl = "/img/product/" + img,
							Index = 0,
							Status = 1,
							Id_Product = result.Id,
						};
						await _imageService.Add(imageProduct);
						index++;
					}
				}
				return RedirectToAction(nameof(Index));
			}
			catch { return View(); }
		}

		// GET: ProductController/Edit/5
		public async Task<IActionResult> Edit(int id)
		{
			_product = await _productService.GetById(id);
			var updatemodel = new UpdateProductModel()
			{
				Id = _product.Id,
				Name = _product.Name,
				Quantity = _product.Quantity,
				Price = _product.Price,
				Description = _product.Description,
				Status = _product.Status,
				Type = _product.Type,
				CollectionId = _product.CollectionId,
				bookViewModels = _product.bookViewModels,
				imageViewModels = _product.imageViewModels,
			};
			updatemodel.bookSelected = new List<int> { };
			foreach (var item in updatemodel.bookViewModels)
			{
				updatemodel.bookSelected.Add(item.Id);
			}
			ViewBag.listBook = await LoadBook(1);
			ViewBag.listCollection = await LoadCollection(1);
			return View(updatemodel);
		}

		// POST: ProductController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(UpdateProductModel request)
		{
			try
			{
				if (request.fileCollection != null)
				{
					var imgvms = await _imageService.GetByProduct(request.Id);	// đếm số ảnh đầu vào
					for (var i = 0; i < imgvms.Count; i++)
					{
						var file = request.fileCollection[i];
						var imgUrl = "/img/product/" + await UpLoadImage(file, request.Id, i);
						if (imgvms[i].ImageUrl != imgUrl)
						{
							var img = new UpdateImageModel()
							{
								Id = imgvms[i].Id,
								ImageUrl = imgUrl,
							};
							await _imageService.Update(img);
						}
					}
				}
				if (request.bookSelected.Count() > 1)
				{
					request.Type = 2;
				}
				else request.Type = 1;
				request.Status = request.Quantity == 0 ? 0 : 1;
				var result = await _productService.Update(request);
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// Post: ProductController/Delete/5
		public async Task<IActionResult> Delete(int id)
		{
			var product = (await _productService.GetAll()).FirstOrDefault(u => u.Id == id);
			if (product != null)
			{
				await _productService.Delete(id);
				return Json(new { success = true });
			}
			return Json(new { success = "error" });
		}
	}
}
