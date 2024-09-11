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
	public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
	{
		public void Configure(EntityTypeBuilder<ProductImage> builder)
		{
			builder.ToTable("ProductImages");

			builder.HasKey(x => x.Id);

			builder.HasOne(x=> x.product).WithMany(x=>x.productImages).HasForeignKey(x=>x.ProductId);

			builder.Property(x=> x.ImagePath).HasMaxLength(255).IsRequired();

			builder.Property(x => x.Caption).HasMaxLength(255);
		}
	}
}
