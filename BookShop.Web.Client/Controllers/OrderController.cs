using BookShop.BLL.ConfigurationModel.OrderDetailModel;
using BookShop.BLL.ConfigurationModel.OrderModel;
using BookShop.BLL.ConfigurationModel.ProductModel;
using BookShop.BLL.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Web.Client.Controllers
{
	public class OrderController : Controller
	{
		public OrderViewModel _order;
		public List<OrderViewModel> _orders;
		public List<OrderDetailViewModel> _details;
		public List<ProductViewModel> _products;

		private readonly IOrderService _orderService;
		private readonly IProductService _productService;
		private readonly IOrderDetailService _orderDetailService;
		private readonly IPaymentFormService _paymentFormService;

		public OrderController(IOrderService orderService, IProductService productService, IOrderDetailService orderDetailService, IPaymentFormService paymentFormService)
		{
			_order = new OrderViewModel();
			_orders = new List<OrderViewModel>();
			_details = new List<OrderDetailViewModel>();
			_products = new List<ProductViewModel>();

			_orderService = orderService;
			_productService = productService;
			_orderDetailService = orderDetailService;
			_paymentFormService = paymentFormService;
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
		public async Task<IActionResult> CreateOnlineOrder(int id)
		{
			var product = await _productService.GetById(id);
			var createModel = new CreateOrderModel();
			foreach(var item in product.bookViewModels)
			{
				createModel.Weight += item.Weight;
				createModel.Width += item.Widght;
				createModel.Length += item.Length;
				createModel.Height += item.Height;
			}
			
			createModel.Weight = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(createModel.Weight/1000)));
			createModel.Width = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(createModel.Width/100)));
			createModel.Length = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(createModel.Length/100)));
			createModel.Height = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(createModel.Height /100)));
			product.ImgUrl = product.imageViewModels.FirstOrDefault().ImageUrl;
			_products.Add(product);
			var payments = (await _paymentFormService.GetAll()).Where(x => x.Status == 1).ToList();

			ViewBag.Products = _products;
			ViewBag.Payments = payments;
			return View(createModel);
		}

		// POST: OrderController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateOnlineOrder(IFormCollection collection)
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
