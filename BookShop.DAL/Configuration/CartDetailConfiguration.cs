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
	public class CartDetailConfiguration : IEntityTypeConfiguration<CartDetail>
	{
		public void Configure(EntityTypeBuilder<CartDetail> builder)
		{
			builder.HasOne(x=>x.Cart).WithMany(x=>x.CartDetails).HasForeignKey(x=>x.Id_User).OnDelete(DeleteBehavior.Cascade);
			builder.HasOne(x=>x.Product).WithMany(x=>x.CartDetails).HasForeignKey(x=>x.Id_Product).OnDelete(DeleteBehavior.Cascade);
		}
	}
}
