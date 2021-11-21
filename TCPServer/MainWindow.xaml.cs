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
using System.Data.SqlClient;

namespace Remote
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
            radioSocket.IsChecked = true;
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
            if (radioDB.IsChecked == true)
            {
                var query = "Insert into Commands(button) values (@button)";
                //using (var conn = new SqlConnection("Data Source=localhost; Initial Catalog=Television; User ID=sa; Password=syntrawest1234A"))
                using (var conn = new SqlConnection("Data Source=localhost; Initial Catalog=Television; User ID=sa; Password=1234"))
                using (var command = new SqlCommand(query, conn))
                {
                    conn.Open();
                    command.Parameters.AddWithValue("@button", Convert.ToByte((sender as Button).Tag));
                    command.ExecuteNonQuery();
                }
            }
            else
            {
                SendBroadcast.data[1] = Convert.ToByte((sender as Button).Tag);            
                SendBroadcast.sendb();
            }



        }

  
    }





}
