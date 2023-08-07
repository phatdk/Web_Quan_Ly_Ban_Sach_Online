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
	public class AuthorConfiguration : IEntityTypeConfiguration<Author>
	{
		public void Configure(EntityTypeBuilder<Author> builder)
		{
			builder.Property(x => x.Name).HasColumnType("nvarchar(50)");
			builder.Property(x => x.Img).HasColumnType("varchar(50)").IsRequired(false);
		}
	}
}
