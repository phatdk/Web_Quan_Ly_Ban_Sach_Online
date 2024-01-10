using BookShop.BLL.ConfigurationModel.AuthorModel;
using BookShop.BLL.ConfigurationModel.OrderDetailModel;
using BookShop.BLL.ConfigurationModel.ProductPromotionModel;
using BookShop.BLL.ConfigurationModel.PromotionModel;
using BookShop.BLL.IService;
using BookShop.BLL.Service;
using BookShop.DAL.ApplicationDbContext;
using BookShop.DAL.Entities;
using BookShop.Web.Client.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.Security.Principal;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using static NuGet.Packaging.PackagingConstants;

namespace BookShop.Web.Client.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class PromotionController : Controller
	{
		IPromotionService _promotionService;
		IPromotionTypeService _promotionTypeService;
		IProductPromotionService _productPromotionService;
		IProductService _productService;
		IAuthorService _authorService;
		IGenreService _genreService;
		ICollectionService _collectionService;
		public PromotionController(IPromotionService promotionService, IPromotionTypeService promotionTypeService, IProductPromotionService productPromotionService, IProductService productService, IAuthorService authorService, IGenreService genreService, ICollectionService collectionService)
		{
			_promotionTypeService = promotionTypeService;
			_promotionService = promotionService;
			_productPromotionService = productPromotionService;
			_productService = productService;
			_authorService = authorService;
			_genreService = genreService;
			_collectionService = collectionService;
		}
		public async Task<IActionResult> Index()
		{
			ViewBag.Type = (await _promotionTypeService.GetAll()).Where(x => x.Status == 1).ToList();
			return View();
		}

		public async Task<IActionResult> GetPromotion(int page, int? type, int? status, int? date, string? keyWord)
		{
			var dataList = await _promotionService.GetAll();
			if (type != null) dataList = dataList.Where(x => x.Id_Type == Convert.ToInt32(type)).ToList();
			if (status != null) dataList = dataList.Where(x => status == Convert.ToInt32(status)).ToList();
			if (date != null)
			{
				var checkList = new List<PromotionViewModel>();
				if (Convert.ToInt32(date) == 1) // còn hạn
				{
					foreach (var item in dataList)
					{
						DateTime endTime = Convert.ToDateTime(item.EndDate);
						if (endTime.CompareTo(DateTime.Now) >= 0) checkList.Add(item);
					}
				}
				else if (Convert.ToInt32(date) == 0) // hết hạn
				{
					foreach (var item in dataList)
					{
						DateTime endTime = Convert.ToDateTime(item.EndDate);
						if (endTime.CompareTo(DateTime.Now) < 0) checkList.Add(item);
					}
				}
				dataList = checkList;
			}
			if (!string.IsNullOrEmpty(keyWord))
			{
				dataList = dataList.Where(
					x => x.Code.ToLower().Contains(keyWord.ToLower())
					|| x.Name.ToLower().Contains(keyWord.ToLower())
					).ToList();
			}
			dataList = dataList.OrderByDescending(x => x.CreatedDate).ToList();
			int pageSize = 10;
			double totalPage = (double)dataList.Count / pageSize;
			dataList = dataList.Skip((page - 1) * pageSize).Take(pageSize).ToList();
			return Json(new { data = dataList, page = page, max = Math.Ceiling(totalPage) });
		}

		public async Task<IActionResult> Details(int id)
		{
			HttpContext.Session.Remove("sessionAddList");
			var promotion = await _promotionService.GetById(id);
			if (promotion == null)
			{
				return NotFound();
			}
			var author = (await _authorService.Getall()).Where(x => x.Status == 1).ToList();
			var genre = (await _genreService.GetAll()).Where(x => x.Status == 1).ToList();
			var collection = (await _collectionService.GetAll()).Where(x => x.Status == 1).ToList();
			ViewBag.Author = author;
			ViewBag.Genre = genre;
			ViewBag.Collection = collection;
			return View(promotion);
		}

		public async Task<IActionResult> AddedList(int promotionId)
		{
			var sessionAddList = HttpContext.Session.GetString("sessionAddList");
			if (string.IsNullOrEmpty(sessionAddList))
			{
				var addedList = await _productPromotionService.GetByPromotion(promotionId);
				HttpContext.Session.SetString("sessionAddList", JsonConvert.SerializeObject(addedList));
				sessionAddList = HttpContext.Session.GetString("sessionAddList");
			}
			var data = JsonConvert.DeserializeObject<List<ProductPromotionViewModel>>(sessionAddList);
			return Json(data);
		}

		public async Task<IActionResult> AddProduct(int promotionId, int productId)
		{
			var addedList = HttpContext.Session.GetString("sessionAddList");
			var ppList = new List<ProductPromotionViewModel>();
			var product = await _productService.GetById(productId);
			var promotion = await _promotionService.GetById(promotionId);
			if (product == null) return Json(new { success = false, errorMessage = "Không tìm thấy sản phẩm" });
			if (promotion == null) return Json(new { success = false, errorMessage = "Không tìm thấy mã khuyến mãi" });
			if (!string.IsNullOrEmpty(addedList))
			{
				ppList = JsonConvert.DeserializeObject<List<ProductPromotionViewModel>>(addedList);
				var prod = ppList.FirstOrDefault(x => x.Id_Product == productId);
				if (prod != null)
				{
					ppList.Remove(prod);
					goto removeItem;
				}
			}
			var pp = new ProductPromotionViewModel
			{
				Id_Product = product.Id,
				NameProduct = product.Name,
				ProductPrice = product.Price,
				Id_Promotion = promotion.Id,
				NamePromotion = promotion.Name,
				AmountReduct = promotion.AmountReduct,
				PercentReduct = promotion.PercentReduct,
				ReductMax = promotion.ReductMax,
			};
			if (pp.PercentReduct != null)
			{
				var amount = Convert.ToInt32(Math.Floor(Convert.ToDouble((pp.ProductPrice / 100) * pp.PercentReduct)));
				if (amount > pp.ReductMax) amount = pp.ReductMax;
				pp.TotalReduct = amount;
			}
			else pp.TotalReduct = Convert.ToInt32(pp.AmountReduct);
			ppList.Add(pp);
		removeItem:
			HttpContext.Session.SetString("sessionAddList", JsonConvert.SerializeObject(ppList));
			return Json(new { success = true });
		}

		[HttpPost]
		public async Task<IActionResult> AddToProduct(int id)
		{
			var promotion = await _promotionService.GetById(id);
			var addedList = HttpContext.Session.GetString("sessionAddList");
			var ppList = JsonConvert.DeserializeObject<List<ProductPromotionViewModel>>(addedList);
			List<int> listSession = ppList.Select(x => x.Id_Product).ToList();
			if(await _productPromotionService.AddProducts(id, listSession)) return Json(new { success = true });
			return Json(new {success = false});
		}

		public async Task<IActionResult> Create()
		{
			ViewBag.Type = (await _promotionTypeService.GetAll()).Where(x => x.Status == 1).ToList();
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Create(CreatePromotionModel request)
		{
			if (request.ReductMax == 0) request.ReductMax = Convert.ToInt32(request.AmountReduct);
			var result = await _promotionService.Add(request);
			var routeValues = new RouteValueDictionary { { "id", result.Id } };
			return RedirectToAction(nameof(Details), routeValues);
		}

		public async Task<IActionResult> Edit(int id)
		{
			var promotion = await _promotionService.GetById(id);
			if (promotion == null) return NotFound();

			var model = new UpdatePromotionModel
			{
				Id = promotion.Id,
				Name = promotion.Name,
				Code = promotion.Code,
				Condition = promotion.Condition,
				StorageTerm = promotion.StorageTerm,
				ConversionPoint = promotion.ConversionPoint,
				PercentReduct = promotion.PercentReduct,
				AmountReduct = promotion.AmountReduct,
				ReductMax = promotion.ReductMax,
				Quantity = promotion.Quantity,
				StartDate = promotion.StartDate,
				EndDate = promotion.EndDate,
				Description = promotion.Description,
				Status = promotion.Status,
				Id_Type = promotion.Id_Type,
				NameType = promotion.NameType,
			};
			if (model.AmountReduct != null) model.PercentReduct = 0;
			else model.AmountReduct = 0;
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(UpdatePromotionModel model)
		{
			if (Convert.ToInt32(model.AmountReduct) != 0) model.ReductMax = Convert.ToInt32(model.AmountReduct);

			await _promotionService.Update(model.Id, model);
			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> Delete(List<int> ids)
		{
			if (ids != null)
			{
				foreach (var id in ids)
				{
					await _promotionService.Delete(id);
				}
			}
			return RedirectToAction(nameof(Index));
		}
	}
}
