using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TCPServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        RecvBroadcst Receiver = new RecvBroadcst();
      
        public MainWindow()
        {
            InitializeComponent();
            SendBroadcast.init();
            SendBroadcast.data[0] = 1; // BYTE 0 = SENDER, (1= REMOTE, 0 = TV)
            Thread t = new Thread(CheckData);
            t.Start();
        }

        static void CheckData(object obj)
        {
            while (true)
            {
                if (RecvBroadcst.receivedinputcommands.Count() != 0)
                {
                    ulong inputje = RecvBroadcst.receivedinputcommands.First();
                    RecvBroadcst.receivedinputcommands.Remove(inputje);
                    MessageBox.Show(string.Format("0x{0:X}", inputje)); // nog met een delegate werken waarschijnlijk om de data op de textboxinfo te krijgen...
                    //Textboxinfo.AppendText(string.Format("0x{0:X}", inputje));
                }
            }
        }

        private void btn_OnOff_Click(object sender, RoutedEventArgs e)
        {            
            SendBroadcast.data[1] = Convert.ToByte((sender as Button).Tag);
            Textboxinfo.AppendText($"Sending Byte 0:{SendBroadcast.data[0]} and byte 1:{SendBroadcast.data[1]}\r\n");
            SendBroadcast.sendb();
        }
    }


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

    public class RecvBroadcst
    {
        public static List<ulong> receivedinputcommands = new List<ulong>();
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
                ulong value = BitConverter.ToUInt64(data, 0);
                receivedinputcommands.Add(value);        
            }
        }
    }
}
