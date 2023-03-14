using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace WorkerService1
{
    public class LbsService
    {
        private readonly Dictionary<LBS, (double, double)> keyValuePairs = new Dictionary<LBS, (double, double)>();
        private readonly char Separator = ',';
        public void ReadAndSave(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File is not exist");
                return;
            }

            using (StreamReader reader = new StreamReader(filePath))
            {
                this.keyValuePairs.Clear();
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    var lineSpan = line.AsSpan();
                    var ind = 0;                   

                    var indBuf = ind;
                    ind = line.IndexOf(Separator, indBuf);
                    if (ind == -1)
                        return;

                    int Mcc;
                    if (!int.TryParse(lineSpan.Slice(indBuf, ind - indBuf), out Mcc))
                    {
                        Console.WriteLine($"line convert error,line skipped");
                        return;
                    }


                    indBuf = ind;
                    ind = line.IndexOf(Separator, indBuf + 1);
                    if (ind == -1)
                        return;

                    int Net;
                    if (!int.TryParse(lineSpan.Slice(indBuf + 1, ind - indBuf - 1), out Net))
                    {
                        Console.WriteLine($"line convert error,line skipped");
                        return;
                    }


                    indBuf = ind;
                    ind = line.IndexOf(Separator, indBuf + 1);
                    if (ind == -1)
                        return;

                    int Area;
                    if (!int.TryParse(lineSpan.Slice(indBuf + 1, ind - indBuf - 1), out Area))
                    {
                        Console.WriteLine($"line convert error,line skipped");
                        return;
                    }


                    indBuf = ind;
                    ind = line.IndexOf(Separator, indBuf + 1);
                    if (ind == -1)
                        return;

                    int Cell;
                    if (!int.TryParse(lineSpan.Slice(indBuf + 1, ind - indBuf - 1), out Cell))
                    {
                        Console.WriteLine($"line convert error,line skipped");
                        return;
                    }

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
                    

                    double Lat;
                    if (ind == -1)
                    {
                        if (!double.TryParse(lineSpan.Slice(indBuf + 1), NumberStyles.Float, CultureInfo.InvariantCulture, out Lat))
                        {
                            Console.WriteLine($"line convert error,line skipped");
                            return;
                        }
                    }
                    else
                    {
                        if (!double.TryParse(lineSpan.Slice(indBuf + 1, ind - indBuf - 1), NumberStyles.Float, CultureInfo.InvariantCulture, out Lat))
                        {
                            Console.WriteLine($"line convert error,line skipped");
                            return;
                        }
                    }

                    var lbs = new LBS(Mcc, Net, Area, Cell);
                    this.keyValuePairs.Add(lbs, (Lon, Lat));


                }

            }
            
        }
      

        public bool TryGetLatLng(LBS Lbs, out double lon, out double lat)
        {
            if(!this.keyValuePairs.TryGetValue(Lbs, out (double,double) value))
            {
                lon = -1;
                lat = -1;
                return false;
            }
            lon = value.Item1;
            lat = value.Item2;
            return true;
        }

        public LBS FindLbs(double lat, double lng)
        {
            throw new NotImplementedException();
        }
    }
    
}
