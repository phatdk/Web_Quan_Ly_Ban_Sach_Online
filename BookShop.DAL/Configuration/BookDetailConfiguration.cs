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
	public class BookDetailConfiguration : IEntityTypeConfiguration<BookDetail>
	{
		public void Configure(EntityTypeBuilder<BookDetail> builder)
		{
			builder.Property(x => x.PageSize).HasColumnType("varchar(50)");
			builder.Property(x => x.Cover).HasColumnType("nvarchar(50)");
			builder.Property(x => x.PublicationDate).HasColumnType("varchar(50)");

			builder.HasOne(x=>x.Supplier).WithMany(x=>x.BookDetails).HasForeignKey(x=>x.Id_Supplier).OnDelete(DeleteBehavior.Restrict);
			builder.HasOne(x=>x.Language).WithMany(x=>x.BookDetails).HasForeignKey(x=>x.Id_Language).OnDelete(DeleteBehavior.Restrict);
			builder.HasOne(x=>x.Book).WithMany(x=>x.BookDetails).HasForeignKey(x=>x.Id_Supplier).OnDelete(DeleteBehavior.Cascade);
		}
	}
}
