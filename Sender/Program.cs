using LbsLibrary;
using Sender;
using Sender.Workers;
using Sender.ServiceConfig;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.Configure<UdpSenderConfig>(configuration.GetSection("SenderConfig"));
        services.AddHostedService<UdpSender>();
        services.AddSingleton<LbsService>();
        services.AddSingleton<WaitingForAppStartupService>();
    })
    .Build();

await host.RunAsync();