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
	public class OrderPaymentConfiguration : IEntityTypeConfiguration<OrderPayment>
	{
		public void Configure(EntityTypeBuilder<OrderPayment> builder)
		{
			builder.HasOne(x=>x.Order).WithMany(x=>x.OrderPayments).HasForeignKey(x=>x.Id_Order).OnDelete(DeleteBehavior.Cascade);
			builder.HasOne(x=>x.PaymentForm).WithMany(x=>x.OrderPayments).HasForeignKey(x=>x.Id_Payment).OnDelete(DeleteBehavior.NoAction);
		}
	}
}
