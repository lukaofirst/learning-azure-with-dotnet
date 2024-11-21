using System.Net;
using System.Text.Json;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Options;
using Microsoft.Extensions.Options;

namespace Infra.Services;

public class PublishService(
	IOptions<AzureServices> options)
	: BaseQueueService(options), IPublishService
{
	public async Task<bool> SendAsync(DomainMessage message)
	{
		var queueClient = await GetQueueClientAsync();
		var jsonMessage = JsonSerializer.Serialize(message);

		// Send the message
		var response = await queueClient.SendMessageAsync(jsonMessage);

		return response.GetRawResponse().Status == (int)HttpStatusCode.Created;
	}
}
