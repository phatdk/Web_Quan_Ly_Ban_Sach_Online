﻿using BookShop.DAL.Entities;
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
				optionsBuilder.UseSqlServer("Data Source=DESKTOP-L9TSC4C\\SQLEXPRESS;Initial Catalog=BookShop.Datn;Integrated Security=True");

			}
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
			modelBuilder.Entity<Admin>().HasData(
					new Admin { Id = 0, Name = "Admin", Phone = "0000000000", Email = "example@gmail.com", Password = "1", Role = 0, CreatedDate = DateTime.Now, Status = 1 }
				);
			modelBuilder.Entity<User>().HasData(
					new User { Id = 0, Name = "Khách vẵng lai", UserName = "customer", Password = "1", CreatedDate = DateTime.Now, Status = 1 }
				);
			modelBuilder.Entity<Shop>().HasData(
				new Shop { Id = 0, ShopName = "Wild Rose", About = "Một số thông tin về shop" }
				);
			modelBuilder.Entity<CustomProperties>().HasData(
				new CustomProperties { Id = 0, propertyName = "Logo" },
				new CustomProperties { Id = 1, propertyName = "Banner" },
				new CustomProperties { Id = 2, propertyName = "Event banner" }
				);
			modelBuilder.Entity<PromotionType>().HasData(
				new PromotionType { Id = 0, Name = "Khuyến mại theo đơn" },
				new PromotionType { Id = 1, Name = "Khuyến mại theo sản phẩm" },
				new PromotionType { Id = 2, Name = "Khuyến mại đổi điểm" }
				);
		}
	}
}
