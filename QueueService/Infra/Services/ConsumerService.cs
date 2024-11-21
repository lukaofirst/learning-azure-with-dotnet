using System.Text.Json;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Options;
using Microsoft.Extensions.Options;

namespace Infra.Services;

public class ConsumerService(
	IOptions<AzureServices> options)
	: BaseQueueService(options), IConsumerService
{
	public async Task<List<DomainMessage>> StartAsync()
	{
		var queueClient = await GetQueueClientAsync();

		var result = new List<DomainMessage>();

		// Get up to 2 messages from the queue
		var receivedMessages = await queueClient.ReceiveMessagesAsync(2);

		// Check if there are any messages
		if (receivedMessages.Value is null || receivedMessages.Value.Length == 0)
			return Enumerable.Empty<DomainMessage>().ToList();

		foreach (var message in receivedMessages.Value)
		{
			// Deserialize message body to DomainMessage
			var messageParsed = JsonSerializer.Deserialize<DomainMessage>(message.MessageText);

			if (messageParsed == null) continue;
			
			result.Add(messageParsed);

			// Delete the message from the queue after processing
			await queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt);
		}

		return result;
	}
}
