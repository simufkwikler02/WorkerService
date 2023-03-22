using System.Text;
using System.Net.Sockets;
using LbsLibrary;

namespace WorkerService1
{
    public class UdpReceiver : BackgroundService
    {
        private readonly ILogger<UdpReceiver> _logger;
        private readonly LbsService _lbsService;
        private const string PathSave = "ResultPoint\\outPoint.csv";

        public UdpReceiver(ILogger<UdpReceiver> logger, LbsService service)
        {
            _logger = logger;
            _lbsService = service;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var server = new UdpClient(22220);
            await using var writer = File.CreateText(PathSave); 

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("UdpReceiver running at: {time}", DateTimeOffset.Now);

                var result = await server.ReceiveAsync(stoppingToken);
                var message = Encoding.UTF8.GetString(result.Buffer);
                var point = Point.Parse(message);
                Console.WriteLine("----------------------------------------------------------");
                Console.WriteLine(message + "(Received)");
                if (point is null)
                {
                    continue;
                }

                ParesePoint(point);
                Console.WriteLine(point.ToString() + "(Parsed)");
                writer.Write(point.ToString());
                await Task.Delay(100, stoppingToken);
            }
        }

        private void ParesePoint(Point point)
        {
            if (point.Sat < 3)
            {
                if(this._lbsService.TryGetLatLng(point.LbsRecord, out Сoordinates Сoordinates))
                {
                    point.СoordinatesRecord = Сoordinates;
                    point.Sat = 0;
                }
            }
        }
    }
}
