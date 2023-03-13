using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace WorkerService1
{
    public class Point
    {
        public DateTime Date { get; set; }

        public double Lat { get; set; }

        public double Lon { get; set; }

        public int Sat { get; set; }

        public LBS LbsRecord { get; set; }

        private readonly char Separator = ',';

        public Point()
        {
            this.Date = DateTime.Now;
            this.Lat = 0;
            this.Lon = 0;
            this.Sat = 0;
            this.LbsRecord = new LBS();
        }

        public void Parse(string line)
        {
            throw new NotImplementedException();
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
