using ConsoleApp_work;
using System.Diagnostics;


var time = new Stopwatch();
string url = "https://drive.google.com/u/0/uc?id=11gltuDZucDoX_7W32o0MfVFAp9PXFkh9&export=download";
//string url = "https://vk.com/doc167552191_659635597?hash=ac787D3lBhREUIqwLeZT5px3AIDPc6Cv1G02z8t8zQT&dl=BBoKYqPxbO5iAGwqTLBHK8uESEMNcamlv3qJozQU5eL";
//string url = "https://opencellid.org/ocid/downloads?token=pk.b6c2d57c221ba5e71e9c481f29767a8b&type=mcc&file=257.csv.gz";
string savePath = "D:\\";

if (!Directory.Exists(savePath))
{
    Console.WriteLine("This path is not exist");
    return;
}

var fileData = new FileData();
Console.WriteLine("Start download and save...");
time.Start();
await fileData.DownloadAndSaveCsv(url, savePath);
time.Stop();
Console.WriteLine($"Finish download and save! Time -- {time.ElapsedMilliseconds} ms.");
Console.WriteLine("---------------------------------------------");





