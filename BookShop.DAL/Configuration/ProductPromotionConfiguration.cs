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
	public class ProductPromotionConfiguration : IEntityTypeConfiguration<ProductPromotion>
	{
		public void Configure(EntityTypeBuilder<ProductPromotion> builder)
		{
			builder.HasOne(x=>x.Product).WithMany(x=>x.ProductPromotions).HasForeignKey(x=>x.Id_Product).OnDelete(DeleteBehavior.Cascade);
			builder.HasOne(x=>x.Promotion).WithMany(x=>x.ProductPromotions).HasForeignKey(x=>x.Id_Promotion).OnDelete(DeleteBehavior.Cascade);
		}
	}
}
