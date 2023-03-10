using WorkerService1;


IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<UdpSender>();
        services.AddHostedService<UdpReceiver>();
    })
    .Build();

await host.RunAsync();
