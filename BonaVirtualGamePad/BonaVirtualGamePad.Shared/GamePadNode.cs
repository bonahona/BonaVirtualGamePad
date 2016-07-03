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
        // These characters are used in to protocol and must therefor not be present in the name of a server. They are allowed in the password as that is sent hashed
        public static readonly char[] DISALLOWED_SERVER_NAME_CHARACTERS = { ';', '=' };

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

        public bool ValidateServerName(String serverName)
        {
            foreach(var disallowedChar in DISALLOWED_SERVER_NAME_CHARACTERS) {
                if (serverName.Contains(disallowedChar)) {
                    return false;
                }
            }

            return true;
        }

        public List<NetworkPackage> PollPackages(UdpClient udpClient)
        {
            var result = new List<NetworkPackage>();

            while (udpClient.Available > 0) {
                IPEndPoint source = new IPEndPoint(IPAddress.Any, Defaults.DEFAULT_SERVER_PORT);
                byte[] buffer = udpClient.Receive(ref source);
                int lastReadOffset = 0;
                while(lastReadOffset < buffer.Length){
                    var networkPackage = new NetworkPackage();
                    lastReadOffset = networkPackage.Read(buffer, lastReadOffset);
                    networkPackage.SourceEndpoint = source;
                    result.Add(networkPackage);
                }
            }

            return result;
        }

        public virtual bool HandlePackagaResponse(NetworkPackage package)
        {
            return false;
        }

        public ClientServerInformation ParseServerInformation(NetworkPackage package)
        {
            var result = new ClientServerInformation();

            var keyValuePairs = ParseAdditionalDataString(package.AdditionalData);
            result.ServerName = keyValuePairs["name"];
            result.ServerEndPoint = ParseIpEndPoint(keyValuePairs["address"], int.Parse(keyValuePairs["port"]));
            result.Capacity = int.Parse(keyValuePairs["capacity"]);
            result.PlayerCount = int.Parse(keyValuePairs["players"]);
            result.UsePassword = bool.Parse(keyValuePairs["usepassword"]);

            return result;
        }

        public Dictionary<String, String> ParseAdditionalDataString(String additionalData)
        {
            var result = new Dictionary<String, String>();
            var packageDataPairs = additionalData.Split(';');

            foreach (var packageDataPair in packageDataPairs) {
                var tmpSplit = packageDataPair.Split('=');
                result.Add(tmpSplit[0], tmpSplit[1]);
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

        public NetworkPackage CreateNetworkDiscoveryResponse(IPEndPoint listeningEndpoint, GamePadServer gamePadServer)
        {
            var result = new NetworkPackage(NetworkPackageType.ServerDiscovertResponse);

            var localIp = GetLocalIp();
            var listeningPort = listeningEndpoint.Port;

            var usePassword = (gamePadServer.ServerPassword != String.Empty);
            result.AdditionalData = String.Format("address={0};port={1};name={2};capacity={3};players={4};usepassword={5}", localIp, listeningEndpoint.Port, gamePadServer.ServerName, gamePadServer.GetPlayerCapacity(), gamePadServer.GetPlayerCount(), usePassword);
            return result;
        }
    }
}
