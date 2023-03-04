namespace WorkerService1
{
    public class Generator : BackgroundService
    {
        private readonly ILogger<Generator> _logger;
        private readonly UdpServer _server;

        public Generator(ILogger<Generator> logger, UdpServer server)
        {
            _logger = logger;
            _server = server;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                await _server.SendAsync();
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}