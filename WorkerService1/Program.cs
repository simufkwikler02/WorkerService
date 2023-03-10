using WorkerService1;
using System.Net;
using System.Net.Sockets;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<UdpSender>();
        services.AddHostedService<UdpReceiver>();
    })
    .Build();

await host.RunAsync();
