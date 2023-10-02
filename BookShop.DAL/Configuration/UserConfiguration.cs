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
	public class UserConfiguration : IEntityTypeConfiguration<Userr>
	{
		public void Configure(EntityTypeBuilder<Userr> builder)
		{
			builder.Property(x => x.Name).HasColumnType("nvarchar(50)");
			builder.Property(x => x.Birth).IsRequired(false);
			builder.Property(x => x.Gender).IsRequired(false);
			builder.Property(x => x.Email).HasColumnType("varchar(50)").IsRequired(false);
			//builder.Property(x => x.Phone).HasColumnType("varchar(13)").IsRequired(false);
			builder.Property(x => x.UserName).HasColumnType("varchar(50)");
			//builder.Property(x => x.Password).HasColumnType("varchar(50)");
		}
	}
}
