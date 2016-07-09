using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace BonaVirtualGamePad.Shared
{
    /* Contains the aggregated information of a server, built from a Discovery response, and stored data about a server 
     * so it can connect at a later moment
     * 
     * Author: Björn Fyrvall*/
    public class ClientServerInformation
    {
        public String ServerName { get; set; }
        public UdpIpEndPoint ServerEndPoint { get; set; }

        public bool UsePassword { get; set; }
        public int Capacity { get; set; }
        public int PlayerCount { get; set; }

        public String DebugServerInfo()
        {
            return String.Format("Name: {0}, Server: {1}, Port: {2}, Playes; {3}/{4}, Use Password: {5}", ServerName, ServerEndPoint.IpAddress, ServerEndPoint.Port, PlayerCount, Capacity, UsePassword);
        }
    }
}
