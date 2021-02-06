using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.ComponentModel;

namespace Back_Door
{

    class Server
    {
        public string Message = "";
        //SQL sql = new SQL();
        public enum IPtype { Wifi, Ethernet };
        static TcpListener server = null;
        public TcpClient client = default(TcpClient);

        public async void GetClientAsync()
        {

            while (true)
            {
                client = await server.AcceptTcpClientAsync();
                Console.WriteLine("Connected");
            }
        }
        public IPAddress getAdderss()
        {
            int[] piaddr = new int[4] { 192, 168, 43, 70 };
            byte[] ip = new byte[4];
            foreach (int i in piaddr)
            {
                ip[Array.IndexOf(piaddr, i)] = Convert.ToByte(i);
            }
            IPAddress LocalAddress = new IPAddress(ip);

            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

            return LocalAddress;
        }

        public IPAddress getAdderss(int[] Addr)
        {
            int[] piaddr = Addr;
            byte[] ip = new byte[4];
            foreach (int i in piaddr)
            {
                ip[Array.IndexOf(piaddr, i)] = Convert.ToByte(i);
            }
            IPAddress LocalAddress = new IPAddress(ip);

            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

            return LocalAddress;
        }


        public void SendData(string msg)
        {
            NetworkStream nStream = client.GetStream();
            byte[] _buffer = ASCIIEncoding.ASCII.GetBytes(msg);
            nStream.Write(_buffer, 0, _buffer.Length);
        }

        public void SetServer(int port)
        {

            IPAddress IPAddr = getAdderss();

            server = new TcpListener(IPAddr,port);
            server.Start();

            Console.WriteLine("Starting Server...");


        }

        public void SetServer(int[] IP,int port)
        {

            IPAddress IPAddr = getAdderss(IP);

            server = new TcpListener(IPAddr, port);
            server.Start();

            Console.WriteLine("Starting Server...");


        }

        public void RecieveDataAsync(int Bytes)
        {
            NetworkStream nStream = client.GetStream();


            byte[] _buffer = new byte[Bytes];
            while (true)
            {
                try
                {
                    nStream.ReadAsync(_buffer, 0, _buffer.Length);
                    string Client_Message = ASCIIEncoding.ASCII.GetString(_buffer);
                    Message = Client_Message.Replace("\0", "");
                    nStream.Flush();
                    if (Message.Length > 0)
                    {
                        Console.WriteLine("Msg Recieved");
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

    }
}
