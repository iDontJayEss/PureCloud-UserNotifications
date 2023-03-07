using Serilog;
using Serilog.Events;
using UserNotifications.ServiceHost;
using Voxai.Integration.Providers.PureCloud;

IHost host = Host.CreateDefaultBuilder(args)
    .UseSerilog(Log.Logger = new LoggerConfiguration()
        .WriteTo.File("logs/serviceHost.log")
        .WriteTo.Console()
        .MinimumLevel.Debug()
        .CreateLogger())
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddPureCloudProviders();
    })
    .Build();

await host.RunAsync();
