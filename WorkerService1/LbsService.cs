using System.Globalization;

namespace WorkerService1
{
    public class LbsService
    {
        private readonly Dictionary<LBS, (double, double)> keyValuePairs = new();

        public void ReadAndSave(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File is not exist");
                return;
            }

            using StreamReader reader = new(filePath);
            this.keyValuePairs.Clear();
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                var lineSpan = line.AsSpan();
                var ind = -1;
                var indBuf = 0;

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

                if (!StringReader.TryNextWord(line, ref ind, ref indBuf))
                    return;

                if (!StringReader.TryParseToInt(line, ref ind, ref indBuf, out int Cell))
                    return;

                if (!StringReader.TryNextWord(line, ref ind, ref indBuf))
                    return;

                if (!StringReader.TryParseToDouble(line, ref ind, ref indBuf, out double Lon))
                    return;

                double Lat;
                if (!StringReader.TryNextWord(line, ref ind, ref indBuf))
                {
                    if (!StringReader.TryParseLastToDouble(line, ref ind, ref indBuf, out Lat))
                        return;
                }
                else
                {
                    if (!StringReader.TryParseToDouble(line, ref ind, ref indBuf, out Lat))
                        return;
                }
                              

                var lbs = new LBS(Mcc, Net, Area, Cell);
                this.keyValuePairs.Add(lbs, (Lon, Lat));
            }

        }
      

        public bool TryGetLatLng(LBS Lbs, out double lon, out double lat)
        {
            if(!this.keyValuePairs.TryGetValue(Lbs, out (double,double) value))
            {
                lon = default;
                lat = default;
                return false;
            }
            lon = value.Item1;
            lat = value.Item2;
            return true;
        }

        public LBS FindLbs(double lon, double lat)
        {
            double minLength = double.MaxValue;
            LBS lbs = new LBS();

            foreach (var item in this.keyValuePairs)
            {
                double lenght = Math.Sqrt(Math.Pow(Math.Abs(lon - item.Value.Item1), 2) + Math.Pow(Math.Abs(lat - item.Value.Item2), 2));
                if (lenght < minLength)
                {
                    minLength = lenght;
                    lbs = item.Key;
                }
            }

            return lbs;
        }
    }
}
