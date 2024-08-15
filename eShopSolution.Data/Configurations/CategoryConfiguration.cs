using eShopSolution.Data.Entities;
using eShopSolution.Data.enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Data.Configurations
{
	public class CategoryConfiguration : IEntityTypeConfiguration<Category>
	{
		public void Configure(EntityTypeBuilder<Category> builder)
		{
			builder.ToTable("Category");
			builder.HasKey(c => c.Id);
			builder.Property(c  => c.SortOrder).IsRequired();
			builder.Property(c => c.IsShowOnHome).IsRequired().HasDefaultValue(false);
			builder.Property(c=> c.Status).IsRequired().HasDefaultValue(Status.Active);
		}
	}
}
