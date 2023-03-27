namespace LbsLibrary
{
    public class LbsService
    {
        //private readonly Dictionary<Lbs, CellTower> _cellTowerDictionary = new();
        private const string FilePath = "D:\\out_257.csv";

        private readonly Lazy<Dictionary<Lbs, CellTower>> _cellTowerDictionary = new(ReadAndSave);

        private static Dictionary<Lbs, CellTower> ReadAndSave()
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

                if (!CsvParser.TryParseToInt(line, ref ind, out int mcc) ||
                    !CsvParser.TryParseToInt(line, ref ind, out int net) ||
                    !CsvParser.TryParseToInt(line, ref ind, out int area) ||
                    !CsvParser.TryParseToInt(line, ref ind, out int cell) ||
                    !CsvParser.TryParseToDouble(line, ref ind, out double lon) ||
                    !CsvParser.TryParseToDouble(line, ref ind, out double lat))
                    continue;

                var lbs = new Lbs { Mcc = mcc, Net = net, Area = area, Cell = cell };
                var lonLat = new Coordinates { Lat = lat, Lon = lon };
                var cellTower = new CellTower { Lbs = lbs, LonLat = lonLat };
                cellTowerDictionary.Add(lbs, cellTower);
            }

            return cellTowerDictionary;
        }      

        public bool TryGetLatLng(Lbs lbs, out Coordinates coordinates)
        {
            if(!this._cellTowerDictionary.Value.TryGetValue(lbs, out CellTower tower))
            {
                coordinates = new Coordinates { Lat = default, Lon = default};
                return false;
            }

            coordinates = tower.LonLat;
            return true;
        }

        public Lbs FindLbs(Coordinates coordinates)
        {
            var minLength = double.MaxValue;
            Lbs lbs = new();

            foreach (var item in this._cellTowerDictionary.Value)
            {
                var length = Math.Pow(coordinates.Lon - item.Value.LonLat.Lon, 2) 
                           + Math.Pow(coordinates.Lat - item.Value.LonLat.Lat, 2);
                if (length < minLength)
                {
                    minLength = length;
                    lbs = item.Key;
                }
            }

            return lbs;
        }
    }
}
