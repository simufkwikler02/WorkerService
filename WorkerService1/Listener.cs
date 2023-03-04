using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerService1
{
    public class Listener : BackgroundService
    {
        private readonly ILogger<Listener> _logger;
        private readonly UdpServer _server;

        public Listener(ILogger<Listener> logger, UdpServer server)
        {
            _logger = logger;
            _server = server;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Listener running at: {time}", DateTimeOffset.Now);

                await _server.ReceiveAsync();
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
