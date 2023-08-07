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
	public class AdminConfiguration : IEntityTypeConfiguration<Admin>
	{
		public void Configure(EntityTypeBuilder<Admin> builder)
		{
			builder.Property(x => x.Name).HasColumnType("nvarchar(50)");
			builder.Property(x => x.Phone).HasColumnType("varchar(13)");
			builder.Property(x => x.Email).HasColumnType("nvarchar(50)");
		}
	}
}
