using BookShop.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DAL.Configuration
{
	public class NewsConfiguration : IEntityTypeConfiguration<News>
	{
		public void Configure(EntityTypeBuilder<News> builder)
		{
			builder.Property(x => x.Title).HasColumnType("nvarchar(100)");
			builder.Property(x => x.Content).HasColumnType("nvarchar(max)");
			builder.Property(x => x.Description).HasColumnType("nvarchar(255)").IsRequired(false);
            builder.Property(x => x.Img).HasColumnType("nvarchar(250)");
        }
	}
}
