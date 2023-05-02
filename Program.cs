using System;
using System.Threading.Tasks;

namespace ThumbnailServer
{
    internal class Program
    {
        public static string version = "22.12.18";
        static void Main(string[] args)
        {
            Console.WriteLine($"ThumbnailServer @ {version}");
            
            Task.Run(() => Tcp.Listen());

            Console.WriteLine($"Listening for TCP on port {AppConfig.TcpPort}");

            ProcessMonitor.Start();
        }
    }
}
