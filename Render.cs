using System.Drawing;
using System.Threading;

namespace ThumbnailServer
{
    internal class Render
    {
        public static string PlrView(string ExecScript)
        {
            ClientInterface.Roblox_StartProcess(ExecScript);
            ClientInterface.Roblox_ThumbnailView();
            Thread.Sleep(300);
            Bitmap render = PostProcess.Screenshot();
            render = PostProcess.CleanBitmap(render, AppConfig.BgColor);
            ClientInterface.Roblox_StopProcess();
            return PostProcess.BitmapToBase64(render);
        }
    }
    public class Job
    {
        public int View { get; set; }
        public string ExecScript { get; set; }
    }
}
