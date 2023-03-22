using System.Globalization;

namespace LbsLibrary
{
    public class LbsService
    {
        private readonly Dictionary<Lbs, CellTower> _cellTowerDictionary = new();

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

                if (!StringReader.TryParseToInt(line, ref ind, ref indBuf, out int mcc) ||
                    !StringReader.TryParseToInt(line, ref ind, ref indBuf, out int net) ||
                    !StringReader.TryParseToInt(line, ref ind, ref indBuf, out int area) ||
                    !StringReader.TryParseToInt(line, ref ind, ref indBuf, out int cell) ||
                    !StringReader.TryParseToDouble(line, ref ind, ref indBuf, out double lon) ||
                    !StringReader.TryParseToDouble(line, ref ind, ref indBuf, out double lat))
                    return;

                var lbs = new Lbs { Mcc = mcc, Net = net, Area = area, Cell = cell };
                var lonLat = new Сoordinates { Lat = lat, Lon = lon };
                var cellTower = new CellTower { Lbs = lbs, LonLat = lonLat };
                this._cellTowerDictionary.Add(lbs, cellTower);
            }

        }      

        public bool TryGetLatLng(Lbs lbs, out Сoordinates coordinates)
        {
            if(!this._cellTowerDictionary.TryGetValue(lbs, out CellTower tower))
            {
                coordinates = new Сoordinates { Lat = default, Lon = default};
                return false;
            }

            coordinates = tower.LonLat;
            return true;
        }

        public Lbs FindLbs(Сoordinates coordinates)
        {
            var minLength = double.MaxValue;
            Lbs lbs = new();

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
