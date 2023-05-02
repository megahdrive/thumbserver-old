using System;
using System.Diagnostics;
using System.Threading;

namespace ThumbnailServer
{
    internal class ProcessMonitor
    {
        public static void Start()
        {
            while (true)
            {
                Process[] ClientProcs = Process.GetProcessesByName(AppConfig.ProcessName);
                if (ClientProcs.Length != 0)
                {
                    TimeSpan activeTime;
                    activeTime = DateTime.Now - ClientProcs[0].StartTime;
                    if (activeTime > TimeSpan.FromSeconds(15))
                    {
                        ClientProcs[0].Kill();
                        Logger.Add("process was open for an abnormal amount of time", "process_monitor");
                    }
                    Thread.Sleep(1000);
                }
            }
        }
    }
}
