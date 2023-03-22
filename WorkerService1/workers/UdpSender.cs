using System.Text;
using System.Net.Sockets;
using Aspose.Gis.Geometries;
using Aspose.Gis;
using System.Text.RegularExpressions;
using System.Globalization;
using LbsLibrary;

namespace WorkerService1
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
            var server = new UdpClient("127.0.0.1", 22220);
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

            // 53.383240 26.538230 179.3
            var regex = new Regex(@"-?\d+(.)\d+ -?\d+(.)\d+ -?\d+(.)\d+");
            var matches = regex.Matches(s);
            var testPoint = new List<LbsLibrary.Point>();
            var rand = new Random();
            foreach (Match match in matches)
            {
                var line = match.Value.Split(' ');
                var point = new LbsLibrary.Point();

                if(!double.TryParse(line[0], NumberStyles.Float, CultureInfo.InvariantCulture, out double lon))
                {
                    continue;
                }

                if (!double.TryParse(line[1], NumberStyles.Float, CultureInfo.InvariantCulture, out double lat))
                {
                    continue;
                }

                point.—oordinatesRecord = new —oordinates() { Lat = lat, Lon = lon }; 

                point.Sat = rand.Next(1,8);
                point.LbsRecord = _lbsService.FindLbs(point.—oordinatesRecord);

                testPoint.Add(point);
            }


            while (!stoppingToken.IsCancellationRequested)
            {
                foreach (var point in testPoint)
                {
                    point.Date = DateTime.Now;

                    var message = point.ToString();
                    var data = Encoding.UTF8.GetBytes(message);

                    await server.SendAsync(data, stoppingToken);
                    await Task.Delay(1000, stoppingToken);
                }

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}