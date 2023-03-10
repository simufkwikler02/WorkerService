using System.Text;
using System.Net.Sockets;

namespace WorkerService1
{
    public class UdpSender : BackgroundService
    {
        private readonly ILogger<UdpSender> _logger;
        private readonly UdpClient _server;

        public UdpSender(ILogger<UdpSender> logger)
        {
            _logger = logger;
            _server = new UdpClient("127.0.0.1", 22220);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                var message = DateTimeOffset.Now.ToString();
                byte[] data = Encoding.UTF8.GetBytes(message);

                await _server.SendAsync(data, stoppingToken);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}