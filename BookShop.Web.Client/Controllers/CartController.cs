using BookShop.BLL.ConfigurationModel.CartDetailModel;
using BookShop.BLL.ConfigurationModel.ProductModel;
using BookShop.BLL.IService;
using BookShop.DAL.Entities;
using Microsoft.AspNetCore.Http;
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
        public ActionResult Index()
        {
            return View();
        }

        // GET: CartController/Details/5
        public async Task<IActionResult> CartDetails()
        {
            var user = await GetCurrentUserAsync();
            List<CartDetailViewModel> detail = new List<CartDetailViewModel>();
            if (user == null)
            {
                var customCartChar = HttpContext.Session.GetString("customCart");
                if (!string.IsNullOrEmpty(customCartChar))
                {
                    detail = JsonConvert.DeserializeObject<List<CartDetailViewModel>>(customCartChar);
                }
            }
            else detail = await _cartDetailService.GetByCart(user.Id);
            return View(detail);
        }

        // GET: CartController/Create
        public async Task<IActionResult> AddToCart(int id, int quantity)
        {
            var user = await GetCurrentUserAsync();
            var product = await _productService.GetById(id);
            if (product == null) return Json(new { success = false, errorMessage = "Không tìm thấy sản phẩm" });
            if (user != null)
            {
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
                }
            }
            else
            {
                var customCartChar = HttpContext.Session.GetString("customCart");
                var customCart = new List<CartDetailViewModel>();
                if (!string.IsNullOrEmpty(customCartChar))
                {
                    customCart = JsonConvert.DeserializeObject<List<CartDetailViewModel>>(customCartChar);
                    var pc = customCart.FirstOrDefault(x => x.Id_Product == product.Id);
                    if (pc != null)
                    {
                        pc.Quantity += quantity;
                    }
                    else
                    {
                        pc = new CartDetailViewModel()
                        {
                            Id_Product = product.Id,
                            Quantity = quantity,
                            ProductName = product.Name,
                            ProductPrice = product.Price,
                            Status = product.Status,
                            CreatedDate = DateTime.Now,
                        };
                        customCart.Add(pc);
                    }
                    HttpContext.Session.SetString("customCart", JsonConvert.SerializeObject(customCart));
                }
                else
                {
                    var pc = new CartDetailViewModel()
                    {
                        Id_Product = product.Id,
                        CreatedDate = DateTime.Now,
                        ProductPrice = product.Price,
                        Quantity = quantity,
                    };
                    customCart.Add(pc);
                    HttpContext.Session.SetString("customCart", JsonConvert.SerializeObject(customCart));
                }
                return Json(new { success = true });
            }
            return Json(new { success = true });
        }

        // GET: CartController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CartController/Edit/5
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

        // GET: CartController/Delete/5
        public async Task<IActionResult> RemoveProduct(int id)
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var detail = (await _cartDetailService.GetByCart(user.Id)).FirstOrDefault(u => u.Id == id);
                if (detail != null)
                {
                    await _cartDetailService.Delete(detail.Id);
                    return RedirectToAction(nameof(CartDetails));
                }
                return Json(new { success = "error" });

            }
            else
            {
                var customCartChar = HttpContext.Session.GetString("customCart");
                var customCart = new List<CartDetailViewModel>();
                if (!string.IsNullOrEmpty(customCartChar))
                {
                    customCart = JsonConvert.DeserializeObject<List<CartDetailViewModel>>(customCartChar);
                    var pc = customCart.FirstOrDefault(x => x.Id_Product == id);
                    if (pc != null)
                    {
                        customCart.Remove(pc);
                        HttpContext.Session.SetString("customCart", JsonConvert.SerializeObject(customCart));
                        return RedirectToAction(nameof(CartDetails));
                    }
                }
                return RedirectToAction(nameof(CartDetails));
            }

        }
    }
}
