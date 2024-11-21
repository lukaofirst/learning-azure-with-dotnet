using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infra.EFCore;

public class DataContext(DbContextOptions<DataContext> dbContextOptions) : DbContext(dbContextOptions)
{
	public DbSet<Person> Persons { get; set; }
}

public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
{
	public DataContext CreateDbContext(string[] args)
	{
		var optionsBuilder = new DbContextOptionsBuilder<DataContext>();

		// For design-time context creation, provide a fallback connection string.
		// Typically, this connection string would come from a configuration or environment variable.
		var connectionString = "Server=localhost,1433;Database=PersonDatabase;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=true";

		optionsBuilder.UseSqlServer(connectionString);

		return new DataContext(optionsBuilder.Options);
	}
}
