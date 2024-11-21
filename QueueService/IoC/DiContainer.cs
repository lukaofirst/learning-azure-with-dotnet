using Azure.Storage.Queues;
using Domain.Interfaces;
using Domain.Options;
using Infra.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IoC;

public static class DiContainer
{
	public static void AddAppServices(this IServiceCollection services, IConfiguration configuration)
	{
		// Azure Storage configuration
		services.AddSingleton(x =>
		{
			var azureServices = new AzureServices();
			configuration.GetSection(nameof(AzureServices)).Bind(azureServices);

			// Create QueueClient using connection string and queue name from configuration
			return new QueueClient(azureServices.ConnectionString, azureServices.QueueName);
		});

		// Infra
		services.AddSingleton<IConsumerService, ConsumerService>();
		services.AddScoped<IPublishService, PublishService>();

		// Binding Options for Azure
		services.Configure<AzureServices>(configuration.GetSection(nameof(AzureServices)));

	}
}
