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
	public class OrderConfiguration : IEntityTypeConfiguration<Order>
	{
		public void Configure(EntityTypeBuilder<Order> builder)
		{
			builder.Property(x => x.Code).HasColumnType("varchar(13)");
			builder.Property(x => x.Receiver).HasColumnType("nvarchar(50)");
			builder.Property(x => x.Phone).HasColumnType("varchar(13)");

			builder.Property(x => x.AcceptDate).IsRequired(false);
			builder.Property(x => x.DeliveryDate).IsRequired(false);
			builder.Property(x => x.ReceiveDate).IsRequired(false);
			builder.Property(x => x.PaymentDate).IsRequired(false);
			builder.Property(x => x.CompleteDate).IsRequired(false);
			builder.Property(x => x.ModifiDate).IsRequired(false);
			builder.Property(x => x.PointUsed).IsRequired(false);
			builder.Property(x => x.PointAmount).IsRequired(false);
			builder.Property(x => x.City).IsRequired(false);
			builder.Property(x => x.District).IsRequired(false);
			builder.Property(x => x.Commune).IsRequired(false);

			builder.Property(x => x.ModifiNotes).HasColumnType("nvarchar(100)").IsRequired(false);
			builder.Property(x => x.Description).HasColumnType("nvarchar(255)").IsRequired(false);

			builder.HasOne(x => x.User).WithMany(x => x.Orders).HasForeignKey(x => x.Id_User).OnDelete(DeleteBehavior.NoAction);
			builder.HasOne(x => x.Promotion).WithMany(x => x.Orders).HasForeignKey(x => x.Id_Promotion).OnDelete(DeleteBehavior.NoAction);
			builder.HasOne(x => x.StatusOrder).WithMany(x => x.Orders).HasForeignKey(x => x.Id_Promotion).OnDelete(DeleteBehavior.NoAction);
		}
	}
}
