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
        public IPEndPoint EndPoint;
        public PLayerClientStatus Status;
        public DateTime LastReievedPackage;

        public PlayerClient(byte[] identity, IPEndPoint endPoint)
        {
            Identity = new PlayerIdentity(identity);
            EndPoint = endPoint;
        }
    }
}
