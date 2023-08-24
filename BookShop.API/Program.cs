
using BookShop.BLL.IService.IBookAuthorService;
using BookShop.BLL.IService.IBookService;
using BookShop.BLL.Service.BookAuthorService;
using BookShop.BLL.Service.BookService;
using BookShop.DAL.Entities;
using BookShop.DAL.Repositopy;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
//builder.Services.AddScoped<IAuthorService, AuthorService>();
//builder.Services.AddScoped<IRepository<Book>,Repository<Book>>();
builder.Services.AddScoped<IBookService, BookService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
