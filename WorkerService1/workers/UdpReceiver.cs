using System.Text;
using System.Net.Sockets;
using LbsLibrary;

namespace WorkerService1
{
    public class UdpReceiver : BackgroundService
    {
        private readonly LbsService _lbsService;
        private const string PathSave = "ResultPoint\\outPoint.csv";

        public UdpReceiver(LbsService service)
        {
            _lbsService = service;
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
                
                Console.WriteLine("----------------------------------------------------------");
                Console.WriteLine(message + "(Received)");


                ParsePoint(point);
                Console.WriteLine(point.ToString() + "(Parsed)");
                writer.Write(point.ToString());
                await Task.Delay(100, stoppingToken);
            }
        }

        private void ParsePoint(Point point)
        {
            if (point.Sat >= 3 || !this._lbsService.TryGetLatLng(point.LbsRecord, out Сoordinates coordinates)) return;

            point.СoordinatesRecord = coordinates;
            point.Sat = 0;
        }
    }
}
