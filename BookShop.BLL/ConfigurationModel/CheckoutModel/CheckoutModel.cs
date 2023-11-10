

using BookShop.BLL.ConfigurationModel.CartDetailModel;

namespace BookShop.BLL.ConfigurationModel.CheckoutModel;

public class CheckoutModel
{
    public int Id_User { get; set; }
    public string PaymentMethod { get; set; }
    public List<CartDetailViewModel> Carts { get; set; }
    public string CustomerName { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public string Note { get; set; }
    public int PaymentId { get; set; }
 
}