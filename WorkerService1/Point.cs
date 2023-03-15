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

        private readonly char Separator = ',';

        public Point()
        {
            this.Date = DateTime.Now;
            this.Lat = 0.4;
            this.Lon = 0.5;
            this.Sat = 4;
            this.LbsRecord = new LBS();
        }

        public void Parse(string line)
        {
            var lineSpan = line.AsSpan();
            var ind = 0;

            var indBuf = ind;
            ind = line.IndexOf(Separator, indBuf + 1);
            if (ind == -1)
                return;

            DateTime time;
            if (!DateTime.TryParse(lineSpan.Slice(indBuf + 1, ind - indBuf - 1), out time))

            {
                Console.WriteLine($"line convert error,line skipped");
                return;
            }

            indBuf = ind;
            ind = line.IndexOf(Separator, indBuf + 1);
            if (ind == -1)
                return;

            double Lon;
            if (!double.TryParse(lineSpan.Slice(indBuf + 1, ind - indBuf - 1), NumberStyles.Float, CultureInfo.InvariantCulture, out Lon))
            {
                Console.WriteLine($"line convert error,line skipped");
                return;
            }

            indBuf = ind;
            ind = line.IndexOf(Separator, indBuf + 1);
            if (ind == -1)
                return;

            double Lat;
            if (!double.TryParse(lineSpan.Slice(indBuf + 1, ind - indBuf - 1), NumberStyles.Float, CultureInfo.InvariantCulture, out Lat))
            {
                Console.WriteLine($"line convert error,line skipped");
                return;
            }          

            indBuf = ind;
            ind = line.IndexOf(Separator, indBuf + 1);
            if (ind == -1)
                return;

            int Mcc;
            if (!int.TryParse(lineSpan.Slice(indBuf + 1, ind - indBuf - 1), out Mcc))
            {
                Console.WriteLine($"line convert error,line skipped");
                return;
            }


            indBuf = ind;
            ind = line.IndexOf(Separator, indBuf + 1);
            if (ind == -1)
                return;

            int Net;
            if (!int.TryParse(lineSpan.Slice(indBuf + 1, ind - indBuf - 1), out Net))
            {
                Console.WriteLine($"line convert error,line skipped");
                return;
            }


            indBuf = ind;
            ind = line.IndexOf(Separator, indBuf + 1);
            if (ind == -1)
                return;

            int Area;
            if (!int.TryParse(lineSpan.Slice(indBuf + 1, ind - indBuf - 1), out Area))
            {
                Console.WriteLine($"line convert error,line skipped");
                return;
            }


            indBuf = ind;
            ind = line.IndexOf(Separator, indBuf + 1);
            if (ind == -1)
                return;

            int Cell;
            if (!int.TryParse(lineSpan.Slice(indBuf + 1, ind - indBuf - 1), out Cell))
            {
                Console.WriteLine($"line convert error,line skipped");
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
