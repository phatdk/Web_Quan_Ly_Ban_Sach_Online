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
	public class ShopConfiguration : IEntityTypeConfiguration<Shop>
	{
		public void Configure(EntityTypeBuilder<Shop> builder)
		{
			builder.Property(x => x.ShopName).HasColumnType("nvarchar(50)");
			builder.Property(x => x.About).HasColumnType("nvarchar(max)");
		}
	}
}
