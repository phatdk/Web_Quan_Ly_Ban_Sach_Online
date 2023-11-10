using BookShop.BLL.Options;

namespace BookShop.Web.Client
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddWebDependency(this IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<VnPayOption>(configuration.GetSection("PaymentConfig:VnPay"));			 
			return services;
		}
	}
}
