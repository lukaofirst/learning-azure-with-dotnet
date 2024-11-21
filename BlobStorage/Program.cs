using Azure.Storage.Blobs;
using BlobStorage.Interfaces;
using BlobStorage.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton(x =>
{
	var configuration = x.GetRequiredService<IConfiguration>();
	var connectionString = configuration["AzureBlob:ConnectionString"];
	return new BlobServiceClient(connectionString);
});

// Add BlobStorageService
builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
