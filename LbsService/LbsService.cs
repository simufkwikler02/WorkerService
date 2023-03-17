using System.Globalization;

namespace LbsLibrary
{
    public class LbsService
    {
        private readonly Dictionary<LBS, Сoordinates> keyValuePairs = new();

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

                if (!StringReader.TryParseToInt(line, ref ind, ref indBuf, out int Mcc) ||
                    !StringReader.TryParseToInt(line, ref ind, ref indBuf, out int Net) ||
                    !StringReader.TryParseToInt(line, ref ind, ref indBuf, out int Area) ||
                    !StringReader.TryParseToInt(line, ref ind, ref indBuf, out int Cell) ||
                    !StringReader.TryParseToDouble(line, ref ind, ref indBuf, out double Lon) ||
                    !StringReader.TryParseLastToDouble(line, ref ind, ref indBuf, out double Lat))
                    return;

                var lbs = new LBS { Mcc = Mcc, Net = Net, Area = Area, Cell = Cell};
                var Сoordinates = new Сoordinates { Latitude = Lat , Longitude = Lon};
                this.keyValuePairs.Add(lbs, Сoordinates);
            }

        }
      

        public bool TryGetLatLng(LBS Lbs, out Сoordinates Сoordinates)
        {
            if(!this.keyValuePairs.TryGetValue(Lbs, out Сoordinates value))
            {
                Сoordinates = new Сoordinates { Latitude = default, Longitude = default};
                return false;
            }
            Сoordinates = value;
            return true;
        }

        public LBS FindLbs(Сoordinates Сoordinates)
        {
            double minLength = double.MaxValue;
            LBS lbs = new LBS();

            foreach (var item in this.keyValuePairs)
            {
                double lenght = Math.Pow(Math.Abs(Сoordinates.Longitude - item.Value.Longitude), 2) + Math.Pow(Math.Abs(Сoordinates.Latitude - item.Value.Latitude), 2);
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
