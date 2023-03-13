using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerService1
{
    public class Point
    {
        public DateTime Date { get; set; }

        public double Lat { get; set; }

        public double Lon { get; set; }

        public int Sat { get; set; }

        public LBS LbsRecord { get; set; }

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

        }

        public override string ToString()
        {
            string result = string.Empty;
            return result;
        }

    }
}
