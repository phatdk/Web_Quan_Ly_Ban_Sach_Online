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
    public class UserShiftConfiguration : IEntityTypeConfiguration<UserShift>
    {
        public void Configure(EntityTypeBuilder<UserShift> builder)
        {
            builder.Property(x => x.Note).HasColumnType("nvarchar(256)");

            builder.HasOne(x=>x.User).WithMany(x=>x.UserShifts).HasForeignKey(x=>x.Id_User).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x=>x.WorkShift).WithMany(x=>x.UserShifts).HasForeignKey(x=>x.Id_Shift).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
