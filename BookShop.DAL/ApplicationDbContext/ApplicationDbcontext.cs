using BookShop.DAL.Entities;
using BookShop.DAL.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DAL.ApplicationDbContext
{
    public class ApplicationDbcontext : IdentityDbContext<Userr, Role, int, UserClaims, UserRoles, UserLogins, RoleClaims, UserTokens>
    {
        public ApplicationDbcontext() { }
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
        public virtual DbSet<OrderPromotion> OrderPromotions { get; set; }
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
        public virtual DbSet<UserPromotion> UserPromotions { get; set; }
        public virtual DbSet<WalletPoint> WalletPoints { get; set; }
        public virtual DbSet<WishList> WishLists { get; set; }
        public virtual DbSet<StatusOrder> StatusOrders { get; set; }
        public virtual DbSet<WorkShift> WorkShifts { get; set; }
        public virtual DbSet<UserShift> UserShifts { get; set; }
        public virtual DbSet<ShiftChange> ShiftChanges { get; set; }
        public override DbSet<Userr> Users { get; set; }
        public override DbSet<UserRoles> UserRoles { get; set; }
        public override DbSet<UserLogins> UserLogins { get; set; }
        public override DbSet<UserTokens> UserTokens { get; set; }
        public override DbSet<UserClaims> UserClaims { get; set; }
        public override DbSet<RoleClaims> RoleClaims { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-L9TSC4C\\SQLEXPRESS;Initial Catalog=BookShop.Datn;Integrated Security=True");
                //optionsBuilder.UseSqlServer(@"Data Source=PHUCDEPZAI2003\SQLEXPRESS;Initial Catalog=BookShopDatn;Integrated Security=True;Encrypt=False");

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)

        {
            modelBuilder.Entity<UserLogins>().HasKey(x=> new {x.LoginProvider,x.ProviderKey});
            modelBuilder.Entity<UserTokens>().HasKey(x=> new {x.LoginProvider,x.UserId,x.Name});
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entity.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entity.SetTableName(tableName.Substring(6));
                }

            }
        }
    }
}
