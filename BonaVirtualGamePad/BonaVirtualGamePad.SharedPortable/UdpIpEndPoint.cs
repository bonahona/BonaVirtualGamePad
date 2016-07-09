using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonaVirtualGamePad.Shared
{
    public delegate void IpAddressChanged(UdpIpEndPoint endPoint);

    public class UdpIpEndPoint
    {
        protected String m_ipAddress;
        protected int m_port;

        public String IpAddress {
            get { return m_ipAddress; }
            set {
                m_ipAddress = value;
                InvokeOnIpAddressChanged();
            }
        }

        public int Port {
            get { return m_port; }
            set {
                m_port = value;
                InvokeOnIpAddressChanged();
            }
        }
        public event IpAddressChanged OnIpAddressChanged;

        public UdpIpEndPoint()
        {
            IpAddress = "0.0.0.0";
            Port = 0;
        }

        public UdpIpEndPoint(String ipAddress, int port)
        {
            IpAddress = ipAddress;
            Port = port;
        }

        public void InvokeOnIpAddressChanged()
        {
            if(OnIpAddressChanged != null) {
                OnIpAddressChanged(this);
            }
        }
    }
}
