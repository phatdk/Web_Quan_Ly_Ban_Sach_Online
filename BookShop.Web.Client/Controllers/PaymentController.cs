using BookShop.BLL.ConfigurationModel.CheckoutModel;
using BookShop.BLL.ConfigurationModel.PaymentFormModel;
using BookShop.BLL.IService;
using BookShop.BLL.Options;
using BookShop.BLL.Service;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Web.Client.Controllers
{
	public class PaymentController : Controller
	{
		private readonly IVNPayService _vnPayService;
		private readonly ICartDetailService _cartDetailService;
		private readonly IOrderService _orderService;

		public PaymentController(IVNPayService vnPayService, ICartDetailService cartDetailService, IOrderService orderService  )
		{
			_vnPayService = vnPayService;
			_cartDetailService = cartDetailService;
			_orderService = orderService;
		}

		[HttpPost]
		public async Task<IActionResult> ProcessCheckout([FromBody] CheckoutModel request)
		{
			var paymentCode = Guid.NewGuid().ToString().Split("-")[0];
			var carts = await _cartDetailService.GetByCart(request.Id_User);

			if (!(carts?.Count > 0)) return BadRequestResponse("Cart is empty");

			switch (request.PaymentMethod.ToLower())
			{
				case "vnpay":
					var responseUriVnPay = _vnPayService.CreatePayment(new PaymentInfoModel()
					{
						TotalAmount = (double)carts.First().TotalPrice,
						PaymentCode = paymentCode
					}, HttpContext);
					return SuccessResponse(responseUriVnPay.Uri);

				case "cash":
					return SuccessResponse($"/Home/PaymentCallback?success=true&paymentMethod=Cash");

				default:
					return BadRequestResponse("Invalid payment method");
			}
		}

		[NonAction]
		//trả về một ObjectResult chứa thông điệp thành công
		protected ActionResult SuccessResponse(string message)
		{
			return new ObjectResult(message);
		}
		[NonAction]
		//trả về một BadRequestObjectResult chứa thông điệp lỗi
		protected ActionResult BadRequestResponse(string message)
		{
			return new BadRequestObjectResult(message);
		}
	}
}