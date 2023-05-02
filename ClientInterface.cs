using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace ThumbnailServer
{
    internal class ClientInterface
    {
        public static Process RobloxProcess;
        public static IntPtr RobloxApp;
        public static void Roblox_DisableInterface()
        {
            DLLImport.PostMessage(new HandleRef(null, RobloxApp), 0x111, (IntPtr)(0x00080F5), IntPtr.Zero);
        }

        public static void Roblox_ThumbnailView()
        {
            DLLImport.SendMessageW(RobloxApp, 0x111, 32983, null);
        }

        public static void MakeExternalWindowBorderless(IntPtr MainWindowHandle)
        {
            int Style = 0;
            Style = DLLImport.GetWindowLongA(MainWindowHandle, (int)-16L);
            Style = Style & ~8388608 | 4194304;
            Style = Style & ~524288;
            Style = Style & ~262144;
            Style = Style & ~536870912;
            Style = Style & ~65536;
            DLLImport.SetWindowLongA(MainWindowHandle, (int)-16L, Style);
            Style = DLLImport.GetWindowLongA(MainWindowHandle, (int)-20L);

        }

        public static void Roblox_StartProcess(string ExecScript)
        {
            RobloxProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = $@"RobloxApp\{AppConfig.ProcessName}.exe",
                    Arguments = $"-script \"{ExecScript}\""
                }
            };

            RobloxProcess.Start();

            RobloxProcess.WaitForInputIdle();
            
            Thread.Sleep(500);

            foreach (Process p in Process.GetProcessesByName(AppConfig.ProcessName)) RobloxApp = p.MainWindowHandle;
            DLLImport.SetWindowPos(RobloxApp, 0, 0, 0, AppConfig.WindowSizeX, AppConfig.WindowSizeY, 0);
            MakeExternalWindowBorderless(RobloxApp);

            Thread.Sleep(2500);

            Roblox_DisableInterface();
        }

        public static void Roblox_StopProcess()
        {
            try
            {
                RobloxProcess.Kill();
            } catch (Exception e)
            {
                Logger.Add(e.Message, "roblox_stopprocess");
            }
        }
    }
}
