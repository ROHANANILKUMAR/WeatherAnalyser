using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.ComponentModel;

namespace WeatherAnalyser
{

    class Server
    {
        string Message = "";
        SQL sql = new SQL();
        public enum IPtype { Wifi, Ethernet };
        static TcpListener server = null;
        public List<TcpClient> clients;
        public TcpClient client = default(TcpClient);
        public List<IPAddress> ClientIp = new List<IPAddress>();
        int requestCount = 0;

        public async void GetClientAsync()
        {
            while (true)
            {
                client = await server.AcceptTcpClientAsync();

                
                RecieveDataAsync(client,10);                        

                string serial = Message;


                if(serial.Length>0 && sql.GetSerial().Contains(serial))
                {
                    clients.Add(client);
                    ClientIp.Add(IPAddress.Parse(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString()));
                    SendData(client,"Ok");
                }
                
            }
        }

        public IPAddress getAdderss()
        {
            int[] piaddr = new int[4] { 192,168,43,70 };
            byte[] ip = new byte[4];
            foreach (int i in piaddr)
            {
                ip[Array.IndexOf(piaddr, i)] = Convert.ToByte(i);
            }
            IPAddress LocalAddress = new IPAddress(ip);

            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

            return LocalAddress;
        }


        public void SendData(TcpClient client,string msg)
        {
            NetworkStream nStream = client.GetStream();
            byte[] _buffer = ASCIIEncoding.ASCII.GetBytes(msg);
            nStream.Write(_buffer, 0, _buffer.Length);
        }

        public void SetServer()
        {
            clients = new List<TcpClient>();

            IPAddress IPAddr = getAdderss();

            server = new TcpListener(IPAddr, 8888);
            server.Start();

            Console.WriteLine("Starting Server...");

            requestCount = 0;

        }
        public void RecieveDataAsync(TcpClient client,int Bytes)
        {
            NetworkStream nStream = client.GetStream();

            requestCount++;

            byte[] _buffer = new byte[Bytes];
            while (true)
            {
                try
                {
                    nStream.ReadAsync(_buffer, 0, _buffer.Length);
                    string Client_Message = ASCIIEncoding.ASCII.GetString(_buffer);
                    Message= Client_Message.Replace("\0","");
                    nStream.Flush();
                    if (Message.Length > 0)
                    {
                        //MainWindow.Error("Break");
                        break;
                    }
                }
                catch { }

            }


        }
        public void Close()
        {
            client.Close();
            server.Stop();
            Console.WriteLine("Exit");
            Console.ReadLine();
        }

        public void TryConnect()
        {
            BackgroundWorker bg = new BackgroundWorker();
            bg.DoWork += TryConnect;
            bg.RunWorkerAsync();
        }

        public void TryConnect(object sender,DoWorkEventArgs e)
        {
            while (true)
            {
                List<TcpClient> junk = new List<TcpClient>();
                try
                {
                    foreach (TcpClient c in clients)
                    {
                        if (!(c.Connected))
                        {
                            MainWindow.Error("Junk");
                            junk.Add(c);
                        }
                    }
                    foreach (TcpClient c in junk)
                    {
                        clients.Remove(c);
                    }
                }
                catch { }
                
            }
            
        }
    }
}
