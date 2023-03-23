using System.Text;
using System.Globalization;

namespace LbsLibrary
{
    public class Point
    {
        public DateTime Date { get; set; }

        public Сoordinates Сoordinates { get; set; }

        public int Sat { get; set; }

        public Lbs Lbs { get; set; }

        public static bool TryParse(string line, out Point point)
        {
            point = new Point();
            var ind = -1;
            var indBuf = 0;
            if (!StringReader.TryParseToDateTime(line, ref ind, ref indBuf, out DateTime time) ||
                !StringReader.TryParseToDouble(line, ref ind, ref indBuf, out double lon) ||
                !StringReader.TryParseToDouble(line, ref ind, ref indBuf, out double lat) ||
                !StringReader.TryParseToInt(line, ref ind, ref indBuf, out int sat) ||
                !StringReader.TryParseToInt(line, ref ind, ref indBuf, out int mcc) ||
                !StringReader.TryParseToInt(line, ref ind, ref indBuf, out int net) ||
                !StringReader.TryParseToInt(line, ref ind, ref indBuf, out int area) ||
                !StringReader.TryParseToInt(line, ref ind, ref indBuf, out int cell))
                return false;

            var lbs = new Lbs { Mcc = mcc, Net = net, Area = area, Cell = cell };
            var coordinates = new Сoordinates() { Lon = lon, Lat = lat };
            point.Date = time;
            point.Lbs = lbs;
            point.Sat = sat;
            point.Сoordinates = coordinates;
            return true;
        }

        public override string ToString()
        {
            StringBuilder outLine = new();
            outLine.Append(this.Date).Append(',');
            outLine.Append(this.Сoordinates.Lon.ToString(CultureInfo.InvariantCulture)).Append(',');
            outLine.Append(this.Сoordinates.Lat.ToString(CultureInfo.InvariantCulture)).Append(',');
            outLine.Append(this.Sat).Append(',');
            outLine.Append(this.Lbs.Mcc).Append(',');
            outLine.Append(this.Lbs.Net).Append(',');
            outLine.Append(this.Lbs.Area).Append(',');
            outLine.Append(this.Lbs.Cell).AppendLine();

            return outLine.ToString();
        }
    }
}
