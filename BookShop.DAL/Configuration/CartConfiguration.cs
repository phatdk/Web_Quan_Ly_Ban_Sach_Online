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
	public class CartConfiguration : IEntityTypeConfiguration<Cart>
	{
		public void Configure(EntityTypeBuilder<Cart> builder)
		{
			builder.HasOne(x => x.User).WithOne(x => x.Cart).HasForeignKey<Cart>(x => x.Id_User);
		}
	}
}
