using System.Globalization;
using System.Text;

namespace CsvService
{
    public class FileData
    {
        private readonly string _fileName = "out_257.csv";


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

            var pathSave = pathWrite + this._fileName;
            using var writer = File.CreateText(pathSave);
            string? line;
            var resultLine = new StringBuilder();

            while ((line = reader.ReadLine()) != null)
            {
                LineEditor(resultLine, line);
                writer.Write(resultLine);
            }

            Console.WriteLine($"Save to {pathSave}");
        }

        private void LineEditor(StringBuilder outLine, string line)
        {
            outLine.Clear();
            var indBuf = 0;
            var ind = -1;

            if (!LbsLibrary.StringReader.SkipWords(line, ref ind, ref indBuf))
                return;

            var firstWord = line.AsSpan()[..ind];

            if (firstWord.Length != 3 ||
                !(firstWord[0] == 'G' && firstWord[1] == 'S' && firstWord[2] == 'M') ||
                !LbsLibrary.StringReader.TryParseToInt(line, ref ind, ref indBuf, out int mcc) ||
                !LbsLibrary.StringReader.TryParseToInt(line, ref ind, ref indBuf, out int net) ||
                !LbsLibrary.StringReader.TryParseToInt(line, ref ind, ref indBuf, out int area) ||
                !LbsLibrary.StringReader.TryParseToInt(line, ref ind, ref indBuf, out int cell) ||
                !LbsLibrary.StringReader.SkipWords(line, ref ind, ref indBuf) ||
                !LbsLibrary.StringReader.TryParseToDouble(line, ref ind, ref indBuf, out double lon) ||
                !LbsLibrary.StringReader.TryParseToDouble(line, ref ind, ref indBuf, out double lat))
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
