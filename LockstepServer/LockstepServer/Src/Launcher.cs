using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace LockstepServer.Src
{
    internal class Launcher : AppServer
    {
        public Launcher(int port) : base()
        {
            StartServer(port);

            var launchManager = new LaunchManager();
            Service.Instance.GetService<ManagerService>().RegisterManager(launchManager);
            launchManager.Launch();
        }

        protected override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            Service.Instance.DoUpdate(deltaTime);
        }

        private static void Main(string[] args)
        {
            Console.Title = "Lockstep Server";

            string host = "0.0.0.0";
            ushort port = 15940;
            string text = string.Empty;
            var dict = ArgumentParser.Parse(args);

            if (args.Length > 0)
            {
                if (dict.ContainsKey("host"))
                {
                    host = dict["host"];
                }
                if (dict.ContainsKey("port"))
                {
                    ushort.TryParse(dict["port"], out port);
                }
            }
            else
            {
                Console.WriteLine("Entering nothing will choose defaults.");
                Console.WriteLine("Enter Host IP (Default: 0.0.0.0):");

                /*text = Console.ReadLine();
                if (string.IsNullOrEmpty(text))
                {
                    host = "0.0.0.0";
                }
                Console.WriteLine("Enter Port (Default: 15940):");
                text = Console.ReadLine();
                if (string.IsNullOrEmpty(text))
                {
                    port = 15940;
                }
                else
                {
                    ushort.TryParse(text, out port);
                }*/

                host = "0.0.0.0";
                port = 15940;
            }

            Launcher server = null;
            new Thread(delegate ()
            {
                Console.WriteLine(string.Format("Hosting ip [{0}] on port [{1}]", host, port));
                Console.WriteLine("Commands Available\n(s)top - Stops hosting\n(r)estart - Restarts the hosting service even when stopped\n(q)uit - Quits the application\n(h)elp - Get a full list of comands");
                server = new Launcher(port);
                var frameTime = (int)(1000 / (float)30);
                while (server.IsRunning)
                {
                    server.DoUpdate(frameTime);
                    Thread.Sleep(frameTime);
                }
            })
            {
                IsBackground = true
            }.Start();
            ///----------------------------------------------------------------
            /// 判断是不是windows平台
            var runOnWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            while (true)
            {
                if (runOnWindows)
                {
                    text = Console.ReadLine().ToLower();
                    switch (text)
                    {
                        case "s":
                        case "stop":
                            OnStop(server);
                            break;

                        case "r":
                        case "restart":
                            OnRestart(server, host, port);
                            break;

                        case "q":
                        case "quit":
                            OnQuit(server);
                            break;

                        case "h":
                        case "help":
                            OnHelp();
                            break;
                    }
                }
                else
                {
                    Thread.Sleep(1);
                }
            }
        }

        private static void OnStop(Launcher server)
        {
            Launcher server4;
            Monitor.Enter(server4 = server);
            try
            {
                if (server.IsRunning)
                {
                    server.StopServer();
                }
                Console.WriteLine("Server stopped.");
            }
            finally
            {
                Monitor.Exit(server);
            }
        }

        private static void OnRestart(Launcher server, string host, int port)
        {
            Launcher server2;
            Monitor.Enter(server2 = server);
            try
            {
                if (server.IsRunning)
                {
                    server.StopServer();
                    Console.WriteLine("Server stopped.");
                }
            }
            finally
            {
                Monitor.Exit(server2);
            }
            Console.WriteLine("Restarting...");
            server = new Launcher(port);
            Console.WriteLine(string.Format("Hosting ip [{0}] on port [{1}]", host, port));
        }

        private static void OnQuit(Launcher server)
        {
            Launcher server3;
            Monitor.Enter(server3 = server);
            try
            {
                Console.WriteLine("Quitting...");
                server.StopServer();
            }
            finally
            {
                Monitor.Exit(server3);
            }
        }

        private static void OnHelp()
        {
            Console.WriteLine("(s)top - Stops hosting\n(r)estart - Restarts the hosting service even when stopped\n(e)lo - Set the elo range to accept in difference [i.e. \"elorange = 10\"]\n(q)uit - Quits the application\n(h)elp - Get a full list of comands");
        }
    }
}