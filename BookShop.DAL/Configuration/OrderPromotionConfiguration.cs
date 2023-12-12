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
	public class OrderPromotionConfiguration : IEntityTypeConfiguration<OrderPromotion>
	{
		public void Configure(EntityTypeBuilder<OrderPromotion> builder)
		{
			builder.HasOne(x => x.Order).WithMany(x => x.OrderPromotions).HasForeignKey(x => x.Id_Order).OnDelete(DeleteBehavior.Cascade);
			builder.HasOne(x => x.Promotion).WithMany(x => x.OrderPromotions).HasForeignKey(x => x.Id_Promotion).OnDelete(DeleteBehavior.NoAction);
		}
	}
}
