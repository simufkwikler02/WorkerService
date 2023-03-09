using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace WorkerService1
{
    public class Generator : BackgroundService
    {
        private readonly ILogger<Generator> _logger;
        private readonly UdpClient _server;

        public Generator(ILogger<Generator> logger, UdpClient server)
        {
            _logger = logger;
            _server = server;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                var message = DateTimeOffset.Now.ToString();
                byte[] data = Encoding.UTF8.GetBytes(message);

                await _server.SendAsync(data);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }

    public class Listener : BackgroundService
    {
        private readonly ILogger<Listener> _logger;
        private readonly UdpClient _server;

        public Listener(ILogger<Listener> logger, UdpClient server)
        {
            _logger = logger;
            _server = server;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Listener running at: {time}", DateTimeOffset.Now);

                
                var result = await _server.ReceiveAsync();
                var message = Encoding.UTF8.GetString(result.Buffer);
                Console.WriteLine(message);
                

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}