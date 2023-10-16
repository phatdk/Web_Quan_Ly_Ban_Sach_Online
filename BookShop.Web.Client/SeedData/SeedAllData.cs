using BookShop.DAL.Entities.Identity;
using BookShop.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System.Drawing.Drawing2D;
using BookShop.DAL.Repositopy;

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
                Code ="AD0000000",
                Name ="Admin"
            };
            var User = new Userr
            {
                UserName = "phatdk",
                Email = "",
                EmailConfirmed = true,
                Code = "KH0000000",
                Name ="Khách vẵng lai"
            };

            var result = await userManager.CreateAsync(adminUser, "admin");
            if (result.Succeeded)
            {
                // Gán role "admin" cho tài khoản "admin"
                await userManager.AddToRoleAsync(adminUser, "Admin");
                await userManager.AddToRoleAsync(User, "Customer");
            }
        }

    }

    public static async Task SeedDataProduct(IRepository<StatusOrder> StatusOrder)
    {


        var status = new StatusOrder()
        {
            Status = 1,
            StatusName = "Chờ xác nhận",
            CreatedDate = DateTime.Now,
        };  var status1 = new StatusOrder()
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
            StatusName = "Giao thành công",
            CreatedDate = DateTime.Now,
        }; var status4 = new StatusOrder()
        {
            Status = 5,
            StatusName = "Thanh toán thành công",
            CreatedDate = DateTime.Now,
        };var status5 = new StatusOrder()
        {
            Status = 6,
            StatusName = "Hoàn thành",
            CreatedDate = DateTime.Now,
        };var status6 = new StatusOrder()
        {
            Status = 7,
            StatusName = "Trả hàng",
            CreatedDate = DateTime.Now,
        };var status7 = new StatusOrder()
        {
            Status = 8,
            StatusName = "Hủy đơn",
            CreatedDate = DateTime.Now,
        };var status8 = new StatusOrder()
        {
            Status = 9,
            StatusName = "Đóng đơn",
            CreatedDate = DateTime.Now,
        };


       await StatusOrder.CreateAsync(status);
       await StatusOrder.CreateAsync(status1);
       await StatusOrder.CreateAsync(status2);
       await StatusOrder.CreateAsync(status3);
       await StatusOrder.CreateAsync(status4);
       await StatusOrder.CreateAsync(status5);
       await StatusOrder.CreateAsync(status6);
       await StatusOrder.CreateAsync(status7);
       await StatusOrder.CreateAsync(status8);

    } 
}