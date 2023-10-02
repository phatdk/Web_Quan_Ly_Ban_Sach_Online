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
	public class UserConfiguration : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.Property(x => x.Name).HasColumnType("nvarchar(50)");
			builder.Property(x => x.Birth).HasColumnType("date").IsRequired(false);
			builder.Property(x => x.Gender).IsRequired(false);
			builder.Property(x => x.Email).HasColumnType("varchar(50)").IsRequired(false);
			//builder.Property(x => x.Phone).HasColumnType("varchar(13)").IsRequired(false);
			builder.Property(x => x.UserName).HasColumnType("varchar(50)");
			//builder.Property(x => x.Password).HasColumnType("varchar(50)");
			builder.Property(x => x.Code).HasColumnType("varchar(100)");
			builder.Property(x => x.Img).HasColumnType("varchar(256)").IsRequired(false);
		}
	}
}
