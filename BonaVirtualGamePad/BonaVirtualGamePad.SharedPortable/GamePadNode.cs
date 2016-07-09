using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace BonaVirtualGamePad.Shared
{
    public delegate void ServerGeneralEvent(GamePadClient client, ClientServerInformation server);
    public delegate void ServerDiscoveredEvent(GamePadClient client, ClientServerInformation server, int serverIndex);
    
    public abstract class GamePadNode
    {
        // These characters are used in to protocol and must therefor not be present in the name of a server. They are allowed in the password as that is sent hashed
        public static readonly String[] DISALLOWED_SERVER_NAME_CHARACTERS = { ";", "=" };

        public IUdpClient UdpClient { get; set; }       // Wrapper for UdpClient connectivity
        public IPlatform Plaform { get; set; }          // Allows injection of platform specific funtionality like file reading and writing

        public abstract void Update(float deltaTime);
        public abstract void ApplyDefaults();

        public GamePadNode(IUdpClient udpClient, IPlatform platform)
        {
            UdpClient = udpClient;
            Plaform = platform;
            ApplyDefaults();
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

        public virtual bool HandlePackageResponse(NetworkPackage package)
        {
            return false;
        }

        public ClientServerInformation ParseServerInformation(NetworkPackage package)
        {
            var result = new ClientServerInformation();

            var keyValuePairs = ParseAdditionalDataString(package.AdditionalData);
            result.ServerName = keyValuePairs["name"];
            result.ServerEndPoint = new UdpIpEndPoint(keyValuePairs["address"], int.Parse(keyValuePairs["port"]));
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

                // Handle empty string (The last entry in the list will be empty due to a trailing semicolon)
                if (packageDataPair != String.Empty) {
                    var tmpSplit = packageDataPair.Split('=');
                    result.Add(tmpSplit[0], tmpSplit[1]);
                }
            }

            return result;
        }

        public List<NetworkPackage> PollNetworkPackages()
        {
            return UdpClient.PollNetworkPackages();
        }

        public void SendPackage(NetworkPackage package, UdpIpEndPoint targetEndpoint)
        {
            var dataToSend = package.ToByteArray();
            UdpClient.SendPackage(dataToSend, dataToSend.Length, targetEndpoint);
        }

        public NetworkPackage CreateNetworkDiscoveryRequest()
        {
            var result = new NetworkPackage(NetworkPackageType.ClientDiscoveryRequest);
            return result;
        }

        public NetworkPackage CreateNetworkDiscoveryResponse(UdpIpEndPoint listeningEndpoint, GamePadServer gamePadServer)
        {
            var result = new NetworkPackage(NetworkPackageType.ServerDiscovertResponse);

            var localIp = UdpClient.GetLocalIp();
            var listeningPort = listeningEndpoint.Port;

            var usePassword = (gamePadServer.ServerPassword != String.Empty);

            var data = new Dictionary<String, object>();
            data.Add("address", localIp);
            data.Add("port", listeningEndpoint.Port);
            data.Add("name", gamePadServer.ServerName);
            data.Add("capacity", gamePadServer.GetPlayerCapacity());
            data.Add("players", gamePadServer.GetPlayerCount());
            data.Add("usepassword", usePassword);
            result.AdditionalData = result.CreateAdditionalDataString(data);

            return result;
        }

        public NetworkPackage CreatePlayerJoinRequest(ClientServerInformation server, PlayerIdentity player, String password = "")
        {
            var result = new NetworkPackage(NetworkPackageType.ClientPlayerJoinRequest);

            var data = new Dictionary<String, object>();
            data.Add("playeridentity", player.ToString());
            data.Add("playername", player.Name);
            
            // If a password is provided, add it SHA1 hashed to the package
            if(password != String.Empty) {
                data.Add("password", Plaform.HashString(password));
            }

            result.AdditionalData = result.CreateAdditionalDataString(data);
            return result;
        }
    }
}
