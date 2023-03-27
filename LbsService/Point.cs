using System.Text;
using System.Globalization;

namespace LbsLibrary
{
    public class Point
    {
        public DateTime Date { get; set; }

        public Coordinates Coordinates { get; set; }

        public int Sat { get; set; }

        public Lbs Lbs { get; set; }

        public static bool TryParse(string line, out Point point)
        {
            point = new Point();
            var ind = -1;
            if (!CsvParser.TryParseToDateTime(line, ref ind, out DateTime time) ||
                !CsvParser.TryParseToDouble(line, ref ind, out double lon) ||
                !CsvParser.TryParseToDouble(line, ref ind, out double lat) ||
                !CsvParser.TryParseToInt(line, ref ind, out int sat) ||
                !CsvParser.TryParseToInt(line, ref ind, out int mcc) ||
                !CsvParser.TryParseToInt(line, ref ind, out int net) ||
                !CsvParser.TryParseToInt(line, ref ind, out int area) ||
                !CsvParser.TryParseToInt(line, ref ind, out int cell))
                return false;

            var lbs = new Lbs { Mcc = mcc, Net = net, Area = area, Cell = cell };
            var coordinates = new Coordinates() { Lon = lon, Lat = lat };
            point.Date = time;
            point.Lbs = lbs;
            point.Sat = sat;
            point.Coordinates = coordinates;
            return true;
        }

        public override string ToString()
        {
            StringBuilder outLine = new();
            outLine.Append(this.Date).Append(',');
            outLine.Append(this.Coordinates.Lon.ToString(CultureInfo.InvariantCulture)).Append(',');
            outLine.Append(this.Coordinates.Lat.ToString(CultureInfo.InvariantCulture)).Append(',');
            outLine.Append(this.Sat).Append(',');
            outLine.Append(this.Lbs.Mcc).Append(',');
            outLine.Append(this.Lbs.Net).Append(',');
            outLine.Append(this.Lbs.Area).Append(',');
            outLine.Append(this.Lbs.Cell).AppendLine();

            return outLine.ToString();
        }
    }
}
