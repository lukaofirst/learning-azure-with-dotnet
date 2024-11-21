using Azure.Storage.Queues;
using Domain.Options;
using Microsoft.Extensions.Options;

namespace Infra.Services;

public abstract class BaseQueueService(IOptions<AzureServices> options)
{
	private protected async Task<QueueClient> GetQueueClientAsync()
	{
		var azureServices = options.Value;

		// Create a new QueueClient for the specified queue name
		var queueClient = new QueueClient(azureServices.ConnectionString, azureServices.QueueName);

		// Ensure the queue exists, and create if not
		await queueClient.CreateIfNotExistsAsync();

		return queueClient;
	}
}
