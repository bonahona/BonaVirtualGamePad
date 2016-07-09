using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using BonaVirtualGamePad.Shared;

namespace BonaVirtualGamePad.SharedWindows
{
    public class WindowsUdpClient : IUdpClient
    {
        protected UdpClient InternalUdpClient;


        public UdpIpEndPoint GetBroadCastEndPoint(int port)
        {
            var broadcastEndpoint =  new IPEndPoint(IPAddress.Broadcast, port);
            var result = new UdpIpEndPoint(broadcastEndpoint.Address.ToString(), port);

            return result;
        }

        public UdpIpEndPoint GetAnyEndPoint(int port)
        {
            var anyEndPoint = new IPEndPoint(IPAddress.Any, port);
            var result = new UdpIpEndPoint(anyEndPoint.Address.ToString(), port);

            return result;
        }

        public string GetLocalIp()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList) {
                if (ip.AddressFamily == AddressFamily.InterNetwork) {
                    return ip.ToString();
                }
            }
            throw new BonaVirtualGamePadException("No local IPAddress found");
        }

        // This only creates an IP address for validation purposes. Its result contains a string representation of the IpAddress
        public UdpIpEndPoint ParseIpEndPoint(string address, int port)
        {
            try {
                var ipAddress = IPAddress.Parse(address);
                var result = new UdpIpEndPoint(ipAddress.ToString(), port);

                return result;
            } catch (Exception e) {
                throw new BonaVirtualGamePadException("Failed to create IPEndPoint", e);
            }
        }

        public void CreateWithEmptyEndPoint()
        {
            if(InternalUdpClient != null) {
                InternalUdpClient.Close();
            }

            InternalUdpClient = new UdpClient();
        }

        public void SetEndPoint(UdpIpEndPoint endPoint)
        {
            if(InternalUdpClient != null) {
                InternalUdpClient.Close();
            }

            InternalUdpClient = new UdpClient(ConvertUdpEndPointToIpEndPoint(endPoint));
        }

        public List<NetworkPackage> PollNetworkPackages()
        {
            var result = new List<NetworkPackage>();

            while (InternalUdpClient.Available > 0) {
                IPEndPoint source = new IPEndPoint(IPAddress.Any, Defaults.DEFAULT_SERVER_PORT);
                byte[] buffer = InternalUdpClient.Receive(ref source);
                int lastReadOffset = 0;
                while (lastReadOffset < buffer.Length) {
                    var networkPackage = new NetworkPackage();
                    lastReadOffset = networkPackage.Read(buffer, lastReadOffset);
                    networkPackage.SourceEndpoint = ConvertIpEndPointToUdpEndPoint(source);
                    result.Add(networkPackage);
                }
            }

            return result;
        }

        public void SendPackage(byte[] data, int packageLength, UdpIpEndPoint reciever)
        {
            InternalUdpClient.Send(data, packageLength, ConvertUdpEndPointToIpEndPoint(reciever));
        }

        public IPEndPoint ConvertUdpEndPointToIpEndPoint(UdpIpEndPoint udpIpEndPoint)
        {
            var result = new IPEndPoint(IPAddress.Parse(udpIpEndPoint.IpAddress), udpIpEndPoint.Port);
            return result;
        }

        public UdpIpEndPoint ConvertIpEndPointToUdpEndPoint(IPEndPoint ipEndPoint)
        {
            var result = new UdpIpEndPoint(ipEndPoint.Address.ToString(), ipEndPoint.Port);
            return result;
        }
    }
}
