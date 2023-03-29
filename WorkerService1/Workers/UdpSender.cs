using System.Globalization;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using Aspose.Gis;
using Aspose.Gis.Geometries;
using LbsLibrary;
using Microsoft.Extensions.Options;
using WorkerService1.ServiceConfig;

namespace WorkerService1.Workers
{
    public class UdpSender : BackgroundService
    {
        private readonly LbsService _lbsService;
        private readonly ILogger<UdpSender> _logger;
        private readonly UdpSenderConfig _udpSenderConfig;
        private readonly WaitingForAppStartupService _waitingService;
        private readonly VectorLayer _layer = Drivers.Gpx.OpenLayer(@"TestPoint\Point.gpx");

        public UdpSender(LbsService service, ILogger<UdpSender> logger, IOptions<UdpSenderConfig> udpSenderConfig, WaitingForAppStartupService waitService)
        {
            _lbsService = service;
            _logger = logger;
            _udpSenderConfig = udpSenderConfig.Value;
            _waitingService = waitService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!await _waitingService.WaitForAppStartup(stoppingToken))
                return;

            using var server = new UdpClient(_udpSenderConfig.Ip, _udpSenderConfig.Port);
            
            var s = string.Empty;
            foreach (var feature in _layer)
            {
                if (feature.Geometry.GeometryType == GeometryType.MultiLineString)
                {
                    var lines = (MultiLineString)feature.Geometry;                   
                    s = lines.AsText();
                }
            }

            _layer.Dispose();
            const RegexOptions options = RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled;

            // 53.383240 26.538230 179.3
            var regex = new Regex(@"(-?\d+[.]\d+)\s
                                    (-?\d+[.]\d+)\s
                                    (-?\d+[.]\d+)", options);

            var matches = regex.Matches(s);
            var testPoint = new List<LbsLibrary.Point>();
            foreach (Match match in matches)
            {
                var point = new LbsLibrary.Point();

                if(!double.TryParse(match.Groups[1].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out double lon) ||
                   !double.TryParse(match.Groups[2].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out double lat))
                    continue;

                point.Coordinates = new Coordinates() { Lat = lat, Lon = lon };
                point.Lbs = _lbsService.FindLbs(point.Coordinates);

                testPoint.Add(point);
            }

            var change = true;
            while (!stoppingToken.IsCancellationRequested)
            {
                var sat = 7;
                if (change) sat = 2;
                change = !change;

                Console.WriteLine("=======================================================");
                Console.WriteLine($"START SEND SAT = {sat}");
                Console.WriteLine("=======================================================");
                foreach (var point in testPoint)
                {
                    point.Date = DateTime.Now;
                    point.Sat = sat;
                    var message = point.ToString();
                    var data = Encoding.UTF8.GetBytes(message);

                    _logger.LogInformation("Send message --> {message}", message);
                    await server.SendAsync(data, stoppingToken);
                    await Task.Delay(1000, stoppingToken);
                }
            }
        }
    }
}