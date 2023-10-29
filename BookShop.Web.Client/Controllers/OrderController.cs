using BookShop.BLL.ConfigurationModel.CartDetailModel;
using BookShop.BLL.ConfigurationModel.OrderDetailModel;
using BookShop.BLL.ConfigurationModel.OrderModel;
using BookShop.BLL.ConfigurationModel.OrderPaymentModel;
using BookShop.BLL.ConfigurationModel.UserModel;
using BookShop.BLL.IService;
using BookShop.DAL.Entities;
using BookShop.Web.Client.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BookShop.Web.Client.Controllers
{
    public class OrderController : Controller
    {
        private List<OrderViewModel> _orders;
        private OrderViewModel _order;
        private readonly UserManager<Userr> _userManager;
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IPaymentFormService _paymentFormService;
        private readonly IOrderPaymentService _orderPaymentService;
        private readonly IUserService _userService;
        private readonly IUserPromotionService _userPromotionService;
        private readonly IStatusOrderService _statusService;
        private readonly ICartDetailService _cartDetailService;
        private readonly IPromotionService _promotionService;
        private readonly ProductPreviewService _productPreviewService;

        public OrderController(UserManager<Userr> userManager, IOrderService orderService, IProductService productService, IOrderDetailService orderDetailService, IPaymentFormService paymentFormService, IOrderPaymentService orderPaymentService, IUserService userService, IStatusOrderService statusOrderService, ICartDetailService cartDetailService, IPromotionService promotionService, IUserPromotionService userPromotionService)
        {
            _orders = new List<OrderViewModel>();
            _order = new OrderViewModel();
            _userManager = userManager;
            _orderService = orderService;
            _productService = productService;
            _orderDetailService = orderDetailService;
            _paymentFormService = paymentFormService;
            _orderPaymentService = orderPaymentService;
            _userService = userService;
            _statusService = statusOrderService;
            _cartDetailService = cartDetailService;
            _promotionService = promotionService;
            _userPromotionService = userPromotionService;

            _productPreviewService = new ProductPreviewService(_productService, _orderDetailService, _orderService);
        }

        private Task<Userr> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }
        // GET: OrderController
        public async Task<IActionResult> Index()
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                _orders = (await _orderService.GetAll()).Where(x => x.Id_User == user.Id).ToList();
                return View(_orders);
            }
            else
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
        }

        // GET: OrderController/Details/5

        public async Task<IActionResult> OrderDetails(int id)
        {
            _order = await _orderService.GetById(id);
            var details = await _orderDetailService.GetByOrder(id);
            ViewBag.Details = details;
            foreach (var item in details)
            {
                _order.Total += item.Quantity * item.Price;
            }
            if (_order.Id_Promotion != null)
            {
                var promotion = await _promotionService.GetById(Convert.ToInt32(_order.Id_Promotion));
                if (promotion.PercentReduct != null)
                {
                    _order.Total -= Convert.ToInt32(Math.Floor(Convert.ToDouble((_order.Total / 100) * promotion.PercentReduct)));
                }
                else _order.Total -= Convert.ToInt32(promotion.AmountReduct);
            }
            if (_order.IsUsePoint)
            {
                _order.Total -= Convert.ToInt32(_order.PointAmount);
            }
            return View(_order);
        }

        // GET: OrderController/Create
        public async Task<IActionResult> CreateOnlineOrder(int id, int quantity)
        {
            var createModel = new OrderViewModel();
            createModel.orderDetails = new List<OrderDetailViewModel>();
            var orderdetail = new OrderDetailViewModel();
            var user = await GetCurrentUserAsync();
            // kiem tra login
            if (user != null)
            {
                createModel.NameUser = user.Name;
                createModel.Email = user.Email;
                createModel.Phone = user.PhoneNumber;
                createModel.Id_User = user.Id;
                ViewBag.UserPromotions = await _userPromotionService.GetByUser(user.Id);
                ViewBag.User = await _userService.GetById(user.Id);
            }
            createModel.paymentsId = new List<int>();
            // mua trong gio hang
            if (id == 0)
            {
                List<CartDetailViewModel> cd = new List<CartDetailViewModel>();
                if (user != null) // non-account
                {
                    cd = await _cartDetailService.GetByCart(user.Id);
                }
                else
                {
                    var customCartChar = HttpContext.Session.GetString("sessionCart");
                    if (!string.IsNullOrEmpty(customCartChar))
                    {
                        cd = JsonConvert.DeserializeObject<List<CartDetailViewModel>>(customCartChar);
                    }
                }
                // duyệt sản phẩm
                foreach (var prod in cd)
                {
                    var product = await _productService.GetById(prod.Id_Product);
                    foreach (var item in product.bookViewModels)
                    {
                        createModel.Weight += (item.Weight * prod.Quantity);
                        createModel.Width += (item.Widght * prod.Quantity);
                        createModel.Length += (item.Length * prod.Quantity);
                        createModel.Height += (item.Height * prod.Quantity);
                    }
                    orderdetail = new OrderDetailViewModel()
                    {
                        NameProduct = product.Name,
                        Img = product.imageViewModels.FirstOrDefault().ImageUrl,
                        Id_Product = product.Id,
                        Price = product.Price,
                        Quantity = prod.Quantity,
                    };
                    createModel.orderDetails.Add(orderdetail);
                }

            }
            else // mua sản phẩm chi định
            {
                var product = await _productService.GetById(id);
                foreach (var item in product.bookViewModels)
                {
                    createModel.Weight += item.Weight * quantity;
                    createModel.Width += item.Widght;
                    createModel.Length += item.Length;
                    createModel.Height += item.Height * quantity;
                }
                orderdetail = new OrderDetailViewModel()
                {
                    NameProduct = product.Name,
                    Img = product.imageViewModels.FirstOrDefault().ImageUrl,
                    Id_Product = product.Id,
                    Price = product.Price,
                    Quantity = quantity,
                };
                createModel.orderDetails.Add(orderdetail);
            }
            createModel.Weight = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(createModel.Weight / 1000)));
            createModel.Width = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(createModel.Width / 100)));
            createModel.Length = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(createModel.Length / 100)));
            createModel.Height = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(createModel.Height / 100)));

            ViewBag.Payments = (await _paymentFormService.GetAll()).Where(x => x.Status == 1).ToList();
            ViewBag.Promotions = (await _promotionService.GetAll()).Where(x => x.Status == 1).ToList();
            return View(createModel);
        }

        // POST: OrderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOnlineOrder(OrderViewModel request)
        {
            try
            {
                //return Ok();
                UserModel user = new UserModel();
                if (request.Id_User == 0)
                {
                    user = (await _userService.GetAll()).Where(x => x.Email.Equals(request.Email)).FirstOrDefault();
                    if (user == null)
                    {
                        user = (await _userService.GetAll()).Where(x => x.Code.Equals("KH0000000")).FirstOrDefault();
                    }
                    request.Id_User = user.Id;
                }
                request.ModifiDate = DateTime.Now;
                request.ModifiNotes = DateTime.Now + " : Đơn hàng được tạo\n";
                request.Id_Status = (await _statusService.GetAll()).Where(x => x.Status == 1).FirstOrDefault().Id; // hóa đơn chờ
                request.IsOnlineOrder = true;
                request.IsUsePoint = request.PointUsed > 0 ? false : true;
                // return Ok(request);
                var result = await _orderService.Add(request);
                if (result.Id != 0)
                {
                    foreach (var item in request.paymentsId)
                    {
                        var op = new CreateOrderPaymentModel()
                        {
                            Id_Order = result.Id,
                            Id_Payment = item,
                            paymentAmount = Convert.ToInt32(request.Total + request.Shipfee),
                            Status = 0,
                        };
                        await _orderPaymentService.Add(op);
                    }
                    await _productPreviewService.ChangeQuantity(result.Id, -1);
                    return RedirectToAction("OrderDetails", new { id = result.Id });
                }
                return BadRequest();
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET: OrderController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            return View();
        }

        // POST: OrderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormCollection collection)
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

        // GET: OrderController/Delete/5
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _orderService.GetById(id);
            bool result = false;
            var statusId = (await _statusService.GetAll()).Where(x => x.Status == 8).FirstOrDefault().Id;
            order.Id_Status = statusId;
            order.ModifiDate = DateTime.Now;
            order.ModifiNotes = order.ModifiNotes + "\n" + DateTime.Now + " : Đơn hàng được hủy bởi khách hàng\n";
            result = await _orderService.Update(order);
            if (result)
            {
                // tang lại so luong sp
                await _productPreviewService.ChangeQuantity(id, 1);
                return Json(new { success = true });
            }
            else return Json(new { success = false });
        }

    }
}
