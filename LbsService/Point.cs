using System.Text;
using System.Globalization;

namespace LbsLibrary
{
    public class Point
    {
        public DateTime Date { get; set; }

        public Сoordinates СoordinatesRecord { get; set; }

        public int Sat { get; set; }

        public Lbs LbsRecord { get; set; }

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
            point.LbsRecord = lbs;
            point.Sat = sat;
            point.СoordinatesRecord = coordinates;
            return true;
        }

        public override string ToString()
        {
            StringBuilder outLine = new();
            outLine.Append(this.Date).Append(',');
            outLine.Append(this.СoordinatesRecord.Lon.ToString(CultureInfo.InvariantCulture)).Append(',');
            outLine.Append(this.СoordinatesRecord.Lat.ToString(CultureInfo.InvariantCulture)).Append(',');
            outLine.Append(this.Sat).Append(',');
            outLine.Append(this.LbsRecord.Mcc).Append(',');
            outLine.Append(this.LbsRecord.Net).Append(',');
            outLine.Append(this.LbsRecord.Area).Append(',');
            outLine.Append(this.LbsRecord.Cell).AppendLine();

            return outLine.ToString();
        }
    }
}
