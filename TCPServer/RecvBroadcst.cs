using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Remote
{
    public class RecvBroadcst
    {
        public static List<string> receivedinputcommands = new List<string>();
        public RecvBroadcst()
        {
            Thread receive = new Thread(new ThreadStart(Receivepackets));
            receive.IsBackground = true;
            receive.Start();
        }
        void Receivepackets()
        {
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint iep = new IPEndPoint(IPAddress.Any, 9052);
            sock.Bind(iep);
            EndPoint ep = (EndPoint)iep;
            Console.WriteLine("Ready to receive...");

            byte[] data = new byte[1024];
            while (true)
            {
                int recv = sock.ReceiveFrom(data, ref ep);
                string stringData = Encoding.ASCII.GetString(data, 0, recv);
                receivedinputcommands.Add(stringData);
                

            }
        }
    }
}
