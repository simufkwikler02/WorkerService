using System.Globalization;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using Aspose.Gis;
using Aspose.Gis.Geometries;
using LbsLibrary;

namespace WorkerService1.Workers
{
    public class UdpSender : BackgroundService
    {
        private readonly LbsService _lbsService;

        private readonly VectorLayer _layer = Drivers.Gpx.OpenLayer(@"TestPoint\Point.gpx");

        public UdpSender(LbsService service)
        {
            _lbsService = service;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var server = new UdpClient("127.0.0.1", 22220);
            _lbsService.ReadAndSave("D:\\out_257.csv");
            var s = string.Empty;
            foreach (var feature in this._layer)
            {
                if (feature.Geometry.GeometryType == GeometryType.MultiLineString)
                {
                    var lines = (MultiLineString)feature.Geometry;                   
                    s = lines.AsText();
                    
                }
            }

            _layer.Dispose();
            var options = RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled;

            // 53.383240 26.538230 179.3
            var regex = new Regex(@"(-?\d+(.)\d+) +
                                    (-?\d+(.)\d+) +
                                    (-?\d+(.)\d+)", options);

            var matches = regex.Matches(s);
            var testPoint = new List<LbsLibrary.Point>();
            foreach (Match match in matches)
            {
                var line = match.Value.Split(' ');
                var point = new LbsLibrary.Point();

                if(!double.TryParse(line[0], NumberStyles.Float, CultureInfo.InvariantCulture, out double lon) ||
                   !double.TryParse(line[1], NumberStyles.Float, CultureInfo.InvariantCulture, out double lat))
                    continue;

                point.Ñoordinates = new Ñoordinates() { Lat = lat, Lon = lon };
                point.Lbs = _lbsService.FindLbs(point.Ñoordinates);

                testPoint.Add(point);
            }


            while (!stoppingToken.IsCancellationRequested)
            {
                var sat = 7;
                Console.WriteLine($"START SEND SAT = {sat}");
                foreach (var point in testPoint)
                {
                    point.Date = DateTime.Now;
                    point.Sat = sat;
                    var message = point.ToString();
                    var data = Encoding.UTF8.GetBytes(message);

                    await server.SendAsync(data, stoppingToken);
                    await Task.Delay(100, stoppingToken);
                }

                sat = 2;
                Console.WriteLine($"START SEND SAT = {sat}");
                foreach (var point in testPoint)
                {
                    point.Date = DateTime.Now;
                    point.Sat = sat;
                    var message = point.ToString();
                    var data = Encoding.UTF8.GetBytes(message);

                    await server.SendAsync(data, stoppingToken);
                    await Task.Delay(100, stoppingToken);
                }
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}