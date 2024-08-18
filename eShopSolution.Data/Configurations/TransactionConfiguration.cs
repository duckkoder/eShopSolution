using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("Transactions");

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.AppUser).WithMany(x => x.Transactions).HasForeignKey(x=>x.UserId);

            builder.Property(x => x.Id).UseIdentityColumn();
        }
    }
}
