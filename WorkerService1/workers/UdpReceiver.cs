using System.Net.Sockets;
using System.Text;
using LbsLibrary;

namespace WorkerService1.Workers
{
    public class UdpReceiver : BackgroundService
    {
        private readonly LbsService _lbsService;
        private readonly ILogger<UdpReceiver> _logger;
        private const string PathSave = "ResultPoint\\outPoint.csv";

        public UdpReceiver(LbsService service, ILogger<UdpReceiver> logger)
        {
            _lbsService = service;
            _logger = logger;

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var server = new UdpClient(22220);
            await using var writer = File.CreateText(PathSave); 

            while (!stoppingToken.IsCancellationRequested)
            {
                var result = await server.ReceiveAsync(stoppingToken);
                var message = Encoding.UTF8.GetString(result.Buffer);
                
                if (!Point.TryParse(message, out var point))
                {
                    continue;
                }
                
                ValidationPoint(point);
                _logger.LogInformation("Received point --> {point}" , point);
                await writer.WriteAsync(point.ToString());
                await Task.Delay(100, stoppingToken);
            }
        }

        private void ValidationPoint(Point point)
        {
            if (point.Sat >= 3)
                return;

            if (!this._lbsService.TryGetLatLng(point.Lbs, out Coordinates coordinates))
            {
                point.Coordinates = coordinates;
                point.Sat = 0;
            }
        }
    }
}
