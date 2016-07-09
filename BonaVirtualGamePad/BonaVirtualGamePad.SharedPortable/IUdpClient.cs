using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonaVirtualGamePad.Shared
{
    /* The UdpClien class and much of the System.Net namespace is not present in the PCL assembly but we need UDP conectivity
     * With the use if this interface, platformspecific implementation can be dependency injected
     * 
     * Auhtor: Björn Fyrvall*/
    public interface IUdpClient
    {
        String GetLocalIp();
        UdpIpEndPoint GetBroadCastEndPoint(int port);
        UdpIpEndPoint GetAnyEndPoint(int port);
        UdpIpEndPoint ParseIpEndPoint(String address, int port);

        void CreateWithEmptyEndPoint();
        void SetEndPoint(UdpIpEndPoint endPoint);
        List<NetworkPackage> PollNetworkPackages();
        void SendPackage(byte[] data, int packageLength, UdpIpEndPoint reciever);
    }
}
