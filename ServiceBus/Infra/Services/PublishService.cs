using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Options;
using Microsoft.Extensions.Options;

namespace Infra.Services;

public class PublishService : IPublishService
{
	private readonly ServiceBusClient _serviceBusClient;
	private readonly string _queueName;

	public PublishService(IOptions<AzureServices> options)
	{
		var azureServices = options.Value;
		_serviceBusClient = new ServiceBusClient(azureServices.ConnectionString);
		_queueName = azureServices.QueueName!;
	}

	public async Task<bool> SendAsync(DomainMessage message)
	{
		var sender = _serviceBusClient.CreateSender(_queueName);

		// Serialize the message
		var jsonMessage = JsonSerializer.Serialize(message);

		// Create a new Service Bus message
		var serviceBusMessage = new ServiceBusMessage(jsonMessage);

		// Send the message
		await sender.SendMessageAsync(serviceBusMessage);

		return true;  // Return true if the message is successfully sent
	}
}
