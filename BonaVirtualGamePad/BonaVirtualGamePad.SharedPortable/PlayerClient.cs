using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace BonaVirtualGamePad.Shared
{
    public class PlayerClient
    {
        public PlayerIdentity Identity;
        public UdpIpEndPoint EndPoint;
        public PLayerClientStatus Status;
        public DateTime LastReievedPackage;

        public PlayerClient(PlayerIdentity playerIdentity, UdpIpEndPoint endPoint)
        {
            Identity = playerIdentity;
            EndPoint = endPoint;
        }
    }
}
