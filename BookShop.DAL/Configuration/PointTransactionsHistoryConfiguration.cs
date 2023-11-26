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
	public class PointTransactionsHistoryConfiguration : IEntityTypeConfiguration<PointTransactionsHistory>
	{
		public void Configure(EntityTypeBuilder<PointTransactionsHistory> builder)
		{
			builder.HasOne(x=>x.WalletPoint).WithMany(x=>x.PointTransactionsHistories).HasForeignKey(x=>x.Id_User).OnDelete(DeleteBehavior.Cascade);
			builder.HasOne(x=>x.Order).WithMany(x=>x.PointTransactionsHistories).HasForeignKey(x=>x.Id_Order).OnDelete(DeleteBehavior.NoAction).IsRequired(false);
			builder.HasOne(x=>x.Promotion).WithMany(x=>x.PointTransactionsHistories).HasForeignKey(x=>x.Id_Promotion).OnDelete(DeleteBehavior.NoAction).IsRequired(false);
		}
	}
}
