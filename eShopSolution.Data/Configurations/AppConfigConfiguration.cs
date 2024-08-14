using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Data.Configurations
{
	public class AppConfigConfiguration : IEntityTypeConfiguration<AppConfig>
	{
		public void Configure(EntityTypeBuilder<AppConfig> builder)
		{
			builder.ToTable("AppConfigs");
			builder.HasKey(t => t.Key);
			builder.Property(t => t.Value)
				.IsRequired();
		}
	}
}
