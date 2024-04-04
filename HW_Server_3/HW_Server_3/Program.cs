using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HW_Server_3
{
    internal class UDPServ
    {
        static IPAddress ipAddr = IPAddress.Parse("127.0.0.1");
        async static Task Main(string[] args)
        {
            Console.WriteLine("Enter port:\n");
            int port = int.Parse(Console.ReadLine());
            Dictionary<string, int> map = new Dictionary<string, int>()
            {
                {"processor", 10000 },
                {"graphic card",  999999},
                {"mother card", 5000 },
                {"psu", 5700 },
                {"SSD", 5900 },
                {"memory", 9000 }
            };

            Task.Run(Answer);
            await waitExit();

            async Task Answer()
            {
                using (UdpClient udpClient = new UdpClient(port))
                {
                    while (true)
                    {
                        try
                        {
                            var result = await udpClient.ReceiveAsync();
                            string msg = Encoding.UTF8.GetString(result.Buffer);
                            byte[] bytes = Encoding.UTF8.GetBytes($"Cost: {map[msg]}");
                            Console.WriteLine(bytes);
                            Console.WriteLine(result.RemoteEndPoint.ToString());
                            await udpClient.SendAsync(bytes, bytes.Length, result.RemoteEndPoint);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                    }
                }
            }
            async Task waitExit()
            {
                Console.WriteLine("Press enter to end programm\n");
                Console.ReadLine();
            }
        }
    }
}
