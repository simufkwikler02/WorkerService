using LbsLibrary;
using WorkerService1;
using WorkerService1.Workers;
using WorkerService1.ServiceConfig;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.Configure<UdpSenderConfig>(configuration.GetSection("SenderConfig"));
        services.Configure<UdpReceiverConfig>(configuration.GetSection("ReceiverConfig"));
        services.AddHostedService<UdpSender>();
        services.AddHostedService<UdpReceiver>();
        services.AddSingleton<LbsService>();
        services.AddSingleton<WaitingForAppStartupService>();
    })
    .Build();

await host.RunAsync();
