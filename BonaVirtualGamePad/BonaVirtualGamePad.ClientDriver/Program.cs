using BonaVirtualGamePad.Shared;
using BonaVirtualGamePad.SharedWindows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BonaVirtualGamePad.ClientDriver
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program();
        }

        public GamePadClient Client { get; set; }

        public Program()
        {
            Client = new GamePadClient(new WindowsUdpClient(), new WindowsPlatform());

            Client.OnServerDiscovered += ServerDiscovered;

            while (true) {
                var commandLine = Console.ReadLine();

                var parts = commandLine.Split(' ');

                var command = parts[0];
                var commandArgs = new List<String>();

                if (parts.Length > 1) {
                    for (int i = 1; i < parts.Length; i++) {
                        commandArgs.Add(parts[i]);
                    }
                }

                try {
                    HandleCommand(command, commandArgs);
                } catch (Exception e) {
                    Console.WriteLine("Failed to parse command");
                }
            }
        }

        public void HandleCommand(String command, List<String>args)
        {
            if (command == "discovery") {
                Console.WriteLine("Send discovery broadcast");
                Client.ClearDiscoveredServers();
                Client.SendDiscoveryBroadCast();

                Thread.Sleep(50);
                var responses = Client.PollNetworkPackages();
                foreach (var response in responses) {
                    try {
                        Client.HandlePackageResponse(response);
                    } catch (Exception e) {
                        Console.WriteLine("Failed to handle package, " + e.Message);
                    }
                }
            }else if(command == "connect") {
                var serverIndex = int.Parse(args[0]);

                var serverConnection = Client.DiscoveredServers[serverIndex];

                if(args.Count > 1) {
                    var password = args[1];
                    Console.WriteLine(String.Format("Connecting to {0} with password", serverConnection.ServerName));
                    Client.ConnectToServer(serverConnection, password);
                }else {
                    Console.WriteLine(String.Format("Connecting to {0}", serverConnection.ServerName));
                    Client.ConnectToServer(serverConnection);
                }
            }
        }

        public void ServerDiscovered(GamePadClient client, ClientServerInformation clientServerInformation, int serverIndex)
        {
            Console.WriteLine(String.Format("{0}\t {1}", serverIndex, clientServerInformation.DebugServerInfo()));
        }
    }
}
