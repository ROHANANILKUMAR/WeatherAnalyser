using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.ComponentModel;
using System.IO;

namespace WeatherAnalyser
{

    class Server
    {
        public Message Message;
        SQL sql = new SQL();
        public enum IPtype { Wifi, Ethernet };
        static TcpListener server = null;
        public List<TcpClient> clients;
        public TcpClient client = default(TcpClient);
        public List<IPAddress> ClientIp = new List<IPAddress>();
        int requestCount = 0;
        public Dictionary<IPAddress, Dictionary<string, string>> ClientData = new Dictionary<IPAddress, Dictionary<string, string>>();
        public Dictionary<IPAddress, TcpClient> ClientIPMap = new Dictionary<IPAddress, TcpClient>();
        public EventHandler<Message> MessageRecieved;
        public EventHandler<IPAddress> NewClient;
        public EventHandler<IPAddress> ClientDisconnected;

        public void LoadClientData(IPAddress IP,string clientserial)
        {
            //MainWindow.Error(clientserial);
            Dictionary<string, string> temp = sql.GetSingleColumn("users","SerialNo",clientserial,new string[] {"Name","SerialNo","PhNo","Address","Location" });
            ClientData[IP] = temp;
            
        }

        public async void GetClientAsync()
        {
            while (true)
            {
                //MainWindow.Error("Waiting for connection");
                
                client = await server.AcceptTcpClientAsync();
               // MainWindow.Error("Connected");
                if (!(ClientIp.Contains(IPAddress.Parse(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString()))))
                {
                    RecieveData(client, 10);

                    string serial = Message.Data;


                    if (serial.Length > 0 && sql.GetSerial().Contains(serial))
                    {
                        IPAddress ClientIpAddress = IPAddress.Parse(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString());
                        

                        clients.Add(client);
                        ClientIp.Add(ClientIpAddress);
                        SendData(client, "Ok");

                        LoadClientData(ClientIpAddress,serial);
                    }
                    else
                    {
                        SendData(client, "Denied");
                        client.Dispose();
                    }
                }
                else
                {
                    IPAddress ClientIpAddress = IPAddress.Parse(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString());
                    ClientIPMap[ClientIpAddress] = client;
                    SendData(client, "Ok");
                    NewClient(client, ClientIpAddress);
                    //RecieveEmergencySignal(IPAddress.Parse(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString()));
                    RecieveDataAsync(client, 1024);
                    //KeepAlive(client);
                }
                
                
                
            }
        }

        public IPAddress getAdderss(byte[] ip)
        {
            IPAddress LocalAddress = new IPAddress(ip);

            return LocalAddress;
        }

        public void CleanUp(IPAddress I)
        {
            clients.Remove(ClientIPMap[I]);
            ClientData.Remove(I);
            ClientIp.Remove(I);
            ClientIPMap.Remove(I);
            ClientDisconnected(this,I);
        }


        public void SendData(TcpClient client,string msg)
        {
            try
            {
                NetworkStream nStream = client.GetStream();
                byte[] _buffer = ASCIIEncoding.ASCII.GetBytes(msg);
                nStream.Write(_buffer, 0, _buffer.Length);
            }
            catch
            {
                IPAddress ClientIpAddress = IPAddress.Parse(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString());
                CleanUp(ClientIpAddress);
            }
            
            
        }

        public void SetServer()
        {
            int[] addr=new int[4];
            byte[] ip = new byte[4];

            string data = File.ReadAllText(@"G:\Science Exhibition\connection.txt");
            string ipaddr = data.Split(';')[0];
            int port = Convert.ToInt16(data.Split(';')[2]);
            string[] ip_data = ipaddr.Split('.');

            for (int i = 0; i < ip_data.Length; i++) addr[i] = Convert.ToInt16(ip_data[i]);

            foreach (int i in addr)
            {
                ip[Array.IndexOf(addr, i)] = Convert.ToByte(i);
            }


            clients = new List<TcpClient>();

            IPAddress IPAddr = getAdderss(ip);

            server = new TcpListener(IPAddr, port);
            server.Start();

            Console.WriteLine("Starting Server...");

            requestCount = 0;

            MessageRecieved += NewMessage;

        }

        

        public void NewMessage(object sender,Message m)
        {

        }

        public void RecieveData(TcpClient client,int Bytes)
        {
            try
            {
                NetworkStream nStream = client.GetStream();

                requestCount++;

                byte[] _buffer = new byte[Bytes];

                nStream.Read(_buffer, 0, _buffer.Length);
                string Client_Message = ASCIIEncoding.ASCII.GetString(_buffer);
                Message = new Message(client, Client_Message.Replace("\0", ""));
                nStream.Flush();
            }  
             catch
            {
                IPAddress ClientIpAddress = IPAddress.Parse(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString());
                CleanUp(ClientIpAddress);
            }
        }

        public async Task RecieveDataAsync(TcpClient client, int Bytes)
        {
            while (true)
            {
                NetworkStream nStream = client.GetStream();

                requestCount++;

                byte[] _buffer = new byte[Bytes];

                await nStream.ReadAsync(_buffer, 0, _buffer.Length);
                string Client_Message = ASCIIEncoding.ASCII.GetString(_buffer);
                Message = new Message(client, Client_Message.Replace("\0", ""));
                nStream.Flush();

                MessageRecieved(client, Message);

            }
            
        }

        public async Task KeepAlive(TcpClient c)
        {
            while (clients.Contains(c))
            {
                System.Threading.Thread.Sleep(5000);
                SendData(c, "Alive");
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
                            //MainWindow.Error("Junk");
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

