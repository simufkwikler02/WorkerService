using System.Text;
using System.Globalization;


namespace ConsoleApp_work
{
    public class FileData
    {
        private char Separator = ',';
        private string fileName = "out_257.csv";


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
            var lineSpan = line.AsSpan();
            var ind = line.IndexOf(Separator);
            if (ind == -1)
                return;

            var firstLine = lineSpan.Slice(0, ind);
            if (firstLine.Length != 3)
                return;

            if (!(firstLine[0] == 'G' && firstLine[1] == 'S' && firstLine[2] == 'M'))
                return;

            var indBuf = ind;
            ind = line.IndexOf(Separator, indBuf + 1);
            if (ind == -1)
                return;

            ushort Mcc;
            if (!ushort.TryParse(lineSpan.Slice(indBuf + 1, ind - indBuf - 1), out Mcc))
            {
                Console.WriteLine($"line convert error,line skipped");
                return;
            }


            indBuf = ind;
            ind = line.IndexOf(Separator, indBuf + 1);
            if (ind == -1)
                return;

            byte Net;
            if (!byte.TryParse(lineSpan.Slice(indBuf + 1, ind - indBuf - 1), out Net))
            {
                Console.WriteLine($"line convert error,line skipped");
                return;
            }


            indBuf = ind;
            ind = line.IndexOf(Separator, indBuf + 1);
            if (ind == -1)
                return;

            ushort Area;
            if (!ushort.TryParse(lineSpan.Slice(indBuf + 1, ind - indBuf - 1), out Area))
            {
                Console.WriteLine($"line convert error,line skipped");
                return;
            }


            indBuf = ind;
            ind = line.IndexOf(Separator, indBuf + 1);
            if (ind == -1)
                return;

            uint Cell;
            if (!uint.TryParse(lineSpan.Slice(indBuf + 1, ind - indBuf - 1), out Cell))
            {
                Console.WriteLine($"line convert error,line skipped");
                return;
            }

            indBuf = ind;
            ind = line.IndexOf(Separator, indBuf + 1);
            if (ind == -1)
                return;

            indBuf = ind;
            ind = line.IndexOf(Separator, indBuf + 1);
            if (ind == -1)
                return;

            double Lon;
            if (!double.TryParse(lineSpan.Slice(indBuf + 1, ind - indBuf - 1), NumberStyles.Float, CultureInfo.InvariantCulture, out Lon))
            {
                Console.WriteLine($"line convert error,line skipped");
                return;
            }

            indBuf = ind;
            ind = line.IndexOf(Separator, indBuf + 1);
            if (ind == -1)
                return;

            double Lat;
            if (!double.TryParse(lineSpan.Slice(indBuf + 1, ind - indBuf - 1), NumberStyles.Float, CultureInfo.InvariantCulture, out Lat))
            {
                Console.WriteLine($"line convert error,line skipped");
                return;
            }


            outLine.Append(Mcc).Append(',');
            outLine.Append(Net).Append(',');
            outLine.Append(Area).Append(',');
            outLine.Append(Cell).Append(',');
            outLine.Append(Lon.ToString(CultureInfo.InvariantCulture)).Append(',');
            outLine.Append(Lat.ToString(CultureInfo.InvariantCulture)).AppendLine();
        }

    }
}
