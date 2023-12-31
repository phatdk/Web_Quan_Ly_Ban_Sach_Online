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
	public class PropertyValueConfiguration : IEntityTypeConfiguration<PropertyValue>
	{
		public void Configure(EntityTypeBuilder<PropertyValue> builder)
		{
			builder.Property(x => x.Value).HasColumnType("nvarchar(max)");

			builder.HasOne(x=>x.CustomProperties).WithMany(x=>x.PropertyValues).HasForeignKey(x=>x.Id_Property).OnDelete(DeleteBehavior.Cascade);
		}
	}
}
