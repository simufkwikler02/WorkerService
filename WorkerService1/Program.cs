using WorkerService1;

var point = new Point();
var line = point.ToString();
point.Parse(line);
var serviceee = new LbsService();
serviceee.ReadAndSave("D:\\out_257.csv");
double lat;
double lon;
var lbs = new LBS(257, 2, 84, 55722);
serviceee.TryGetLatLng(lbs, out lon, out lat);
lbs = new LBS();
lbs = serviceee.FindLbs(20, 50);
serviceee.TryGetLatLng(lbs, out lon, out lat);

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<UdpSender>();
        services.AddHostedService<UdpReceiver>();
    })
    .Build();

await host.RunAsync();
