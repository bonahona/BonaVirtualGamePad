using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace BonaVirtualGamePad.Shared
{
    public abstract class GamePadNode
    {
        protected IPEndPoint ParseIpEndPoint(String listeningAddress, int listeningPort)
        {
            try{
                IPAddress ipAddress = IPAddress.Parse(listeningAddress);
                IPEndPoint result = new IPEndPoint(ipAddress, listeningPort);

                return result;
            } catch(Exception e){
                throw new BonaVirtualGamePadException("Failed to create IPEndPoint", e);
            }
        }

        protected String GetLocalIp()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList) {
                if (ip.AddressFamily == AddressFamily.InterNetwork) {
                    return ip.ToString();
                }
            }
            throw new BonaVirtualGamePadException("No local IPAddress found");
        }

        protected IPEndPoint GetBroadCastEndPoint(int listeningPort = Defaults.DEFAULT_SERVER_PORT)
        {
            return new IPEndPoint(IPAddress.Broadcast, listeningPort);
        }

        public List<NetworkPackage> PollPackages(UdpClient udpClient)
        {
            List<NetworkPackage> result = new List<NetworkPackage>();

            if (udpClient.Available > 0) {
                IPEndPoint source = new IPEndPoint(IPAddress.Any, Defaults.DEFAULT_SERVER_PORT);
                var networkPackage = new NetworkPackage();
                networkPackage.Read(udpClient.Receive(ref source));
                networkPackage.SourceEndpoint = source;
                result.Add(networkPackage);
            }

            return result;
        }

        public void SendPackage(NetworkPackage package, UdpClient udpClient, IPEndPoint targetEndpoint)
        {
            var dataToSend = package.ToByteArray();
            udpClient.Send(dataToSend, dataToSend.Length, targetEndpoint);
        }

        public NetworkPackage CreateNetworkDiscoveryRequest()
        {
            var result = new NetworkPackage(NetworkPackageType.ClientDiscoveryRequest);
            return result;
        }

        public NetworkPackage CreateNetworkDiscoveryResponse(IPEndPoint listeningEndpoint)
        {
            var result = new NetworkPackage(NetworkPackageType.ServerDiscovertResponse);

            var localIp = GetLocalIp();
            var listeningPort = listeningEndpoint.Port;

            result.AdditionalData = String.Format("address={0};port={1}", localIp, listeningEndpoint.Port);
            return result;
        }
    }
}
