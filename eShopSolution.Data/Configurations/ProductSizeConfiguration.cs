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
    public class ProductSizeConfiguration : IEntityTypeConfiguration<ProductSize>
    {
        public void Configure(EntityTypeBuilder<ProductSize> builder)
        {
            // Đặt khóa chính cho bảng ProductSize
            builder.HasKey(ps => new { ps.ProductId, ps.SizeId });

            // Cấu hình quan hệ với bảng Product
            builder.HasOne(ps => ps.Product)
                .WithMany(p => p.ProductSizes)
                .HasForeignKey(ps => ps.ProductId)
                .OnDelete(DeleteBehavior.Cascade);  

            // Cấu hình quan hệ với bảng Size
            builder.HasOne(ps => ps.Size)
                .WithMany(s => s.ProductSizes)
                .HasForeignKey(ps => ps.SizeId)
                .OnDelete(DeleteBehavior.Cascade);  // Thêm hành vi xóa, có thể tùy chỉnh

            // Cấu hình thêm nếu cần
            builder.Property(ps => ps.Quantity)
                .IsRequired()
                .HasDefaultValue(0); // Số lượng mặc định là 0
        }
    }

}
