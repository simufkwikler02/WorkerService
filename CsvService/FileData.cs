using System.Globalization;
using System.Text;

namespace CsvService
{
    public class FileData
    {
        private const string FileName = "out_257.csv";


        public async Task DownloadAndSaveCsv(string uri, string pathWrite)
        {
            using var httpClient = new HttpClient();
            var stream = await httpClient.GetStreamAsync(uri);
            //using var decompressor = new GZipStream(stream, CompressionMode.Decompress);
            using var reader = new StreamReader(stream);

            if (!Directory.Exists(pathWrite))
            {
                Console.WriteLine("ERROR read. This path is not exist");
                return;
            }

            var pathSave = pathWrite + FileName;
            await using var writer = File.CreateText(pathSave);
            var resultLine = new StringBuilder();

            while (reader.ReadLine() is { } line)
            {
                LineEditor(resultLine, line);
                writer.Write(resultLine);
            }

            Console.WriteLine($"Save to {pathSave}");
        }

        private void LineEditor(StringBuilder outLine, string line)
        {
            outLine.Clear();
            var ind = -1;

            if (!LbsLibrary.CsvParser.NextIndex(line, ref ind))
                return;

            var firstWord = line.AsSpan()[..ind];

            if (firstWord.Length != 3 ||
                !(firstWord[0] == 'G' && firstWord[1] == 'S' && firstWord[2] == 'M') ||
                !LbsLibrary.CsvParser.TryParseToInt(line, ref ind, out int mcc) ||
                !LbsLibrary.CsvParser.TryParseToInt(line, ref ind, out int net) ||
                !LbsLibrary.CsvParser.TryParseToInt(line, ref ind, out int area) ||
                !LbsLibrary.CsvParser.TryParseToInt(line, ref ind, out int cell) ||
                !LbsLibrary.CsvParser.NextIndex(line, ref ind) ||
                !LbsLibrary.CsvParser.TryParseToDouble(line, ref ind, out double lon) ||
                !LbsLibrary.CsvParser.TryParseToDouble(line, ref ind, out double lat))
                return;
            
            outLine.Append(mcc).Append(',');
            outLine.Append(net).Append(',');
            outLine.Append(area).Append(',');
            outLine.Append(cell).Append(',');
            outLine.Append(lon.ToString(CultureInfo.InvariantCulture)).Append(',');
            outLine.Append(lat.ToString(CultureInfo.InvariantCulture)).AppendLine();
        }
    }
}
