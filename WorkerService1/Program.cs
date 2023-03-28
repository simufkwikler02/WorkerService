using LbsLibrary;
using WorkerService1.Workers;
using WorkerService1.ServiceConfig;

//var point = new Point();
//var line = point.ToString();
//point.Parse(line);
//var serviceee = new LbsService();
//serviceee.ReadAndSave("D:\\out_257.csv");
//double lat;
//double lon;
//var lbs = new LBS(257, 2, 84, 55722);
//serviceee.TryGetLatLng(lbs, out lon, out lat);
//lbs = new LBS();
//lbs = serviceee.FindLbs(20, 50);
//serviceee.TryGetLatLng(lbs, out lon, out lat);

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
    })
    .Build();

await host.RunAsync();
