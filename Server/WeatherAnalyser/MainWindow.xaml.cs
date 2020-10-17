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
using System.Net.Sockets;
using System.Net;
using System.ComponentModel;

namespace WeatherAnalyser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static void Error(string message)
        {
            MessageBox.Show(message);
        }
        Server s = new Server();
        public MainWindow()
        {
            InitializeComponent();
            
            s.SetServer();
            s.GetClientAsync();
            s.TryConnect();

            BackgroundWorker bg = new BackgroundWorker();
            bg.DoWork += ListUpdate;
            bg.RunWorkerAsync();
            
        }

        void ListUpdate(object sender,DoWorkEventArgs e)
        {
            while (true)
            {
                this.Dispatcher.Invoke((Action)delegate () { IpList.Items.Clear(); });
                foreach (IPAddress i in s.ClientIp)
                {
                    
                    this.Dispatcher.Invoke((Action)delegate(){ IpList.Items.Add(i.ToString()); });
                    
                    
                }
                System.Threading.Thread.Sleep(1000);
            }
            
        }
    }
}
