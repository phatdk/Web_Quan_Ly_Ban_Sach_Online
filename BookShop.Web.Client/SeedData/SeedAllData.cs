using BookShop.DAL.Entities.Identity;
using BookShop.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System.Drawing.Drawing2D;
using BookShop.DAL.Repositopy;
using BookShop.BLL.IService;
using BookShop.BLL.ConfigurationModel.PaymentFormModel;

public static class SeedDataMD
{
	public static async Task SeedAsync(UserManager<Userr> userManager, RoleManager<Role> roleManager)
	{
		// Tạo role "admin" nếu chưa tồn tại
		if (!await roleManager.RoleExistsAsync("Admin"))
		{
			var adminRole = new Role() { Name = "Admin" };
			var adminRole1 = new Role() { Name = "Staff" };
			var adminRole2 = new Role() { Name = "Customer" };
			await roleManager.CreateAsync(adminRole);
			await roleManager.CreateAsync(adminRole1);
			await roleManager.CreateAsync(adminRole2);
		}

		// Tạo tài khoản "admin" nếu chưa tồn tại
		if (await userManager.FindByNameAsync("admin") == null)
		{
			var adminUser = new Userr
			{
				UserName = "admin",
				Email = "phuc2003zgt@gmail.com",
				EmailConfirmed = true,
				Code = "AD0000000",
				Name = "Admin",
				Status = 2,
			};
			var User = new Userr
			{
				UserName = "customer",
				Email = "",
				EmailConfirmed = true,
				Code = "KH0000000",
				Name = "Khách vẵng lai",
				Status = 2,
			};

			var result = await userManager.CreateAsync(adminUser, "admin");
			var result1 = await userManager.CreateAsync(User, "123");
			if (result.Succeeded && result1.Succeeded)
			{
				// Gán role "admin" cho tài khoản "admin"
				await userManager.AddToRoleAsync(adminUser, "Admin");
				await userManager.AddToRoleAsync(User, "Customer");
			}
		}

	}

	public static async Task SeedDataStatus(IStatusOrderService service)
	{
		var status = new StatusOrder()
		{
			Status = 1,
			StatusName = "Chờ xử lý",
			CreatedDate = DateTime.Now,
		}; var status1 = new StatusOrder()
		{
			Status = 2,
			StatusName = "Đã xác nhận",
			CreatedDate = DateTime.Now,
		}; var status2 = new StatusOrder()
		{
			Status = 3,
			StatusName = "Đang giao",
			CreatedDate = DateTime.Now,
		}; var status3 = new StatusOrder()
		{
			Status = 4,
			StatusName = "Hoàn thành",
			CreatedDate = DateTime.Now,
		}; var status4 = new StatusOrder()
		{
			Status = 5,
			StatusName = "Trả hàng",
			CreatedDate = DateTime.Now,
		}; var status5 = new StatusOrder()
		{
			Status = 6,
			StatusName = "Xử lý hàng trả",
			CreatedDate = DateTime.Now,
		}; var status6 = new StatusOrder()
		{
			Status = 7,
			StatusName = "Trả hàng thành công",
			CreatedDate = DateTime.Now,
		}; var status7 = new StatusOrder()
		{
			Status = 8,
			StatusName = "Hủy đơn",
			CreatedDate = DateTime.Now,
		}; var status8 = new StatusOrder()
		{
			Status = 9,
			StatusName = "Đóng đơn",
			CreatedDate = DateTime.Now,
		}; var status9 = new StatusOrder()
		{
			Status = 0,
			StatusName = "Đơn chờ",
			CreatedDate = DateTime.Now,
		};


		await service.Add(status);
		await service.Add(status1);
		await service.Add(status2);
		await service.Add(status3);
		await service.Add(status4);
		await service.Add(status5);
		await service.Add(status6);
		await service.Add(status7);
		await service.Add(status8);
		await service.Add(status9);

	}
	public static async Task SeedDataPayment(IPaymentFormService service)
	{
		var form = new CreatePaymentFormModel()
		{
			Name = "Thanh toán tiền mặt tại quầy",
			Status = 2,
		};
		var form1 = new CreatePaymentFormModel()
		{
			Name = "Thanh toán banking tại quầy",
			Status = 2,
		};
		var form2 = new CreatePaymentFormModel()
		{
			Name = "Thanh toán khi nhận hàng",
			Status = 1,
		};
		var form3 = new CreatePaymentFormModel()
		{
			Name = "Thanh toán qua banking",
			Status = 1,
		};

		await service.Add(form);
		await service.Add(form1);
		await service.Add(form2);
		await service.Add(form3);
	}
}