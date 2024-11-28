using eShopSolution.Data.Configurations;
using eShopSolution.Data.Entities;
using eShopSolution.Data.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.EF
{
	public class EShopDbContext : IdentityDbContext<AppUser,AppRole,Guid>
	{
		public EShopDbContext(DbContextOptions options) : base(options)
		{

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			//Configure using fluent API
			modelBuilder.ApplyConfiguration(new CartConfiguration());
			modelBuilder.ApplyConfiguration(new AppConfigConfiguration());
			modelBuilder.ApplyConfiguration(new ProductConfiguration());
			modelBuilder.ApplyConfiguration(new CategoryConfiguration());
			modelBuilder.ApplyConfiguration(new ProductInCategoryConfiguration());
			modelBuilder.ApplyConfiguration(new OrderConfiguration());
			modelBuilder.ApplyConfiguration(new OrderDetailConfiguration());
			modelBuilder.ApplyConfiguration(new CategoryTranslationConfiguration());
			modelBuilder.ApplyConfiguration(new ContactConfiguration());
			modelBuilder.ApplyConfiguration(new LanguageConfiguration());
			modelBuilder.ApplyConfiguration(new ProductTranslationConfiguration());
			modelBuilder.ApplyConfiguration(new PromotionConfiguration());
			modelBuilder.ApplyConfiguration(new TransactionConfiguration());
			modelBuilder.ApplyConfiguration(new ProductImageConfiguration());
			modelBuilder.ApplyConfiguration(new BrandConfiguration());
			//configure for IdentityContext (Authentication)
			modelBuilder.ApplyConfiguration(new AppUserConfiguration());
			modelBuilder.ApplyConfiguration(new AppRoleConfiguration());

			modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("AppUserClaims");
			modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("AppUserRoles");
			modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("AppUserLogin");
			modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("AppRoleClaims");
			modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("AppUserTokens");

			// IdentityDbContext<TUser, TRole, TKey, IdentityUserClaim<TKey>,
			// IdentityUserRole<TKey>, IdentityUserLogin<TKey>, IdentityRoleClaim<TKey>, IdentityUserToken<TKey>>
			//Data Seeding 
			modelBuilder.Seed();

			//
			base.OnModelCreating(modelBuilder);
		}

		public DbSet<Product> Products { get; set; }
		public DbSet<Category> Categories { get; set; }

		public DbSet<AppConfig> AppConfigs { get; set; }

		public DbSet<Brand> Brands { get; set; }

		public DbSet<Cart> Carts { get; set; }

		public DbSet<CategoryTranslation> CategoryTranslations { get; set; }
		public DbSet<ProductInCategory> ProductInCategories { get; set; }

		public DbSet<Contact> Contacts { get; set; }

		public DbSet<Language> Languages { get; set; }

		public DbSet<Order> Orders { get; set; }

		public DbSet<OrderDetail> OrderDetails { get; set; }
		public DbSet<ProductTranslation> ProductTranslations { get; set; }

		public DbSet<Promotion> Promotions { get; set; }

		public DbSet<ProductImage> ProductImages { get; set; }
		public DbSet<Transaction> Transactions { get; set; }



	}
}