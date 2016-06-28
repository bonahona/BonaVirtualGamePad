using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace BonaVirtualGamePad.Shared
{
    public class GamePadServer : GamePadNode
    {
        public IPEndPoint ListeningEndpoint { get; set; }
        public UdpClient UdpListener { get; set; }

        public GamePadServer(int listeningPort = Defaults.DEFAULT_SERVER_PORT)
        {
            ListeningEndpoint = new IPEndPoint(IPAddress.Any, listeningPort);
            UdpListener = new UdpClient(ListeningEndpoint);
        }

        public void SendNetworkDiscoveryResponse(IPEndPoint localEndpoint, IPEndPoint remoteEndpoint)
        {
            var package = CreateNetworkDiscoveryResponse(localEndpoint);
            SendPackage(package, UdpListener, remoteEndpoint);
        }
    }
}
