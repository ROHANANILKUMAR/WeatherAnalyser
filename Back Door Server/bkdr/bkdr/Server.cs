using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.ComponentModel;

namespace bkdr
{

    class Server
    {
        string Message = "";
        //SQL sql = new SQL();
        public enum IPtype { Wifi, Ethernet };
        static TcpListener server = null;
        public TcpClient client = default(TcpClient);

        public void GetClientAsync()
        {
            
                client = server.AcceptTcpClient();                       
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


        public void SendData(string msg)
        {
            NetworkStream nStream = client.GetStream();
            byte[] _buffer = ASCIIEncoding.ASCII.GetBytes(msg);
            nStream.Write(_buffer, 0, _buffer.Length);
        }

        public void SetServer()
        {

            IPAddress IPAddr = getAdderss();

            server = new TcpListener(IPAddr, 1111);
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

    }
}
