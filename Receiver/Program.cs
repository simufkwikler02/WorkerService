using LbsLibrary;
using Receiver;
using Receiver.Workers;
using Receiver.ServiceConfig;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.Configure<UdpReceiverConfig>(configuration.GetSection("ReceiverConfig"));
        services.AddHostedService<UdpReceiver>();
        services.AddSingleton<LbsService>();
        services.AddSingleton<WaitingForAppStartupService>();
    })
    .Build();

await host.RunAsync();