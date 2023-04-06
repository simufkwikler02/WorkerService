using System.Net.Sockets;
using System.Text;
using LbsLibrary;
using Microsoft.Extensions.Options;
using Receiver.ServiceConfig;

namespace Receiver.Workers
{
    public class UdpReceiver : BackgroundService
    {
        private readonly LbsService _lbsService;
        private readonly ILogger<UdpReceiver> _logger;
        private readonly UdpReceiverConfig _udpSenderConfig;
        private readonly WaitingForAppStartupService _waitingService;

        public UdpReceiver(LbsService service, ILogger<UdpReceiver> logger, IOptions<UdpReceiverConfig> udpReceiverConfig, WaitingForAppStartupService waitService)
        {
            _lbsService = service;
            _logger = logger;
            _udpSenderConfig = udpReceiverConfig.Value;
            _waitingService = waitService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!await _waitingService.WaitForAppStartup(stoppingToken))
                return;

            _logger.LogInformation("Port --> {_udpSenderConfig.Port}", _udpSenderConfig.Port);
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
