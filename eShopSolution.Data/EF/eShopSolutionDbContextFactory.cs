using eShopSolution.Utilities.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Data.EF
{
	public class eShopSolutionDbContextFactory : IDesignTimeDbContextFactory<EShopDbContext>
	{
		public EShopDbContext CreateDbContext(string[] args)
		{
			IConfigurationRoot configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json")
				.Build();
			var connectionString = configuration.GetConnectionString(SystemConstants.MainConnectionString);

			var optionsBuilder = new DbContextOptionsBuilder<EShopDbContext>();
			optionsBuilder.UseSqlServer(connectionString);

			return new EShopDbContext(optionsBuilder.Options);
		}
	}
}
