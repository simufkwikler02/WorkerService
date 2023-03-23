namespace LbsLibrary
{
    public class LbsService
    {
        //private readonly Dictionary<Lbs, CellTower> _cellTowerDictionary = new();
        private const string FilePath = "D:\\out_257.csv";

        private readonly Lazy<Dictionary<Lbs, CellTower>> _cellTowerDictionary = new(ReadAndSave());

        public static Dictionary<Lbs, CellTower> ReadAndSave()
        {
            var cellTowerDictionary = new Dictionary<Lbs, CellTower>();

            if (!File.Exists(FilePath))
            {
                throw new FileNotFoundException("File is not exist");
            }

            using StreamReader reader = new(FilePath);

            while (reader.ReadLine() is { } line)
            {
                var ind = -1;
                var indBuf = 0;

                if (!StringReader.TryParseToInt(line, ref ind, ref indBuf, out int mcc) ||
                    !StringReader.TryParseToInt(line, ref ind, ref indBuf, out int net) ||
                    !StringReader.TryParseToInt(line, ref ind, ref indBuf, out int area) ||
                    !StringReader.TryParseToInt(line, ref ind, ref indBuf, out int cell) ||
                    !StringReader.TryParseToDouble(line, ref ind, ref indBuf, out double lon) ||
                    !StringReader.TryParseToDouble(line, ref ind, ref indBuf, out double lat))
                    continue;

                var lbs = new Lbs { Mcc = mcc, Net = net, Area = area, Cell = cell };
                var lonLat = new Сoordinates { Lat = lat, Lon = lon };
                var cellTower = new CellTower { Lbs = lbs, LonLat = lonLat };
                cellTowerDictionary.Add(lbs, cellTower);
            }

            return cellTowerDictionary;
        }      

        public bool TryGetLatLng(Lbs lbs, out Сoordinates coordinates)
        {
            if(!this._cellTowerDictionary.Value.TryGetValue(lbs, out CellTower tower))
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

            foreach (var item in this._cellTowerDictionary.Value)
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
