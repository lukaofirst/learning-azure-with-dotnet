using System.Text.Json;
using Application.Interfaces;
using Application.Mappings;
using Application.Services;
using Infra.Azure.Interfaces;
using Infra.Azure.Models;
using Infra.Azure.Services;
using Infra.EFCore;
using Infra.Repositories;
using Infra.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IoC;

public static class DiContainer
{
	public const string AZURE_KEYVAULT_NAME = "KeyVaultExample";

	public static void AddAppServices(this IServiceCollection services, IConfiguration configuration)
	{
		// Azure
		var keyVaultUri = configuration["AzureKeyVault:VaultUri"];
		services.AddSingleton<IAzureKeyVaultService>(new AzureKeyVaultService(keyVaultUri!));

		// Application
		services.AddScoped<IPersonService, PersonService>();
		services.AddAutoMapper(typeof(AutoMapperMappings).Assembly);

		// Infra
		services.AddScoped<IPersonRepository, PersonRepository>();

		var sqlServerConnectionString = string.Empty;

		using (var serviceScope = services.BuildServiceProvider().CreateScope())
		{
			var azureKeyVaultServiceService = serviceScope.ServiceProvider.GetRequiredService<IAzureKeyVaultService>();
			var result = azureKeyVaultServiceService
				.GetSecretAsync(AZURE_KEYVAULT_NAME).Result;

			var resultSerialized = JsonSerializer.Deserialize<EfCoreSecretsDto>(
				result,
				new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
			)!;

			sqlServerConnectionString = $@"
				Server={resultSerialized.Host},{resultSerialized.Port};
				Database={resultSerialized.Database};
				User Id={resultSerialized.Username};
				Password={resultSerialized.Password};
				TrustServerCertificate=true
			";
		};

		services.AddDbContext<DataContext>(opts => opts.UseSqlServer(sqlServerConnectionString));
	}
}
