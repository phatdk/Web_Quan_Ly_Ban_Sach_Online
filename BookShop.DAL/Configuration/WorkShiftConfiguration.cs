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
	public class WorkShiftConfiguration : IEntityTypeConfiguration<WorkShift>
	{
		public void Configure(EntityTypeBuilder<WorkShift> builder)
		{
			builder.Property(x => x.Shift).HasColumnType("nvarchar(100)");
			builder.Property(x => x.Time).HasColumnType("nvarchar(256)");
		}
	}
}
