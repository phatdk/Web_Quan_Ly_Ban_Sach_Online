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
	public class ImageConfiguration : IEntityTypeConfiguration<Image>
	{
		public void Configure(EntityTypeBuilder<Image> builder)
		{
			builder.Property(x => x.ImageUrl).HasColumnType("varchar(50)");

			builder.HasOne(x=>x.Product).WithMany(x=>x.Images).HasForeignKey(x=>x.Id_Product).OnDelete(DeleteBehavior.Cascade);
		}
	}
}
