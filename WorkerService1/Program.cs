using WorkerService1;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddSingleton<UdpServer>(provider => new UdpServer("127.0.0.1", 22220));
    })
    .Build();

await host.RunAsync();
