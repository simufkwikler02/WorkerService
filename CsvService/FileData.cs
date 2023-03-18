using System.Text;
using System.Globalization;


namespace ConsoleApp_work
{
    public class FileData
    {
        private readonly string fileName = "out_257.csv";


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

            var path_save = pathWrite + this.fileName;
            using var writer = File.CreateText(path_save);
            string? line;
            var resultLine = new StringBuilder();

            while ((line = reader.ReadLine()) != null)
            {
                LineEditor(resultLine, line);
                writer.Write(resultLine);
            }

            Console.WriteLine($"Save to {path_save}");
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
                !LbsLibrary.StringReader.TryParseToInt(line, ref ind, ref indBuf, out int Mcc) ||
                !LbsLibrary.StringReader.TryParseToInt(line, ref ind, ref indBuf, out int Net) ||
                !LbsLibrary.StringReader.TryParseToInt(line, ref ind, ref indBuf, out int Area) ||
                !LbsLibrary.StringReader.TryParseToInt(line, ref ind, ref indBuf, out int Cell) ||
                !LbsLibrary.StringReader.SkipWords(line, ref ind, ref indBuf) ||
                !LbsLibrary.StringReader.TryParseToDouble(line, ref ind, ref indBuf, out double Lon) ||
                !LbsLibrary.StringReader.TryParseToDouble(line, ref ind, ref indBuf, out double Lat))
                return;
            
            outLine.Append(Mcc).Append(',');
            outLine.Append(Net).Append(',');
            outLine.Append(Area).Append(',');
            outLine.Append(Cell).Append(',');
            outLine.Append(Lon.ToString(CultureInfo.InvariantCulture)).Append(',');
            outLine.Append(Lat.ToString(CultureInfo.InvariantCulture)).AppendLine();
        }
    }
}
