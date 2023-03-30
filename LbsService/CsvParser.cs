using System.Globalization;

namespace LbsLibrary
{
    public static class CsvParser
    {
        private const char Separator = ',';
        public static bool NextIndex(string line, ref int ind)
        {
            ind = line.IndexOf(Separator, ind + 1);
            return ind != -1;
        }

        public static bool TryParseToInt(string line, ref int ind, out int result)
        {
            var leftIndex = ind;
            if (!NextIndex(line, ref ind))
            {
                ind = line.Length;
            }

            if (!int.TryParse(line.AsSpan().Slice(leftIndex + 1, ind - leftIndex - 1), out result))
            {
                Console.WriteLine("line convert error,line skipped");
                return false;
            }

            return true;
        }

        public static bool TryParseToDouble(string line, ref int ind, out double result)
        {
            var leftIndex = ind;
            if (!NextIndex(line, ref ind))
            {
                ind = line.Length;
            }

           
            if (!double.TryParse(line.AsSpan().Slice(leftIndex + 1, ind - leftIndex - 1), NumberStyles.Float, CultureInfo.InvariantCulture, out result))
            {
                Console.WriteLine("line convert error,line skipped");
                return false;
            }
            

            return true;
        }

        public static bool TryParseToDateTime(string line, ref int ind, out DateTime result)
        {
            var leftIndex = ind;
            if (!NextIndex(line, ref ind))
            {
                ind = line.Length;
            }

            if (!DateTime.TryParse(line.AsSpan().Slice(leftIndex + 1, ind - leftIndex - 1), out result))
            {
                Console.WriteLine("line convert error,line skipped");
                return false;
            }

            return true;
        }
    }
}
