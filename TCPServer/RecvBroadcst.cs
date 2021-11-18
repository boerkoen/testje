using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Remote
{
    public class RecvBroadcst
    {
        public static List<byte[]> receivedinputcommands = new List<byte[]>();
        public RecvBroadcst()
        {
            Thread receive = new Thread(new ThreadStart(Receivepackets));
            receive.IsBackground = true;
            receive.Start();
        }
        void Receivepackets()
        {
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint iep = new IPEndPoint(IPAddress.Any, 9050);
            sock.Bind(iep);
            EndPoint ep = (EndPoint)iep;
            Console.WriteLine("Ready to receive...");

            byte[] data = new byte[1024];
            while (true)
            {
                int recv = sock.ReceiveFrom(data, ref ep);
                receivedinputcommands.Add(data);
            }
        }
    }
}
