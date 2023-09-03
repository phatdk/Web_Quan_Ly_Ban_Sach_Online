using BookShop.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DAL.ApplicationDbContext
{
	public class ApplicationDbcontext : DbContext
	{
		public ApplicationDbcontext() { }
		public virtual DbSet<Admin> Admins { get; set; }
		public virtual DbSet<Author> Authors { get; set; }
		public virtual DbSet<Book> Books { get; set; }
		public virtual DbSet<BookAuthor> BookAuthors { get; set; }
		public virtual DbSet<Cart> Carts { get; set; }
		public virtual DbSet<CartDetail> CartDetails { get; set; }
		public virtual DbSet<Category> Categories { get; set; }
		public virtual DbSet<CollectionBook> CollectionBooks { get; set; }
		public virtual DbSet<CustomProperties> CustomProperties { get; set; }
		public virtual DbSet<Evaluate> Evaluates { get; set; }
		public virtual DbSet<Genre> Genres { get; set; }
		public virtual DbSet<Image> Images { get; set; }
		public virtual DbSet<News> News { get; set; }
		public virtual DbSet<Order> Orders { get; set; }
		public virtual DbSet<OrderDetail> OrderDetails { get; set; }
		public virtual DbSet<OrderPayment> OrderPayments { get; set; }
		public virtual DbSet<PaymentForm> PaymentForms { get; set; }
		public virtual DbSet<PointTransactionsHistory> PointTransactionsHistories { get; set; }
		public virtual DbSet<Product> Products { get; set; }
		public virtual DbSet<ProductBook> ProductBooks { get; set; }
		public virtual DbSet<ProductPromotion> ProductPromotions { get; set; }
		public virtual DbSet<Promotion> Promotions { get; set; }
		public virtual DbSet<PromotionType> PromotionTypes { get; set; }
		public virtual DbSet<PropertyValue> PropertyValues { get; set; }
		public virtual DbSet<ReturnOrder> ReturnOrders { get; set; }
		public virtual DbSet<Shop> Shops { get; set; }
		public virtual DbSet<Supplier> Suppliers { get; set; }
		public virtual DbSet<User> Users { get; set; }
		public virtual DbSet<UserPromotion> UserPromotions { get; set; }
		public virtual DbSet<WalletPoint> WalletPoints { get; set; }
		public virtual DbSet<WishList> WishLists { get; set; }
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseSqlServer("Data Source=Duy;Initial Catalog=BookStore;Integrated Security=False;Persist Security Info=False;User ID=Duy;Password=12345;Trust Server Certificate=True");

            }
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
		}

	}
}
