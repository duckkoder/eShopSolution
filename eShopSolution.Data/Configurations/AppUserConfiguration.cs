using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Data.Configurations
{
	public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
	{
		public void Configure(EntityTypeBuilder<AppUser> builder)
		{
			builder.ToTable("AppUsers");

			builder.HasKey(x => x.Id);

			builder.Property(x => x.FirstName).IsRequired().HasMaxLength(200);

			builder.Property(x => x.LastName).IsRequired().HasMaxLength(200);

			builder.Property(x => x.Dob).IsRequired();
		}
	}
}
