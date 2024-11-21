using Application;
using Application.Interfaces;
using Application.Services;
using Infra.Repositories;
using Infra.Repositories.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IoC;

public static class DiContainer
{
	public static void AddAppServices(this IServiceCollection services, IConfiguration configuration)
	{
		// Azure
		services.AddSingleton(cosmosClient =>
		{
			var cosmosDbEndpoint = configuration["CosmosDb:AccountEndpoint"];
			var cosmosDbKey = configuration["CosmosDb:AccountKey"];
			var options = new CosmosClientOptions()
			{
				HttpClientFactory = () => new HttpClient(new HttpClientHandler()
				{
					ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
				}),
				ConnectionMode = ConnectionMode.Gateway,
				LimitToEndpoint = true
			};

			return new CosmosClient(cosmosDbEndpoint, cosmosDbKey, options);
		});

		services.AddScoped<IPersonRepository, PersonRepository>();

		// Application
		services.AddAutoMapper(typeof(AutoMapperMappings).Assembly);
		services.AddScoped<IPersonService, PersonService>();

		// Infra
		services.AddScoped<IPersonRepository, PersonRepository>();
	}
}
