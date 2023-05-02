using System.Net;
using ImageMagick;

namespace ThumbnailServer
{
    internal class AppConfig
    {
        public static int WindowSizeX = 420;
        public static int WindowSizeY = 420;
        public static MagickColor BgColor = new MagickColor(0, 255, 0);
        public static string ProcessName = "robloxapp";

        public static IPAddress TcpAddress = IPAddress.Parse("192.168.4.77");
        public static int TcpPort = 420;

        public static string LogFileName = "ThumbnailServer.log";
    }
}
