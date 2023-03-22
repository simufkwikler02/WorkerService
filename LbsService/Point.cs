using System.Text;
using System.Globalization;

namespace LbsLibrary
{
    public class Point
    {
        public DateTime Date { get; set; }

        public Сoordinates СoordinatesRecord { get; set; }

        public int Sat { get; set; }

        public LBS LbsRecord { get; set; }

        public static Point? Parse(string line)
        {
            var ind = -1;
            var indBuf = 0;
            if (!StringReader.TryParseToDateTime(line, ref ind, ref indBuf, out DateTime time) ||
                !StringReader.TryParseToDouble(line, ref ind, ref indBuf, out double Lon) ||
                !StringReader.TryParseToDouble(line, ref ind, ref indBuf, out double Lat) ||
                !StringReader.TryParseToInt(line, ref ind, ref indBuf, out int Sat) ||
                !StringReader.TryParseToInt(line, ref ind, ref indBuf, out int Mcc) ||
                !StringReader.TryParseToInt(line, ref ind, ref indBuf, out int Net) ||
                !StringReader.TryParseToInt(line, ref ind, ref indBuf, out int Area) ||
                !StringReader.TryParseToInt(line, ref ind, ref indBuf, out int Cell))
                return null;

            var lbs = new LBS { Mcc = Mcc, Net = Net, Area = Area, Cell = Cell };
            var Сoordinates = new Сoordinates() { Lon = Lon, Lat = Lat };

            return new Point() { Date = time, СoordinatesRecord = Сoordinates, Sat = Sat, LbsRecord = lbs };
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
