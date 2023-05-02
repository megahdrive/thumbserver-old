using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ThumbnailServer
{
    internal class Tcp
    {
        static TcpListener listener = new TcpListener(AppConfig.TcpAddress, AppConfig.TcpPort);
        public static void Listen()
        {
            listener.Start();

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                NetworkStream ns = client.GetStream();

                Task.Run(() => HandleConnection(client));
            }
        }

        static void HandleConnection(TcpClient client)
        {
            Logger.Add($"accepted client {client.Client.RemoteEndPoint}", "tcp");
            try
            {
                NetworkStream networkStream = client.GetStream();
                StreamReader streamReader = new StreamReader(networkStream);
                StreamWriter streamWriter = new StreamWriter(networkStream);

                // let client know that this is the server it expects
                streamWriter.Write($"OK {Program.version}\n");
                streamWriter.Flush();

                while (client.Connected)
                {
                    var com = streamReader.ReadLine();

                    HandleInteraction(streamWriter, com);
                }

                Logger.Add($"client {client.Client.RemoteEndPoint} disconnected", "tcp");
                
                networkStream.Close();
                streamReader.Close();
                streamWriter.Close();

                client.Close();
            } catch {
                Logger.Add($"client {client.Client.RemoteEndPoint} disconnected", "tcp");
                client.Close();
            }
        }

        static void HandleInteraction(StreamWriter writer, string interaction)
        {
            Job Operation = JsonConvert.DeserializeObject<Job>(interaction);
            
            // SBQS (Super Bad Queue System)
            while (true)
            {
                Process[] activeProc = Process.GetProcessesByName(AppConfig.ProcessName);
                if (activeProc.Length == 0)
                {
                    break;
                }
                Thread.Sleep(1000);
            }

            //{"View":1, "ExecScript":"game.GuiRoot:remove(); Instance.new('Part', game.Workspace)"}
            //{"View":1, "ExecScript":"game.GuiRoot:remove(); local player = game.Players:CreateLocalPlayer(0); player:LoadCharacter()"}
            switch (Operation.View)
            {
                case 1:
                    writer.WriteLine(Render.PlrView(Operation.ExecScript)); writer.Flush(); break;
            }
        }
    }
}
