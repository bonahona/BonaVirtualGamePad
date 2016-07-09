using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BonaVirtualGamePad.Shared
{
    // Status a GameNodeClient can have with regards to it trying to connect to a server
    public enum ClientStatus : int
    {
        Unknown         = 0,
        Connecting      = 1,
        Connected       = 2,
        Disconnected    = 3,
        Dropped         = 4
    }
}
