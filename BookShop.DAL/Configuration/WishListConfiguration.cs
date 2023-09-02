using BookShop.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DAL.Configuration
{
	public class WishListConfiguration : IEntityTypeConfiguration<WishList>
	{
		public void Configure(EntityTypeBuilder<WishList> builder)
		{
			builder.HasKey(x => new { x.Id_Product, x.Id_User });
			builder.HasOne(x=>x.User).WithMany(x=>x.WishLists).HasForeignKey(x=>x.Id_User).OnDelete(DeleteBehavior.Cascade);
			builder.HasOne(x=>x.Product).WithMany(x=>x.WishLists).HasForeignKey(x=>x.Id_Product).OnDelete(DeleteBehavior.Cascade);
		}
	}
}
