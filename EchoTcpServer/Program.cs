using System;
using System.Threading.Tasks;

namespace EchoTcpServer
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            EchoServer server = new EchoServer(5000);
            var serverTask = server.StartAsync();

            string host = "127.0.0.1";
            int port = 60000;

            using (var sender = new UdpTimedSender(host, port))
            {
                sender.StartSending(1000);

                Console.WriteLine("Press 'q' to quit...");
                while (true)
                {
                    if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Q) 
                        break;
                    await Task.Delay(100);
                }

                sender.StopSending();
                server.Stop();
            }
        }
    }
}
