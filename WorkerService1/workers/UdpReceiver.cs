using System.Text;
using System.Net.Sockets;
using LbsLibrary;

namespace WorkerService1
{
    public class UdpReceiver : BackgroundService
    {
        private readonly ILogger<UdpReceiver> _logger;
        private readonly UdpClient _server;

        public UdpReceiver(ILogger<UdpReceiver> logger)
        {
            _logger = logger;
            _server = new UdpClient(22220);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var lbsService = new LbsService();
            lbsService.ReadAndSave("D:\\out_257.csv");

            while (!stoppingToken.IsCancellationRequested)
            {
                //_logger.LogInformation("Listener running at: {time}", DateTimeOffset.Now);

                var result = await _server.ReceiveAsync(stoppingToken);
                var message = Encoding.UTF8.GetString(result.Buffer);
                var point = new Point();
                point.Parse(message);
                ParesePoint(point, lbsService);
                Console.WriteLine(point.ToString());


                await Task.Delay(5000, stoppingToken);
            }
        }

        private void ParesePoint(Point point, LbsService  service)
        {
            if (point.Sat < 3)
            {
                var lbs = service.FindLbs(point.Lon, point.Lat);
                point.LbsRecord = lbs;
                point.Sat = 0;
                service.TryGetLatLng(lbs, out double Lon, out double Lat);
                point.Lon = Lon;
                point.Lat = Lat;
            }
        }
    }
}
