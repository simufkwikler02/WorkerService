using System.Text;
using System.Net.Sockets;

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
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Listener running at: {time}", DateTimeOffset.Now);

                var result = await _server.ReceiveAsync(stoppingToken);
                var message = Encoding.UTF8.GetString(result.Buffer);
                Console.WriteLine(message);


                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
