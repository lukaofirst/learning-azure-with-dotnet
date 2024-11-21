using Consumer;
using IoC;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddAppServices(builder.Configuration);

var host = builder.Build();
host.Run();
