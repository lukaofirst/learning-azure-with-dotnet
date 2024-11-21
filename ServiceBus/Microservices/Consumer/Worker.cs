using Domain.Interfaces;

namespace Consumer;

public class Worker(
	ILogger<Worker> logger,
	IConsumerService consumerService) : BackgroundService
{
	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		while (!stoppingToken.IsCancellationRequested)
		{
			logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

			// No need to pass queueUrl since QueueClient in Azure already knows which queue to use
			var result = await consumerService.StartAsync();

			if (result.Count > 0)
				return;

			// Process each result
			result.ForEach(x => Console.WriteLine(x.ToString()));

			await Task.Delay(1000, stoppingToken);
		}
	}
}
