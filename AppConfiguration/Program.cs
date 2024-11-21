using AppConfiguration.Workers;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.FeatureManagement;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAzureAppConfiguration(); // w3
builder.Services.AddFeatureManagement(); // w2

// w1
builder.Configuration.AddAzureAppConfiguration(options =>
{
	options.Connect(builder.Configuration["AppConfigurationConnString"])
		.Select(KeyFilter.Any, LabelFilter.Null)
		.ConfigureRefresh(refresh =>
		{
			refresh
				.Register("ConfigKeyExample", refreshAll: true)
				.SetRefreshInterval(TimeSpan.FromSeconds(15));
		});

	// w3
	options.UseFeatureFlags(featureFlagOptions =>
	{
		featureFlagOptions.Select(KeyFilter.Any, LabelFilter.Null);
		featureFlagOptions.SetRefreshInterval(TimeSpan.FromSeconds(5));
	});

	// Capture the refresher and add it as a singleton
	var refresher = options.GetRefresher(); // w2
	builder.Services.AddSingleton(refresher); // w2
});

builder.Services.AddHostedService<Worker>(); // w2

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseAzureAppConfiguration(); // w3

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
