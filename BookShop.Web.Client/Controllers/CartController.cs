using BookShop.BLL.ConfigurationModel.CartDetailModel;
using BookShop.BLL.IService;
using BookShop.DAL.Entities;
using BookShop.Web.Client.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BookShop.Web.Client.Controllers
{
	public class CartController : Controller
	{
		private readonly UserManager<Userr> _userManager;
		private readonly IUserService _userService;
		private readonly IProductService _productService;
		private readonly ICartService _cartService;
		private readonly ICartDetailService _cartDetailService;

		public CartController(UserManager<Userr> userManager, IUserService userService, IProductService productService, ICartService cartService, ICartDetailService cartDetailService)
		{
			_userManager = userManager;
			_userService = userService;
			_productService = productService;
			_cartService = cartService;
			_cartDetailService = cartDetailService;
		}
		private Task<Userr> GetCurrentUserAsync()
		{
			return _userManager.GetUserAsync(HttpContext.User);
		}
		// GET: CartController
		//public ActionResult Index()
		//{
		//	return View();
		//}

        // GET: CartController/Details/5
        public async Task<IActionResult> CartDetails()
        {
            var user = await GetCurrentUserAsync();
            List<CartDetailViewModel> detail = new List<CartDetailViewModel>();

            if (user != null)
            {
                var cartCheck = await _cartService.GetByUser(user.Id);

                if (cartCheck == null)
                {
                    await _cartService.Add(new CartViewModel() { Id_User = user.Id });
				}

				detail = await _cartDetailService.GetByCart(user.Id);
				foreach(var item in detail)
				{
					var product = await _productService.GetById(item.Id_Product);
					item.NewPrice = product.NewPrice;
					item.TotalPrice = item.Quantity * item.NewPrice;
				}

				// Set IsSelected to true for all items initially
				foreach (var item in detail)
                {
                    item.IsSelected = true;
                    item.IsCanceled = false;
                }
            }
            else
            {
                var customCartChar = HttpContext.Session.GetString("sessionCart");
                if (!string.IsNullOrEmpty(customCartChar))
                {
                    detail = JsonConvert.DeserializeObject<List<CartDetailViewModel>>(customCartChar);
					foreach(var item in detail)
                    {
                        var product = await _productService.GetById(item.Id_Product);
						item.NewPrice = product.NewPrice;
						item.TotalPrice = item.Quantity * item.NewPrice;
						item.SoLuongKho = product.Quantity;
						item.ImgProductCartDetail = product.ImgUrl;
                    }
                }
                else
                {
                    HttpContext.Session.SetString("sessionCart", JsonConvert.SerializeObject(detail));
                }
            }

            return View(detail);
        }



        // GET: CartController/Create
        public async Task<IActionResult> AddToCart(int id, int quantity)
		{
			var user = await GetCurrentUserAsync();
			var product = await _productService.GetById(id);
			if (product == null) return Json(new { success = false, errorMessage = "Không tìm thấy sản phẩm" });
			if (user != null) // có đăng nhập
			{
				var cartCheck = await _cartService.GetByUser(user.Id);
				if (cartCheck == null)
				{
					await _cartService.Add(new CartViewModel() { Id_User = user.Id, });
				}
				var userId = user.Id;
				var cart = await _cartService.GetByUser(userId);
				if (cart == null)
				{
					cart = new CartViewModel()
					{
						Id_User = userId,
					};
					await _cartService.Add(cart);
				}
				var pc = (await _cartDetailService.GetByCart(userId)).FirstOrDefault(p => p.Id_Product == product.Id);
				if (pc == null)
				{
					var cpc = new CreateCartDetailModel()
					{
						Id_User = userId,
						Id_Product = product.Id,

						Quantity = quantity,
					};
					await _cartDetailService.Add(cpc);
					return Json(new { success = true });
				}
				else
				{
					var upc = new UpdateCartDetailModel()
					{
						Quantity = pc.Quantity + quantity,
					};
					if (upc.Quantity > product.Quantity)
					{
						return Json(new { success = false, errorMessage = "Số lượng vượt quá số lượng tồn kho" });
					}
					await _cartDetailService.Update(pc.Id, upc);
					return Json(new { success = true });
				}
			}
			else // không đăng nhập
			{
				var customCartChar = HttpContext.Session.GetString("sessionCart");
				var customCart = new List<CartDetailViewModel>();
				var pc = new CartDetailViewModel();
				if (!string.IsNullOrEmpty(customCartChar))
				{
					customCart = JsonConvert.DeserializeObject<List<CartDetailViewModel>>(customCartChar);
					pc = customCart.FirstOrDefault(x => x.Id_Product == product.Id);
					if (pc != null)
					{
						pc.Quantity += quantity;
						pc.TotalPrice = product.Price * pc.Quantity;
						HttpContext.Session.SetString("sessionCart", JsonConvert.SerializeObject(customCart));
						return Json(new { success = true });
					}
				}
				pc = new CartDetailViewModel()
				{
					Id_Product = product.Id,
					Quantity = quantity,
					ProductName = product.Name,
					ProductPrice = product.Price,
					Status = product.Status,
					CreatedDate = DateTime.Now,
					TotalPrice = product.Price * quantity,
					ImgProductCartDetail = product.ImgUrl,
					SoLuongKho = product.Quantity
					
				};
				customCart.Add(pc);
				HttpContext.Session.SetString("sessionCart", JsonConvert.SerializeObject(customCart));
				return Json(new { success = true });
			}
		}

		// POST: CartController/Edit/5
		[HttpPost]
		public async Task<IActionResult> EditCart(int id, int quantity) // truyền id cart detail (không phải idUser) và số lượng trên thẻ input
		{
			try
			{
				var itemCart = await _cartDetailService.GetById(id);
				if(itemCart != null) {
					itemCart.Quantity = quantity!=(int)quantity?1:quantity;
					var result = await _cartDetailService.Update(id, new UpdateCartDetailModel { Quantity = itemCart.Quantity });
					return Json(new { success = result });
				}
				return Json(new { success = false, message = "không tìm thấy item" });
			}
			catch (Exception ex)
			{
				return Json(new { success = false, errorMessage = ex });
			}
		}

		// GET: CartController/Delete/5
		public async Task<IActionResult> RemoveProduct(int id)
		{
			var user = await GetCurrentUserAsync();
			if (user != null) // co nguoi
			{
				var cd = (await _cartDetailService.GetByCart(user.Id)).FirstOrDefault(u => u.Id_Product == id);
				if (cd != null)
				{
					await _cartDetailService.Delete(cd.Id);
					return RedirectToAction(nameof(CartDetails));
				}
				return Json(new { success = "error" + "product: " + id + "| user: " + user.Id });

			}
			else
			{
				var customCartChar = HttpContext.Session.GetString("sessionCart");
				var customCart = new List<CartDetailViewModel>();
				if (!string.IsNullOrEmpty(customCartChar))
				{
					customCart = JsonConvert.DeserializeObject<List<CartDetailViewModel>>(customCartChar);
					var pc = customCart.FirstOrDefault(x => x.Id_Product == id);
					if (pc != null)
					{
						customCart.Remove(pc);
						HttpContext.Session.SetString("sessionCart", JsonConvert.SerializeObject(customCart));
						return RedirectToAction(nameof(CartDetails));
					}
				}
				return RedirectToAction(nameof(CartDetails));
			}

		}
        public async Task<IActionResult> RemoveAllProduct()
        {
            var user = await GetCurrentUserAsync();
            if (user != null) // co nguoi
            {
                var cd = (await _cartDetailService.GetByCart(user.Id)).ToList();
                if (cd != null)
                {
					foreach (var item in cd.ToList())
					{
						await _cartDetailService.Delete(item.Id);
					}
                    return RedirectToAction(nameof(CartDetails));
                }
                return Json(new { success = "error" + "product: " + "| user: " + user.Id });

            }
            else
            {
                var customCartChar = HttpContext.Session.GetString("sessionCart");
                var customCart = new List<CartDetailViewModel>();
                if (!string.IsNullOrEmpty(customCartChar))
                {
                    customCart = JsonConvert.DeserializeObject<List<CartDetailViewModel>>(customCartChar);
                    var pc = customCart.ToList();
                    if (pc != null)
                    {
						foreach (var itr in pc.ToList())
						{
							customCart.Remove(itr);
							HttpContext.Session.SetString("sessionCart", JsonConvert.SerializeObject(customCart));
							
						}
                        return RedirectToAction(nameof(CartDetails));
                    }
                }
                return RedirectToAction(nameof(CartDetails));
            }

        }
    }
}
