using System.Text;
using System.Net.Sockets;
using Aspose.Gis.Geometries;
using Aspose.Gis;
using System.Text.RegularExpressions;
using LbsLibrary;
using System.Globalization;

namespace WorkerService1
{
    public class UdpSender : BackgroundService
    {
        private readonly ILogger<UdpSender> _logger;
        private readonly UdpClient _server;
  
        private readonly VectorLayer layer = Drivers.Gpx.OpenLayer(@"D:\Point.gpx");

        public UdpSender(ILogger<UdpSender> logger)
        {
            _logger = logger;
            _server = new UdpClient("127.0.0.1", 22220);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string s = string.Empty;
            foreach (var feature in this.layer)
            {
                // Проверка геометрии MultiLineString
                if (feature.Geometry.GeometryType == GeometryType.MultiLineString)
                {
                    // Читать трек
                    var lines = (MultiLineString)feature.Geometry;                   
                    s = lines.AsText();
                    
                }
            }
            var regex = new Regex(@"-?\d+(.)\d+ -?\d+(.)\d+ -?\d+(.)\d+");
            MatchCollection matches = regex.Matches(s);
            var TestPoint = new List<LbsLibrary.Point>();
            foreach (Match match in matches)
            {
                var line = match.Value.Split(' ');
                var point = new LbsLibrary.Point();
                if(!double.TryParse(line[0], NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
                {
                    continue;
                }
                point.Lon = value;

                if (!double.TryParse(line[0], NumberStyles.Float, CultureInfo.InvariantCulture, out value))
                {
                    continue;
                }
                point.Lat = value;
                point.Sat = 5;
                TestPoint.Add(point);
            }


            while (!stoppingToken.IsCancellationRequested)
            {
                foreach (var point in TestPoint)
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    point.Date = DateTime.Now;

                    var message = point.ToString();
                    byte[] data = Encoding.UTF8.GetBytes(message);

                    await _server.SendAsync(data, stoppingToken);
                    await Task.Delay(3000, stoppingToken);
                }

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}