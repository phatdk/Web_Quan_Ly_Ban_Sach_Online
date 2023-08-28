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
	public class BookConfiguration : IEntityTypeConfiguration<Book>
	{
		public void Configure(EntityTypeBuilder<Book> builder)
		{
			builder.Property(x => x.ISBN).HasColumnType("varchar(20)").IsRequired(false);
			builder.Property(x => x.Title).HasColumnType("nvarchar(50)");
			builder.Property(x => x.Reader).HasColumnType("nvarchar(50)");
			builder.Property(x => x.Description).HasColumnType("nvarchar(255)");
			builder.Property(x => x.PageSize).HasColumnType("varchar(50)");
			builder.Property(x => x.Cover).HasColumnType("nvarchar(50)");
			builder.Property(x => x.PublicationDate).HasColumnType("varchar(50)");

			builder.HasOne(x=>x.CollectionBook).WithMany(x=>x.Books).HasForeignKey(x=>x.Id_Collection).OnDelete(DeleteBehavior.Restrict).IsRequired(false);
			builder.HasOne(x => x.Supplier).WithMany(x => x.Books).HasForeignKey(x => x.Id_Supplier).OnDelete(DeleteBehavior.Restrict);
		}
	}
}
