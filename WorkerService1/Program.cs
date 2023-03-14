using WorkerService1;

var point = new Point();
var line = point.ToString();
point.Parse(line); 

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<UdpSender>();
        services.AddHostedService<UdpReceiver>();
    })
    .Build();

await host.RunAsync();
