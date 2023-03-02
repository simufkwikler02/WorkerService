using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;


namespace WorkerService1
{

    public class UdpServer : IDisposable
    {
        private readonly UdpClient client;

        public UdpServer(string ipAddress, int port)
        {
            client = new UdpClient(port);
            client.Connect(IPAddress.Parse(ipAddress), port);
        }

        public async Task ReceiveAsync()
        {
            while (true)
            {
                var result = await client.ReceiveAsync();
                var message = Encoding.UTF8.GetString(result.Buffer);
                Console.WriteLine(message);
            }
        }

        public async Task SendAsync()
        {
            var message = DateTimeOffset.Now.ToString();
            byte[] data = Encoding.UTF8.GetBytes(message);

            await client.SendAsync(data);
        }

        public void Dispose() => client.Dispose();
    }

}

