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

        public List<ClientServerInformation> DiscoveredServers { get; set; }

        public GamePadClient()
        {
            BroadCastEndPoint = GetBroadCastEndPoint();
            Connection = new UdpClient();
        }

        public void ClearDiscoveredServers()
        {
            if(DiscoveredServers == null) {
                DiscoveredServers = new List<ClientServerInformation>();
            }

            DiscoveredServers.Clear();
        }

        public override bool HandlePackagaResponse(NetworkPackage package)
        {
            try {
                if (base.HandlePackagaResponse(package)) {
                    return true;
                }

                if (package.PackageType == NetworkPackageType.ServerDiscovertResponse) {
                    var clientServerInformation = ParseServerInformation(package);
                    DiscoveredServers.Add(clientServerInformation);
                    Console.WriteLine(String.Format("{0}\t {1}", DiscoveredServers.Count, clientServerInformation.DebugServerInfo()));
                }
            }catch(Exception e) {
                throw new BonaVirtualGamePadException("Failed to handle package, se inner exception", e);
            }

            return false;
        }

        public void SendDiscoveryBroadCast()
        {
            var package = CreateNetworkDiscoveryRequest();
            SendPackage(package, Connection, BroadCastEndPoint);
        }
    }
}
