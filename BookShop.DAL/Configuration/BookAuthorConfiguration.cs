﻿using BookShop.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DAL.Configuration
{
	public class BookAuthorConfiguration : IEntityTypeConfiguration<BookAuthor>
	{
		public void Configure(EntityTypeBuilder<BookAuthor> builder)
		{
			builder.HasOne(x=>x.Book).WithMany(x=>x.BookAuthors).HasForeignKey(x=>x.Id_Book).OnDelete(DeleteBehavior.Cascade);
			builder.HasOne(x=>x.Author).WithMany(x=>x.BookAuthors).HasForeignKey(x=>x.Id_Author).OnDelete(DeleteBehavior.Cascade);
		}
	}
}
