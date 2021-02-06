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
        Dictionary<IPAddress, Control[]> IpListMappings = new Dictionary<IPAddress, Control[]>();
        Dictionary<IPAddress, Grid> IpGridMappings = new Dictionary<IPAddress, Grid>();

        SolidColorBrush DefaultWindowColor;
        

        public MainWindow()
        {
            InitializeComponent();
            //DataList.SelectedIndex = DataList.Items.Count - 1;
            //DataList.SelectedIndex = -1;
            UsrDataGrid.Visibility = Visibility.Hidden;
            LogData.Visibility = Visibility.Hidden;
            Controls.Visibility = Visibility.Hidden;

            DefaultWindowColor = (SolidColorBrush)Root.Background;

            s.SetServer();
            s.GetClientAsync();
            s.TryConnect();

            s.NewClient += ListUpdate;

            s.MessageRecieved += ClientMessageRecieved;
            s.ClientDisconnected += Disconnected;

        }

        private void Disconnected(object sender,IPAddress i)
        {
            this.Dispatcher.Invoke((Action)delegate{ IpList.Items.Remove(IpGridMappings[i]); });
            
        }

        private void SetColor(Control[] l,Color c)
        {
            foreach(Label i in l)
            {
                i.Foreground = new SolidColorBrush(c);
            }
        }

        bool logged = false;

        private void ClientMessageRecieved(object sender,Message m)
        {
           
            if (m.Data.StartsWith("Err:") && !logged)
            {
                bool human = m.Data.Split(':')[3].Equals("1") ? true : false;
                int per=Convert.ToInt16(m.Data.Split(':')[2]);
                string errmsg = m.Data.Split(':')[1];
                switch (per)
                {
                    case 100:
                        SetColor(IpListMappings[m.IP],Color.FromRgb(255,0,0));
                        Root.Background = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                        break;
                    case 75:
                        SetColor(IpListMappings[m.IP], Color.FromRgb(255,50,0));
                        break;
                    case 50:
                        SetColor(IpListMappings[m.IP], Color.FromRgb(255, 90, 20));
                        break;
                    case 25:
                        SetColor(IpListMappings[m.IP], Color.FromRgb(0, 0,255));
                        break;
                }
                Log l = new Log(m.IP,errmsg,per,s.ClientData[m.IP],human);
                LogList.Items.Add(l);
                logged = true;
            }
            else if (m.Data.StartsWith("EndEmer"))
            {
                foreach (Label i in IpListMappings[m.IP])
                {
                    i.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                }
                Root.Background = DefaultWindowColor;
                logged = false;
            }
            else if (m.Data.StartsWith("dta"))
            {
                 DataList.Items.Add(Decode(m.Data.Replace("dta","")));
                UpdateScrollBar(DataList);
                if (CheckPresence)
                {
                    CheckPresence = false;
                    bool result = m.Data.Split(',')[5].Equals("1")?true:false;
                    HumanResponse.Text = result.ToString();
                }
            }
            
            if (ClosingMsg)
            {
                if (m.Data.Equals("Shut"))
                {
                    s.CleanUp(m.IP);

                }

            }
            
        }

        List<TcpClient> ClosingConfirmation = new List<TcpClient>();

        void ListUpdate(object sender,IPAddress i)
        {
                
            IpList.Items.Clear();
                
                
            Grid g = new Grid() { Height = 30, Width = 264,Name="g"+i.ToString().Replace(".","_") };
            Label ip = new Label() { Margin = new Thickness(0, 0, 166, 0), Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)), Content = i.ToString() };
            Label loc = new Label() { Margin = new Thickness(103, 0, 10,0), Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)), Content = s.ClientData[i]["Location"].ToString() };
            g.Children.Add(ip);
            g.Children.Add(loc);
            IpList.Items.Add(g);
            IpListMappings[i] = new Control[] { ip,loc};
            IpGridMappings[i] = g;
            UpdateScrollBar(IpList);
                    //IpList.SelectedIndex++;

        }

        IPAddress Selected_Client;
        
        private void IpList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IpList.SelectedItem != null)
            {
                Selected_Client = IPAddress.Parse(((Grid)IpList.SelectedItem).Name.Replace("_",".").Replace("g",""));
                Dictionary<string, string> CData = s.ClientData[Selected_Client];
                Usr.Text = CData["Name"];
                UsrLocation.Text = CData["Location"];
                UsrPhNo.Text = CData["PhNo"];
                UsrSer.Text = CData["SerialNo"];
                DataList.Items.Clear();
                DoDump = false;
                UsrDataGrid.Visibility = Visibility.Visible;
                Controls.Visibility = Visibility.Visible;
            }
            else
            {
                UsrDataGrid.Visibility = Visibility.Hidden;
                Controls.Visibility = Visibility.Hidden;
            }
        }

        BackgroundWorker Data_Dump_Worker = new BackgroundWorker();

        bool DoDump = false;

        private void Data_Dump_Click(object sender, RoutedEventArgs e)
        {
            if (Data_Dump.Content.Equals("Data Dump"))
            {
                DoDump = true;
                Data_Dump_Worker = new BackgroundWorker();
                Data_Dump_Worker.DoWork += Data_List_Update;
                Data_Dump_Worker.RunWorkerAsync();
                Data_Dump.Content = "Stop Dump";
            }
            else
            {
                DoDump = false;
                Data_Dump_Worker.WorkerSupportsCancellation = true;
                Data_Dump_Worker.CancelAsync();
                Data_Dump_Worker.Dispose();
                Data_Dump_Worker = null;
                Data_Dump.Content = "Data Dump";
            }
        }

        private static string Decode(string msg)
        {
            string res = "";

            string[] DataLayout = new string[] { "ldr", "temp", "humid", "rain", "gas", "man", "rainbool","flood", "mansig"};
            string[] data = msg.Replace(">", "").Replace("<", "").Split(',');

            for (int i = 0; i < DataLayout.Length; i++)
            {
                res += DataLayout[i] + ":" + data[i] + "; ";
            }

            return res;
        }

        private void Data_List_Update(object sender, DoWorkEventArgs e)
        {

            while (DoDump)
            {
                if (s.ClientIPMap.ContainsKey(Selected_Client))
                {
                    s.SendData(s.ClientIPMap[Selected_Client], "senval");
                    //s.SendData(s.client, "senval");
                    //DataList.Items.Add("Sending data");
                    //s.RecieveData(s.ClientIPMap[Selected_Client], 50);
                    //s.RecieveDataAsync(s.client, 50);
                    //UpdateScrollBar(DataList);
                    System.Threading.Thread.Sleep(1000);
                }

                
            }


        }

        bool CheckPresence = false;

        private void HPresence_Click(object sender, RoutedEventArgs e)
        {
            CheckPresence = true;
            s.SendData(s.ClientIPMap[Selected_Client], "senval");
        }

        private void LogList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LogList.SelectedItem != null)
            {
                LogData.Visibility = Visibility.Visible;
                Log l = (Log)LogList.SelectedItem;
                Log_From.Text = l.Details["Location"];
                Log_Response.Text = l.ErrMsg;
                Log_PhNo.Text = l.Details["PhNo"];
                Log_Per.Text = Convert.ToString(l.Per);
                Log_Name.Text = l.Details["Name"];
                Log_Time.Text = l.Date.Hour.ToString() + ":" + l.Date.Minute;
                Log_Casualty.Text = l.HumanPresent.ToString();
                UpdateScrollBar(LogList);
            }
            else
            {
                LogData.Visibility = Visibility.Hidden;
            }
            
        }

        private void UpdateScrollBar(ListBox listBox)
        {
            if (listBox != null)
            {
                var border = (Border)VisualTreeHelper.GetChild(listBox, 0);
                var scrollViewer = (ScrollViewer)VisualTreeHelper.GetChild(border, 0);
                scrollViewer.ScrollToBottom();
            }

        }

        private void DeleteSerialMapping_Click(object sender, RoutedEventArgs e)
        {
            SQL sql = new SQL();
            sql.DeleteData("users","serialno",s.ClientData[Selected_Client]["SerialNo"]);
            s.CleanUp(Selected_Client);
            MessageBox.Show("User deleted", "Successful");
        }

        bool ClosingMsg = false;

        private void ShutDown_Click(object sender, RoutedEventArgs e)
        {
            if (Selected_Client!=null)
            {
                s.SendData(s.ClientIPMap[Selected_Client], "Closing");
            }
            ClosingMsg = true;
        }
    }
       
}
