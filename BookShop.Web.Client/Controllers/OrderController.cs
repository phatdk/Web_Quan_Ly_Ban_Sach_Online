﻿using BookShop.BLL.ConfigurationModel.CartDetailModel;
using BookShop.BLL.ConfigurationModel.OrderDetailModel;
using BookShop.BLL.ConfigurationModel.OrderModel;
using BookShop.BLL.ConfigurationModel.OrderPaymentModel;
using BookShop.BLL.ConfigurationModel.ProductModel;
using BookShop.BLL.ConfigurationModel.StatusOrderModel;
using BookShop.BLL.IService;
using BookShop.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BookShop.Web.Client.Controllers
{
    public class OrderController : Controller
    {
        public OrderViewModel _order;
        public List<OrderViewModel> _orders;
        public List<OrderDetailViewModel> _details;
        public List<ProductViewModel> _products;
        public List<StatusViewModel> _status;

        private readonly UserManager<Userr> _userManager;
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IPaymentFormService _paymentFormService;
        private readonly IOrderPaymentService _orderPaymentService;
        private readonly IUserService _userService;
        private readonly IStatusOrderService _statusService;
        private readonly ICartDetailService _cartDetailService;

        public OrderController(UserManager<Userr> userManager, IOrderService orderService, IProductService productService, IOrderDetailService orderDetailService, IPaymentFormService paymentFormService, IOrderPaymentService orderPaymentService, IUserService userService, IStatusOrderService statusOrderService, ICartDetailService cartDetailService)
        {
            _order = new OrderViewModel();
            _orders = new List<OrderViewModel>();
            _details = new List<OrderDetailViewModel>();
            _products = new List<ProductViewModel>();
            _status = new List<StatusViewModel>();

            _userManager = userManager;
            _orderService = orderService;
            _productService = productService;
            _orderDetailService = orderDetailService;
            _paymentFormService = paymentFormService;
            _orderPaymentService = orderPaymentService;
            _userService = userService;
            _statusService = statusOrderService;
            _cartDetailService = cartDetailService;
        }

        private Task<Userr> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }
        // GET: OrderController
        public async Task<IActionResult> Index()
        {
            return View();
        }

        // GET: OrderController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            return View();
        }

        // GET: OrderController/Create
        public async Task<IActionResult> CreateOnlineOrder(int id, int quantity)
        {
            var createModel = new CreateOrderModel();
            createModel.orderDetails = new List<CreateOrderDetailModel>();
            var orderdetail = new CreateOrderDetailModel();
            var user = await GetCurrentUserAsync();
            // kiem tra login
            if (user != null)
            {
                createModel.NameUser = user.Name;
                createModel.Email = user.Email;
                createModel.Phone = user.PhoneNumber;
                createModel.Id_User = user.Id;
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
                    var customCartChar = HttpContext.Session.GetString("customCart");
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
                    product.BuyQuantity = prod.Quantity;
                    product.ImgUrl = product.imageViewModels.FirstOrDefault().ImageUrl;
                    _products.Add(product); // sp hien thi
                    orderdetail = new CreateOrderDetailModel()
                    {
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
                    createModel.Width += item.Widght * quantity;
                    createModel.Length += item.Length * quantity;
                    createModel.Height += item.Height * quantity;
                }
                product.BuyQuantity = quantity;
                product.ImgUrl = product.imageViewModels.FirstOrDefault().ImageUrl;
                _products.Add(product);
                orderdetail = new CreateOrderDetailModel()
                {
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

            var payments = (await _paymentFormService.GetAll()).Where(x => x.Status == 1).ToList();
            ViewBag.Products = _products;
            ViewBag.Payments = payments;
            return View(createModel);
        }

        // POST: OrderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOnlineOrder(CreateOrderModel request)
        {
            try
            {
                if (request != null)
                {
                    var user = (await _userService.GetAll()).Where(x => x.Email == request.Email);
                    request.Id_StatusOrder = (await _statusService.GetAll()).Where(x => x.Status == 1).FirstOrDefault().Id; // hóa đơn chờ
                    var details = new List<CreateOrderDetailModel>();
                    foreach (var item in request.orderDetails)
                    {
                        var product = await _productService.GetById(item.Id_Product);
                        var deltail = new CreateOrderDetailModel()
                        {
                            Id_Product = product.Id,
                            Price = product.Price,
                            Quantity = 1
                        };
                        details.Add(deltail);
                    }
                    //return Ok(request);
                    var result = await _orderService.Add(request, request.orderDetails);
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
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return Ok();
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
        public async Task<IActionResult> Delete(int id)
        {
            return View();
        }

    }
}
