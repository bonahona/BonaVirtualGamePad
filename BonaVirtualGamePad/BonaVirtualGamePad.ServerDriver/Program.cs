using BonaVirtualGamePad.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace BonaVirtualGamePad.ServerDriver
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program();
        }


        public GamePadServer Server { get; set; }

        public Program()
        {
            Server = new GamePadServer(Defaults.DEFAULT_SERVER_NAME, "H4mligt");

            Console.WriteLine("Start listening...");
            while (true) {
                Thread.Sleep(2);
                var recievedPackages = Server.PollPackages(Server.UdpListener);

                foreach(var package in recievedPackages) {

                    if(package.PackageType == NetworkPackageType.ClientDiscoveryRequest) {
                        Server.SendNetworkDiscoveryResponse(Server.ListeningEndpoint, package.SourceEndpoint);
                    }
                    Console.WriteLine("Recieved package");
                }
            }
        }
    }
}
