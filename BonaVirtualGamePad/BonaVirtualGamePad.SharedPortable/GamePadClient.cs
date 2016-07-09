using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace BonaVirtualGamePad.Shared
{
    public class GamePadClient : GamePadNode
    {
        public PlayerIdentity PlayerIdentity { get; set; }

        public UdpIpEndPoint BroadCastEndPoint { get; set; }
        public UdpIpEndPoint HostEndPoint { get; set; }
        public UdpIpEndPoint LocalEndPoint { get; set; }

        // List of all available servers who have responded to the discovery request
        public List<ClientServerInformation> DiscoveredServers { get; set; }

        // Server the client has connected to (or is trying). Null means it is not connect not is it trying to connect to anything)
        public ClientServerInformation ConnectedServer { get; set; }
        public ClientStatus ConnectionStatus { get; set; }

        // State data for a connection attempt
        public int ConnectionAttemptMax { get; set; }
        public int ConnectionAttemptCurrent { get; set; }
        public float ConnectionTimerMax { get; set; }
        public float ConnectionTimerCurrent { get; set; }

        public event ServerDiscoveredEvent OnServerDiscovered;
        public event ServerGeneralEvent OnServerJoinAccept;
        public event ServerGeneralEvent OnServerJoinDenied;
        public event ServerGeneralEvent OnServerDropped;

        public GamePadClient(IUdpClient udpClient, IPlatform platform) : base(udpClient, platform)
        {
            udpClient.CreateWithEmptyEndPoint();
            BroadCastEndPoint = udpClient.GetBroadCastEndPoint(Defaults.DEFAULT_SERVER_PORT);

            // TODO: Change this to a proper identity
            PlayerIdentity = PlayerIdentity.GenerateNew("Test Player");
        }

        public override void Update(float deltaTime)
        {

        }

        public override void ApplyDefaults()
        {
            ConnectionAttemptCurrent = 0;
            ConnectionAttemptMax = Defaults.CLIENT_CONNECTION_ATTEMPTS;
            ConnectionTimerCurrent = 0;
            ConnectionTimerMax = Defaults.CLIENT_CONNECTION_ATTEMPT_TIMER;
        }

        public void InvokeOnServerDiscovered(ClientServerInformation clientServerInformation, int serverIndex)
        {
            if(OnServerDiscovered != null) {
                OnServerDiscovered(this, clientServerInformation, serverIndex);
            }
        }

        public void InvokeOnServerJoinAccept(ClientServerInformation clientServerInformation)
        {
            if(OnServerJoinAccept != null) {
                OnServerJoinAccept(this, clientServerInformation);
            }
        }

        public void InvokeOnServerJoinDenied(ClientServerInformation clientServerInformation)
        {
            if(OnServerJoinDenied != null) {
                OnServerJoinDenied(this, clientServerInformation);
            }
        }

        public void InvokeServerJoin(ClientServerInformation clientServerInformation)
        {
            if(OnServerDropped != null) {
                OnServerDropped(this, clientServerInformation);
            }
        }

        public void ClearDiscoveredServers()
        {
            if(DiscoveredServers == null) {
                DiscoveredServers = new List<ClientServerInformation>();
            }

            DiscoveredServers.Clear();
        }

        public override bool HandlePackageResponse(NetworkPackage package)
        {
            try {
                if (base.HandlePackageResponse(package)) {
                    return true;
                }

                if (package.PackageType == NetworkPackageType.ServerDiscovertResponse) {
                    var clientServerInformation = ParseServerInformation(package);
                    AddDiscoveredServer(clientServerInformation);
                }
            }catch(Exception e) {
                throw new BonaVirtualGamePadException("Failed to handle package, se inner exception", e);
            }

            return false;
        }

        public void AddDiscoveredServer(ClientServerInformation clientServerInformation)
        {
            DiscoveredServers.Add(clientServerInformation);
            InvokeOnServerDiscovered(clientServerInformation, DiscoveredServers.Count -1);
        }

        public void SendDiscoveryBroadCast()
        {
            var package = CreateNetworkDiscoveryRequest();
            SendPackage(package, BroadCastEndPoint);
        }

        public void ConnectToServer(ClientServerInformation clientServerInformation, String password = "")
        {
            ConnectedServer = clientServerInformation;
            SendJoinRequest(clientServerInformation, PlayerIdentity, password);
            ConnectionStatus = ClientStatus.Connecting;
        }

        public void SendJoinRequest(ClientServerInformation server, PlayerIdentity playerIdentity, String password)
        {
            var package = CreatePlayerJoinRequest(server, playerIdentity, password);
            SendPackage(package, server.ServerEndPoint);
        }
    }
}
