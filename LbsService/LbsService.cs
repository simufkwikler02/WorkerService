using System.Globalization;

namespace LbsLibrary
{
    public class LbsService
    {
        private readonly Dictionary<LBS, CellTower> _cellTowerDictionary = new();

        public void ReadAndSave(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File is not exist");
                return;
            }

            using StreamReader reader = new(filePath);
            this._cellTowerDictionary.Clear();
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                var ind = -1;
                var indBuf = 0;

                if (!StringReader.TryParseToInt(line, ref ind, ref indBuf, out int Mcc) ||
                    !StringReader.TryParseToInt(line, ref ind, ref indBuf, out int Net) ||
                    !StringReader.TryParseToInt(line, ref ind, ref indBuf, out int Area) ||
                    !StringReader.TryParseToInt(line, ref ind, ref indBuf, out int Cell) ||
                    !StringReader.TryParseToDouble(line, ref ind, ref indBuf, out double Lon) ||
                    !StringReader.TryParseToDouble(line, ref ind, ref indBuf, out double Lat))
                    return;

                var lbs = new LBS { Mcc = Mcc, Net = Net, Area = Area, Cell = Cell };
                var lonLat = new Сoordinates { Lat = Lat, Lon = Lon };
                var cellTower = new CellTower { Lbs = lbs, LonLat = lonLat };
                this._cellTowerDictionary.Add(lbs, cellTower);
            }

        }      

        public bool TryGetLatLng(LBS lbs, out Сoordinates coordinates)
        {
            if(!this._cellTowerDictionary.TryGetValue(lbs, out CellTower tower))
            {
                coordinates = new Сoordinates { Lat = default, Lon = default};
                return false;
            }

            coordinates = tower.LonLat;
            return true;
        }

        public LBS FindLbs(Сoordinates coordinates)
        {
            var minLength = double.MaxValue;
            LBS lbs = new();

            foreach (var item in this._cellTowerDictionary)
            {
                var length = Math.Pow(coordinates.Lon - item.Value.LonLat.Lon, 2)
                                 + Math.Pow(coordinates.Lat - item.Value.LonLat.Lat, 2);
                if (!(length < minLength)) continue;
                minLength = length;
                lbs = item.Key;
            }

            return lbs;
        }
    }
}
