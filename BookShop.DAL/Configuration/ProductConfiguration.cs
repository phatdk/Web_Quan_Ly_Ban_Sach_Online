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
	public class ProductConfiguration : IEntityTypeConfiguration<Product>
	{
		public void Configure(EntityTypeBuilder<Product> builder)
		{
			builder.Property(x => x.Name).HasColumnType("nvarchar(100)");
			builder.Property(x => x.Description).HasColumnType("nvarchar(255)").IsRequired(false);

			builder.HasOne(x => x.CollectionBook).WithMany(x => x.Products).HasForeignKey(x => x.Id_Collection).OnDelete(DeleteBehavior.NoAction).IsRequired(false);
		}
	}
}
