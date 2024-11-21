using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Options;
using Microsoft.Extensions.Options;

namespace Infra.Services;

public class ConsumerService : IConsumerService
{
	private readonly ServiceBusClient _serviceBusClient;
	private readonly string _queueName;

	public ConsumerService(IOptions<AzureServices> options)
	{
		var azureServices = options.Value;

		_serviceBusClient = new ServiceBusClient(azureServices.ConnectionString);
		_queueName = azureServices.QueueName!;
	}

	public async Task<List<DomainMessage>> StartAsync()
	{
		var result = new List<DomainMessage>();

		// Create a receiver for the queue
		var receiver = _serviceBusClient.CreateReceiver(_queueName);

		// Receive a batch of messages (up to 2)
		var receivedMessages = await receiver.ReceiveMessagesAsync(maxMessages: 2);

		// Check if there are any messages
		if (receivedMessages is null || !receivedMessages.Any())
			return Enumerable.Empty<DomainMessage>().ToList();

		// Process each message
		foreach (var message in receivedMessages)
		{
			// Deserialize message body to DomainMessage
			var messageParsed = JsonSerializer.Deserialize<DomainMessage>(message.Body.ToString());

			if (messageParsed is null) continue;
			
			result.Add(messageParsed);

			// Complete (acknowledge) the message after processing
			await receiver.CompleteMessageAsync(message);
		}

		return result;
	}
}
