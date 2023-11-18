 
using BookShop.BLL.Options;
using Microsoft.AspNetCore.Http;

namespace BookShop.BLL.IService;

public interface IVNPayService
{
	ResponseUriModel CreatePayment(PaymentInfoModel model, HttpContext context);

	PaymentResponseModel PaymentExecute(IQueryCollection collection);
}