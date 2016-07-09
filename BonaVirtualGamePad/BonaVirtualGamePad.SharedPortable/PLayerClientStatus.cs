using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BonaVirtualGamePad.Shared
{
    // Status a PlayerClient entry on the GameNodeServer can have.
    public enum PLayerClientStatus : int
    {
        Unknown         = 0,
        Connected       = 1,
        NotConnected    = 2
    }
}
