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
	public class BookGenreConfiguration : IEntityTypeConfiguration<BookGenre>
	{
		public void Configure(EntityTypeBuilder<BookGenre> builder)
		{
			builder.HasOne(x=>x.Book).WithMany(x=>x.BookGenres).HasForeignKey(x=>x.Id_Book).OnDelete(DeleteBehavior.Cascade);
			builder.HasOne(x=>x.Genre).WithMany(x=>x.BookGenres).HasForeignKey(x=>x.Id_Genre).OnDelete(DeleteBehavior.Cascade);
		}
	}
}
