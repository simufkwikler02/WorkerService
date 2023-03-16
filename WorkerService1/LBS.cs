
namespace WorkerService1
{
    public struct LBS
    {
        public int Mcc { get; set; }

        public int Net { get; set; }

        public int Area { get; set; }

        public int Cell { get; set; }

        public LBS(int Mcc, int Net, int Area, int Cell)
        {
            this.Mcc = Mcc;
            this.Net = Net;
            this.Area = Area;
            this.Cell = Cell;
        }
    }
}
