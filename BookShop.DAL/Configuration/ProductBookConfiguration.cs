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
	public class ProductBookConfiguration : IEntityTypeConfiguration<ProductBook>
	{
		public void Configure(EntityTypeBuilder<ProductBook> builder)
		{
			builder.HasOne(x=>x.Product).WithMany(x=>x.ProductBooks).HasForeignKey(x=>x.Id_Product).OnDelete(DeleteBehavior.Cascade);
			builder.HasOne(x=>x.Book).WithMany(x=>x.ProductBooks).HasForeignKey(x=>x.Id_BookDetail).OnDelete(DeleteBehavior.Cascade);
		}
	}
}
