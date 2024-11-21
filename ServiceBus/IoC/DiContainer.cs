using Azure.Messaging.ServiceBus;
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
		// Azure Service Bus configuration
		services.AddSingleton(provider =>
		{
			var azureServices = new AzureServices();
			configuration.GetSection(nameof(AzureServices)).Bind(azureServices);

			// Create ServiceBusClient using connection string from configuration
			return new ServiceBusClient(azureServices.ConnectionString);
		});

		// Infra
		services.AddSingleton<IConsumerService, ConsumerService>();
		services.AddScoped<IPublishService, PublishService>();

		// Binding Options for Azure
		services.Configure<AzureServices>(configuration.GetSection(nameof(AzureServices)));
	}
}
