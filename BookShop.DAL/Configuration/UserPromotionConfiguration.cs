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
	public class UserPromotionConfiguration : IEntityTypeConfiguration<UserPromotion>
	{
		public void Configure(EntityTypeBuilder<UserPromotion> builder)
		{
			builder.Property(x => x.EndDate).IsRequired(false);

			builder.HasOne(x=>x.User).WithMany(x=>x.UserPromotions).HasForeignKey(x=>x.Id_User).OnDelete(DeleteBehavior.Cascade);
			builder.HasOne(x=>x.Promotion).WithMany(x=>x.UserPromotions).HasForeignKey(x=>x.Id_Promotion).OnDelete(DeleteBehavior.Cascade);
		}
	}
}
