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
	public class ShiftChangeConfiguration : IEntityTypeConfiguration<ShiftChange>
	{
		public void Configure(EntityTypeBuilder<ShiftChange> builder)
		{
			builder.Property(x => x.Code).HasColumnType("varchar(100)");
			builder.Property(x => x.EndTime).IsRequired(false);
			builder.Property(x => x.IntialAmount).IsRequired(false);
			builder.Property(x => x.TotalMoneyEarn).IsRequired(false);
			builder.Property(x => x.TotalCash).IsRequired(false);
			builder.Property(x => x.TotalCredit).IsRequired(false);
			builder.Property(x => x.CostIncurred).IsRequired(false);
			builder.Property(x => x.IncurredNote).HasColumnType("nvarchar(256)").IsRequired(false);
			builder.Property(x => x.TotalCashPreShift).IsRequired(false);
			builder.Property(x => x.ResetTime).IsRequired(false);
			builder.Property(x => x.TotalWithDrawn).IsRequired(false);
			builder.Property(x => x.Note).HasColumnType("nvarchar(256)").IsRequired(false);

			builder.HasOne(x=>x.WorkShift).WithMany(x=>x.ShiftChanges).HasForeignKey(x=>x.Id_Shift).OnDelete(DeleteBehavior.NoAction);
			builder.HasOne(x=>x.UserIn).WithMany(x=>x.ShiftChangesIn).HasForeignKey(x=>x.Id_UserInShift).OnDelete(DeleteBehavior.NoAction).IsRequired(false);
			builder.HasOne(x=>x.UserNx).WithMany(x=>x.ShiftChangesNx).HasForeignKey(x=>x.Id_UserNxShift).OnDelete(DeleteBehavior.NoAction).IsRequired(false);
			builder.HasOne(x=>x.UserReset).WithMany(x=>x.ShiftChangesReset).HasForeignKey(x=>x.Id_UserReset).OnDelete(DeleteBehavior.NoAction).IsRequired(false);
		}
	}
}
