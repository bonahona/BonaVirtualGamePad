using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace BonaVirtualGamePad.Shared
{
    public class GamePadClient : GamePadNode
    {
        public IPEndPoint BroadCastEndPoint { get; set; }
        public IPEndPoint HostEndPoint { get; set; }
        public IPEndPoint LocalEndPoint { get; set; }

        public UdpClient Connection { get; set; }

        public GamePadClient()
        {
            BroadCastEndPoint = GetBroadCastEndPoint();
            Connection = new UdpClient();
        }

        public void SendDiscoveryBroadCast()
        {
            var package = CreateNetworkDiscoveryRequest();
            SendPackage(package, Connection, BroadCastEndPoint);
        }
    }
}
