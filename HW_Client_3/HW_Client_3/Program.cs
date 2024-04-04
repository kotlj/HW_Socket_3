using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace HW_Client_3
{
    internal class Program
    {
        static IPAddress ipAddr = IPAddress.Parse("127.0.0.1");
        static int count = 0;
        async static Task Main(string[] args)
        {
            Console.Write("Enter recive port: ");
            if (!int.TryParse(Console.ReadLine(), out var recPort)) return;
            Console.Write("Enter send port: ");
            if (!int.TryParse(Console.ReadLine(), out var sendPort)) return;
            Console.WriteLine("\n");

            

            await sendMsg();

            async Task sendMsg()
            {
                using (UdpClient sender = new UdpClient(recPort))
                {
                    Task.Run(reciveMsg);
                    Console.WriteLine("Enter name of detail who price you want to recive\n(Processor, graphic card, mother card, PSU, SSD, memory, or disconnect):\n");
                    while (true)
                    {
                        try
                        {
                            string msg = Console.ReadLine();
                            if (msg != null && msg.ToLower() != "disconnect" && count < 10)
                            {
                                byte[] data = Encoding.UTF8.GetBytes(msg);
                                await sender.SendAsync(data, data.Length, new IPEndPoint(ipAddr, sendPort));
                                count++;
                            }
                            else if (msg.ToLower() == "disconnect")
                            {
                                break;
                            }
                            else if (count >= 10)
                            {
                                Console.WriteLine("Out of requests!");
                            }
                            else
                            {
                                Console.WriteLine();
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                    }
                    async Task reciveMsg()
                    {
                     
                       while (true)
                       {
                           
                           var res = await sender.ReceiveAsync();
                           string recived = Encoding.UTF8.GetString(res.Buffer);
                           Console.WriteLine(recived);
                       }
                     
                    }
                }
            }
            async Task counterReset()
            {
                while (true)
                {
                    await Task.Delay(180000); 
                    //Таймеры на тасках. Да, опять. Ну работает же! Я их сделаю столь же неотемлимой частью себя, как и тернарные операторы.
                    //А ещё эту информацию можно было-бы хранить на сервере. Только это по новой мапу добавлять, ставить там для всех время первого запроса
                    //каждые пару минут проверять не прошел-ли у кого-то тайм-аут... А я ещё поспать там вроде-бы планировал. 
                    count = 0;
                }
            }
        }
    }
}
