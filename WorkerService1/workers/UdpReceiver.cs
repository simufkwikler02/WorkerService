using System.Text;
using System.Net.Sockets;
using LbsLibrary;

namespace WorkerService1
{
    public class UdpReceiver : BackgroundService
    {
        private readonly ILogger<UdpReceiver> _logger;
        private readonly LbsService _lbsService;

        public UdpReceiver(ILogger<UdpReceiver> logger, LbsService service)
        {
            _logger = logger;
            _lbsService = service;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var server = new UdpClient(22220);
            _lbsService.ReadAndSave("D:\\out_257.csv");

            while (!stoppingToken.IsCancellationRequested)
            {
                //_logger.LogInformation("Listener running at: {time}", DateTimeOffset.Now);

                var result = await server.ReceiveAsync(stoppingToken);
                var message = Encoding.UTF8.GetString(result.Buffer);
                var point = Point.Parse(message);
                Console.WriteLine("----------------------------------------------------------");
                Console.WriteLine(message + "(Received)");
                if (point is null)
                {
                    continue;
                }
                ParesePoint(point, _lbsService);
                Console.WriteLine(point.ToString() + "(Parsed)");
                


                await Task.Delay(100, stoppingToken);
            }
        }

        private void ParesePoint(Point point, LbsService  service)
        {
            if (point.Sat < 3)
            {
                if(service.TryGetLatLng(point.LbsRecord, out Сoordinates lonlat))
                {
                    point.СoordinatesRecord = lonlat;
                    point.Sat = 0;
                }
            }
        }
    }
}
