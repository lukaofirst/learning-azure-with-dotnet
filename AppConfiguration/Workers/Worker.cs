using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.FeatureManagement;

namespace AppConfiguration.Workers;

public class Worker(
	ILogger<Worker> logger,
	IConfigurationRefresher refresher,
	IFeatureManager featureManager) : BackgroundService
{
	private readonly ILogger<Worker> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
	private readonly IConfigurationRefresher _refresher = refresher ?? throw new ArgumentNullException(nameof(refresher));
	private readonly IFeatureManager _featureManager = featureManager ?? throw new ArgumentNullException(nameof(featureManager));

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		while (!stoppingToken.IsCancellationRequested)
		{
			// Intentionally not await TryRefreshAsync to avoid blocking the execution.
			_ = _refresher.TryRefreshAsync(stoppingToken);

			if (_logger.IsEnabled(LogLevel.Information))
			{
				if (await _featureManager.IsEnabledAsync("Beta"))
				{
					_logger.LogInformation("[{time}]: Worker is running with Beta feature.", DateTimeOffset.Now);
				}
				else
				{
					_logger.LogInformation("[{time}]: Worker is running.", DateTimeOffset.Now);
				}
			}

			await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
		}
	}
}
