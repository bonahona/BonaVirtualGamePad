using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BonaVirtualGamePad.Shared
{
    public enum NetworkPackageType : int
    {
        Unknown                 = 0,
        ClientDiscoveryRequest  = 1,
        ServerDiscovertResponse = 2
    }
}
