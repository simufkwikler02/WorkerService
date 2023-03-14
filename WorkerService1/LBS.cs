using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerService1
{
    public class LBS
    {
        public int Mcc { get; set; }

        public int Net { get; set; }

        public int Area { get; set; }

        public int Cell { get; set; }

        public LBS()
        {           
            this.Mcc = 23;
            this.Net = 54;
            this.Area = 2;
            this.Cell = 3;
        }
    }
}
