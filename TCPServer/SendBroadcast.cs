using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Remote
{
    public static class SendBroadcast
    {
        public static byte[] data = new byte[2];
        public static Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        public static IPEndPoint iep1 = new IPEndPoint(IPAddress.Broadcast, 9050);

        public static void init()
        {
            sock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
        }

        public static void sendb()
        {
            sock.SendTo(data, iep1);
        }

    }
}
