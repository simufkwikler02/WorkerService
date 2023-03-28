using System.Net.Sockets;
using System.Text;
using LbsLibrary;
using Microsoft.Extensions.Options;
using WorkerService1.ServiceConfig;

namespace WorkerService1.Workers
{
    public class UdpReceiver : BackgroundService
    {
        private readonly LbsService _lbsService;
        private readonly UdpReceiverConfig _udpSenderConfig;
        private readonly ILogger<UdpReceiver> _logger;

        public UdpReceiver(LbsService service, ILogger<UdpReceiver> logger, IOptions<UdpReceiverConfig> udpReceiverConfig)
        {
            _lbsService = service;
            _logger = logger;
            _udpSenderConfig = udpReceiverConfig.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var server = new UdpClient(_udpSenderConfig.Port);

            while (!stoppingToken.IsCancellationRequested)
            {
                var result = await server.ReceiveAsync(stoppingToken);
                var message = Encoding.UTF8.GetString(result.Buffer);
                
                if (!Point.TryParse(message, out var point))
                    continue;
                
                
                ValidationPoint(point);
                _logger.LogInformation("Received point --> {point}" , point);
               
                await Task.Delay(1000, stoppingToken);
            }
        }

        private void ValidationPoint(Point point)
        {
            if (point.Sat >= 3)
                return;

            if (_lbsService.TryGetLatLng(point.Lbs, out Coordinates coordinates))
            {
                point.Coordinates = coordinates;
                point.Sat = 0;
            }
        }
    }
}
