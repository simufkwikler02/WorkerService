using System.Globalization;

namespace LbsLibrary
{
    public static class StringReader
    {
        private readonly static char Separator = ',';
        public static bool SkipWords(string line, ref int ind, ref int indBuf)
        {
            indBuf = ind;
            ind = line.IndexOf(Separator, indBuf + 1);
            if (ind == -1)
                return false;
            return true;
        }

        public static bool TryParseToInt(string line, ref int ind, ref int indBuf, out int value)
        {
            if (!StringReader.SkipWords(line, ref ind, ref indBuf))
            {
                value = default;
                return false;
            }

            if (!int.TryParse(line.AsSpan().Slice(indBuf + 1, ind - indBuf - 1), out value))
            {
                Console.WriteLine($"line convert error,line skipped");
                return false;
            }

            return true;
        }

        public static bool TryParseToDouble(string line, ref int ind, ref int indBuf, out double value)
        {
            if (!StringReader.SkipWords(line, ref ind, ref indBuf))
            {
                value = default;
                return false;
            }

            if (!double.TryParse(line.AsSpan().Slice(indBuf + 1, ind - indBuf - 1), NumberStyles.Float, CultureInfo.InvariantCulture, out value))
            {
                Console.WriteLine($"line convert error,line skipped");
                return false;
            }

            return true;
        }

        public static bool TryParseLastToInt(string line, ref int ind, ref int indBuf, out int value)
        {
            if (StringReader.SkipWords(line, ref ind, ref indBuf))
            {
                value = default;
                return false;
            }

            if (!int.TryParse(line.AsSpan()[(indBuf + 1)..], out value))
            {
                Console.WriteLine($"line convert error,line skipped");
                return false;
            }

            return true;
        }

        public static bool TryParseLastToDouble(string line, ref int ind, ref int indBuf, out double value)
        {
            if (StringReader.SkipWords(line, ref ind, ref indBuf))
            {
                value = default;
                return false;
            }

            if (!double.TryParse(line.AsSpan()[(indBuf + 1)..], NumberStyles.Float, CultureInfo.InvariantCulture, out value))
            {
                Console.WriteLine($"line convert error,line skipped");
                return false;
            }

            return true;
        }

        public static bool TryParseToDateTime(string line, ref int ind, ref int indBuf, out DateTime value)
        {
            if (!StringReader.SkipWords(line, ref ind, ref indBuf))
            {
                value = default;
                return false;
            }

            if (!DateTime.TryParse(line.AsSpan().Slice(indBuf + 1, ind - indBuf - 1), out value))
            {
                Console.WriteLine($"line convert error,line skipped");
                return false;
            }

            return true;
        }
    }
}
