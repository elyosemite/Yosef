using EventProcessor.Worker;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<EventProcessorWorker>();

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddJsonFile("secrets.json", optional: true)
    .AddEnvironmentVariables();

builder.Services.AddWorker(builder.Configuration);

var host = builder.Build();
host.Run();
