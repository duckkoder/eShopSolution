using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Data.Configurations
{
	public class OrderConfiguration : IEntityTypeConfiguration<Order>
	{
		public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Order> builder)
		{
			builder.ToTable("Orders");

			builder.HasKey(x => x.Id);

			builder.HasOne(x => x.AppUser).WithMany(x => x.Orders).HasForeignKey(x => x.UserId);

			builder.Property(x => x.Id).UseIdentityColumn();

			builder.Property(x => x.OrderDate).HasDefaultValue(DateTime.Now);

			builder.Property(x => x.ShipEmail).IsRequired().IsUnicode(false).HasMaxLength(50);

			builder.Property(x => x.ShipAddress).IsRequired().HasMaxLength(200);


			builder.Property(x => x.ShipName).IsRequired().HasMaxLength(200);


			builder.Property(x => x.ShipPhoneNumber).IsRequired().HasMaxLength(200);
		}
	}
}
