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
	public class PromotionConfiguration : IEntityTypeConfiguration<Promotion>
	{
		public void Configure(EntityTypeBuilder<Promotion> builder)
		{
			builder.Property(x => x.Name).HasColumnType("nvarchar(50)");
			builder.Property(x => x.Code).HasColumnType("nvarchar(13)");
			builder.Property(x => x.Description).HasColumnType("nvarchar(255)");
			builder.Property(x => x.EndDate).IsRequired(false);
			builder.Property(x=>x.StorageTerm).IsRequired(false);
			builder.Property(x=>x.Condition).IsRequired(false);
			builder.Property(x=>x.ConversionPoint).IsRequired(false);
			builder.Property(x=>x.AmountReduct).IsRequired(false);
			builder.Property(x=>x.PercentReduct).IsRequired(false);
			builder.Property(x=>x.ReductMax).IsRequired(false);

			builder.HasOne(x=>x.PromotionType).WithMany(x=>x.Promotions).HasForeignKey(x=>x.Id_Type).OnDelete(DeleteBehavior.NoAction);
		}
	}
}
