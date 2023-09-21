using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Api.Infrastructure.Context
{
	public class CatalogContextDesignFactory : IDesignTimeDbContextFactory<CatalogContext>
	{
		public CatalogContext CreateDbContext(string[] args)
		{
			var optionsBuilder = new DbContextOptionsBuilder<CatalogContext>()
				.UseSqlServer("Data Source=ASUSX550V\\SQLEXPRESS;Initial Catalog=catalog;Persist Security Info =True;Trusted_Connection=True;TrustServerCertificate=True;");

			return new CatalogContext(optionsBuilder.Options);
		}
	}
}
