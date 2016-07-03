using BonaVirtualGamePad.Shared;
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
            Client = new GamePadClient();

            while (true) {
                var command = Console.ReadLine();

                if (command == "broadcast") {
                    Console.WriteLine("Send broadcast");
                    Client.ClearDiscoveredServers();
                    Client.SendDiscoveryBroadCast();

                    Thread.Sleep(50);
                    var responses = Client.PollPackages(Client.Connection);
                    foreach(var response in responses) {
                        try {
                            Client.HandlePackagaResponse(response);
                        }catch(Exception e) {
                            Console.WriteLine("Failed to handle package, " + e.Message);
                        }
                    }
                }
            }
        }
    }
}
