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
using System.Windows.Threading;

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

        public void CheckData()
        {
            while (true)
            {
                if (RecvBroadcst.receivedinputcommands.Count() != 0)
                {
                    byte[] received = RecvBroadcst.receivedinputcommands.First();
                    RecvBroadcst.receivedinputcommands.Remove(received);
                    //if (received[0] == 0) // 0 = TV
                    if (received[0]==0 | received[0]==1) // only during debug to see also the commands from the remote
                    {
                        //var str = System.Text.Encoding.Default.GetString(received).Substring(1,received.Length-1);
                        Dispatcher.BeginInvoke(new Action(() => {
                            Textboxinfo.Text += $"{received[1].ToString()}{Environment.NewLine}"; //replace received[1] by str
                        }), DispatcherPriority.SystemIdle);
                    }
                }
            }
        }

        private void btn_Remote_Click(object sender, RoutedEventArgs e)
        {            
            SendBroadcast.data[1] = Convert.ToByte((sender as Button).Tag);
            //Textboxinfo.AppendText($"Sending Byte 0:{SendBroadcast.data[0]} and byte 1:{SendBroadcast.data[1]}\r\n");
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
