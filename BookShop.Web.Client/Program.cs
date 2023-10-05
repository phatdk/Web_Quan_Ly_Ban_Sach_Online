using BookShop.DAL.Entities.Identity;
using BookShop.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using BookShop.DAL.ApplicationDbContext;
using Microsoft.AspNetCore.Builder;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

#region Gửi email
builder.Services.AddOptions();
var _Mailsetting = builder.Configuration.GetSection("MailSettings");
builder.Services.Configure<MailSettings>(_Mailsetting);
builder.Services.AddSingleton<IEmailSender, SendMailService>();
#endregion
#region service #
builder.Services.AddHttpClient();
builder.Services.AddRazorPages();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<ApplicationDbcontext>();
#endregion
#region Service Identity

builder.Services.AddIdentity<Userr, Role>()
   .AddEntityFrameworkStores<ApplicationDbcontext>()
   .AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(op =>
{
    // đăng nhập
    op.LoginPath = "/login/";
    //đăng xuất
    op.LogoutPath = "/logout/";
    // cấm truy cập
    op.AccessDeniedPath = "/accessdenied";

});


builder.Services.Configure<IdentityOptions>(options =>
{
    // Thiết lập về Password
    options.Password.RequireDigit = false; // Không bắt phải có số
    options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
    options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
    options.Password.RequireUppercase = false; // Không bắt buộc chữ in
    options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
    options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt


    // Cấu hình Lockout - khóa user
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1); // Khóa 5 phút
    options.Lockout.MaxFailedAccessAttempts = 3; // Thất bại 5 lầ thì khóa
    options.Lockout.AllowedForNewUsers = true;

    // Cấu hình về User.
    // options.User.AllowedUserNameCharacters =
    //"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+!$";
    options.User.RequireUniqueEmail = false;  // Email là duy nhất

    // Cấu hình đăng nhập.
    // Cấu hình xác thực địa chỉ email (email phải tồn tại)
    options.SignIn.RequireConfirmedEmail = true;
    // Xác thực số điện thoại
    options.SignIn.RequireConfirmedPhoneNumber = false;
    // người dùng phải confirmemail để đăng nhập
    options.SignIn.RequireConfirmedAccount = true;
});
builder.Services.AddDistributedMemoryCache();           // Đăng ký dịch vụ lưu cache trong bộ nhớ (Session sẽ sử dụng nó)
builder.Services.AddSession(op =>
{
    // setting timeout 20s cho hệ thống
    op.IdleTimeout = TimeSpan.FromSeconds(60);
    op.Cookie.Name = "Phucdepzai";
});
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddAuthorization(op =>
op.AddPolicy("ManagerMenu", builder =>
{
    builder.RequireAuthenticatedUser();
    builder.RequireRole("Admin");
}));

#endregion
builder.Services.AddControllersWithViews();
// Configure the HTTP request pipeline.
var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseStatusCodePages(er =>
{
    er.Run(async context =>
    {
        var respone = context.Response;
        var code = respone.StatusCode;
        var content = $@"<html>
    <head
        <meta charset='utf-8' />
        <title> Bug {code} </title>
        
    </head>
    <body>
        <p>
        co loi xay ra :{code}-{(HttpStatusCode)code}
          </p>
    </body>
</html>";
        await respone.WriteAsync(content);
    });
});
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "area",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
