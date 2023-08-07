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
	public class WalletPointConfiguration : IEntityTypeConfiguration<WalletPoint>
	{
		public void Configure(EntityTypeBuilder<WalletPoint> builder)
		{
			builder.HasOne(x => x.User).WithOne(x => x.WalletPoint).HasForeignKey<WalletPoint>(x => x.Id_User);
		}
	}
}
