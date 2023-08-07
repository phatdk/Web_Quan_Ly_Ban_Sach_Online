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
	public class CustomPropertiesConfiguration : IEntityTypeConfiguration<CustomProperties>
	{
		public void Configure(EntityTypeBuilder<CustomProperties> builder)
		{
			builder.Property(x => x.propertyName).HasColumnType("varchar(50)");

			builder.HasOne(x => x.Shop).WithMany(x => x.CustomProperties).HasForeignKey(x => x.Id_Shop).OnDelete(DeleteBehavior.Cascade);
		}
	}
}
