using WorkerService1;
using System.Net;
using System.Net.Sockets;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Generator>();
        services.AddHostedService<Listener>();
        services.AddSingleton<UdpClient>(provider => new UdpClient("127.0.0.1", 22221));
    })
    .Build();

await host.RunAsync();
