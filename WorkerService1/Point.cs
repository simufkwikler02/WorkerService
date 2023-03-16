using System.Text;
using System.Globalization;

namespace WorkerService1
{
    public class Point
    {
        public DateTime Date { get; set; }

        public double Lon { get; set; }

        public double Lat { get; set; }

        public int Sat { get; set; }

        public LBS LbsRecord { get; set; }

        public void Parse(string line)
        {
            var ind = -1;
            var indBuf = 0;

            if (!StringReader.TryNextWord(line, ref ind, ref indBuf))
                return;

            if (!StringReader.TryParseToDateTime(line, ref ind, ref indBuf, out DateTime time))
                return;

            if (!StringReader.TryNextWord(line, ref ind, ref indBuf))
                return;

            if (!StringReader.TryParseToDouble(line, ref ind, ref indBuf, out double Lon))
                return;

            if (!StringReader.TryNextWord(line, ref ind, ref indBuf))
                return;

            if (!StringReader.TryParseToDouble(line, ref ind, ref indBuf, out double Lat))
                return;

            if (!StringReader.TryNextWord(line, ref ind, ref indBuf))
                return;

            if (!StringReader.TryParseToInt(line, ref ind, ref indBuf, out int Sat))
                return;

            if (!StringReader.TryNextWord(line, ref ind, ref indBuf))
                return;

            if (!StringReader.TryParseToInt(line, ref ind, ref indBuf, out int Mcc))
                return;

            if (!StringReader.TryNextWord(line, ref ind, ref indBuf))
                return;

            if (!StringReader.TryParseToInt(line, ref ind, ref indBuf, out int Net))
                return;

            if (!StringReader.TryNextWord(line, ref ind, ref indBuf))
                return;

            if (!StringReader.TryParseToInt(line, ref ind, ref indBuf, out int Area))
                return;
                
            int Cell;
            if (!StringReader.TryNextWord(line, ref ind, ref indBuf))
            {
                if (!StringReader.TryParseLastToInt(line, ref ind, ref indBuf, out Cell))
                    return;
            }
            else
            {
                if (!StringReader.TryParseToInt(line, ref ind, ref indBuf, out Cell))
                    return;
            }

            this.Date = time;
            this.Lon = Lon;
            this.Lat = Lat;
            this.Sat = Sat;
            this.LbsRecord = new LBS(Mcc, Net, Area, Cell);
        }

        public override string ToString()
        {
            StringBuilder outLine = new StringBuilder();
            outLine.Append(this.Date).Append(',');
            outLine.Append(this.Lat.ToString(CultureInfo.InvariantCulture)).Append(',');
            outLine.Append(this.Lon.ToString(CultureInfo.InvariantCulture)).Append(',');
            outLine.Append(this.Sat).Append(',');
            outLine.Append(this.LbsRecord.Mcc).Append(',');
            outLine.Append(this.LbsRecord.Net).Append(',');
            outLine.Append(this.LbsRecord.Area).Append(',');
            outLine.Append(this.LbsRecord.Cell).AppendLine();

            return outLine.ToString();
        }
    }
}
