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
	public class EvaluateConfiguration : IEntityTypeConfiguration<Evaluate>
	{
		public void Configure(EntityTypeBuilder<Evaluate> builder)
		{
			builder.Property(x => x.Point).IsRequired(false);
			builder.Property(x => x.Content).HasColumnType("nvarchar(255)").IsRequired(false);

			builder.HasOne(x=>x.User).WithMany(x=>x.Evaluates).HasForeignKey(x=>x.Id_User).OnDelete(DeleteBehavior.Cascade);
			builder.HasOne(x=>x.OrderDetail).WithMany(x=>x.Evaluates).HasForeignKey(x=>x.Id_Product).OnDelete(DeleteBehavior.Cascade);
			builder.HasOne(x=>x.Parents).WithMany(x=>x.Evaluates).HasForeignKey(x=>x.Id_Parents).OnDelete(DeleteBehavior.NoAction);
		}
	}
}
