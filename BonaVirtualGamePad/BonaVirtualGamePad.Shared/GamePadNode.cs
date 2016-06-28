using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    }
}
