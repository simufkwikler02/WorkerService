using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace WorkerService1
{
    public static class StringReader
    {
        private readonly static char Separator = ',';
        public static bool TryNextWord(string line, ref int ind, ref int indBuf)
        {
            indBuf = ind;
            ind = line.IndexOf(Separator, indBuf + 1);
            if (ind == -1)
                return false;
            return true;
        }

        public static bool TryParseToInt(string line, ref int ind, ref int indBuf, out int value)
        {
            if (!int.TryParse(line.AsSpan().Slice(indBuf + 1, ind - indBuf - 1), out value))
            {
                Console.WriteLine($"line convert error,line skipped");
                return false;
            }

            return true;
        }

        public static bool TryParseToDouble(string line, ref int ind, ref int indBuf, out double value)
        {
            if (!double.TryParse(line.AsSpan().Slice(indBuf + 1, ind - indBuf - 1), NumberStyles.Float, CultureInfo.InvariantCulture, out value))
            {
                Console.WriteLine($"line convert error,line skipped");
                return false;
            }

            return true;
        }

        public static bool TryParseLastToInt(string line, ref int ind, ref int indBuf, out int value)
        {
            if (!int.TryParse(line.AsSpan().Slice(indBuf + 1), out value))
            {
                Console.WriteLine($"line convert error,line skipped");
                return false;
            }

            return true;
        }

        public static bool TryParseLastToDouble(string line, ref int ind, ref int indBuf, out double value)
        {
            if (!double.TryParse(line.AsSpan().Slice(indBuf + 1), NumberStyles.Float, CultureInfo.InvariantCulture, out value))
            {
                Console.WriteLine($"line convert error,line skipped");
                return false;
            }

            return true;
        }

        public static bool TryParseToDateTime(string line, ref int ind, ref int indBuf, out DateTime value)
        {
            if (!DateTime.TryParse(line.AsSpan().Slice(indBuf + 1, ind - indBuf - 1), out value))
            {
                Console.WriteLine($"line convert error,line skipped");
                return false;
            }

            return true;
        }
    }
}
