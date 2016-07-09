using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace BonaVirtualGamePad.Shared
{
    public class GamePadServer : GamePadNode
    {
        public String ServerName;
        public String ServerPassword;

        public PlayerClient[] Clients;          // Array of all currently connected players

        public UdpIpEndPoint ListeningEndPoint;

        public GamePadServer(IUdpClient udpClient, IPlatform platform, String serverName, String serverPassword = "", int capacity = Defaults.DEFAULT_SERVER_CAPACITY, int listeningPort = Defaults.DEFAULT_SERVER_PORT) : base(udpClient, platform)
        {
            if (!ValidateServerName(serverName)) {
                throw new BonaVirtualGamePadException("Server name contains disallowed characters");
            }

            ServerName = serverName;
            ServerPassword = serverPassword;

            ListeningEndPoint = UdpClient.GetAnyEndPoint(listeningPort);
            UdpClient.SetEndPoint(ListeningEndPoint);

            Clients = new PlayerClient[capacity];
        }

        public override void ApplyDefaults()
        {
            // TODO: Implement this
        }

        public override void Update(float deltaTime)
        {
            // TODO: Implement this
        }

        public void SendNetworkDiscoveryResponse(UdpIpEndPoint localEndpoint, UdpIpEndPoint remoteEndpoint)
        {
            var package = CreateNetworkDiscoveryResponse(localEndpoint, this);
            SendPackage(package, remoteEndpoint);
        }

        public int GetPlayerCount()
        {
            var result = 0;

            for (int i = 0; i < Clients.Length; i++) {
                if (Clients[i] != null) {
                    result++;
                }
            }

            return result;
        }

        public int GetPlayerCapacity()
        {
            return Clients.Length;
        }
    }
}
