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
	public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
	{
		public void Configure(EntityTypeBuilder<OrderDetail> builder)
		{
			builder.HasOne(x=>x.Order).WithMany(x=>x.OrderDetails).HasForeignKey(x=>x.Id_Order).OnDelete(DeleteBehavior.Cascade);
			builder.HasOne(x=>x.Product).WithMany(x=>x.OrderDetails).HasForeignKey(x=>x.Id_Product).OnDelete(DeleteBehavior.NoAction);
		}
	}
}
