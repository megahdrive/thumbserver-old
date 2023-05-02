using System;
using System.Diagnostics;
using System.IO;

namespace ThumbnailServer
{
    internal class Logger
    {
        private static int RendersInSession = 0;
        private static int ErrorInSession = 0;

        public static void Add(string message, string function, bool writeToStdio = false)
        {
            if (!File.Exists(AppConfig.LogFileName)) File.Create(AppConfig.LogFileName);
            string now = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            string formulatedLog = $"{now}  {function.ToUpper()}    {message.ToUpper()}";

            Debug.WriteLine(formulatedLog);
            if (writeToStdio) Console.WriteLine(formulatedLog);
            File.AppendAllText(AppConfig.LogFileName, formulatedLog + Environment.NewLine);
        }

        public static void SetStatus(bool success, bool increment = true)
        {
            if (increment) RendersInSession++;
            if (!success) ErrorInSession++;
            
            Console.Title = $"ThumbnailServer ({RendersInSession} renders, {ErrorInSession} failures)";
        }
    }
}
