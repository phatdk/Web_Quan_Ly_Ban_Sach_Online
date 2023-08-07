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
	public class ReturnOrderConfiguration : IEntityTypeConfiguration<ReturnOrder>
	{
		public void Configure(EntityTypeBuilder<ReturnOrder> builder)
		{
			builder.Property(x => x.Notes).HasColumnType("nvarchar(255)");

			builder.HasOne(x => x.Order).WithMany(x=>x.ReturnOrders).HasForeignKey(x=>x.Id_Order).OnDelete(DeleteBehavior.Cascade);
		}
	}
}
