using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
namespace bkdr
{
    class Message
    {
        public string Client_Message;

        public Message(string message)
        {
            Client_Message = message;
        }
    }
    class Client
    {
        static TcpClient client = new TcpClient();
        public enum IPtype { Wifi, Ethernet };
        public string Message = "";
        public event EventHandler<Message> OnMessageRecieved;

        private IPAddress GetIP(int[] addr)
        {
            byte[] ip = new byte[4];
            foreach (int i in addr)
            {
                ip[Array.IndexOf(addr, i)] = Convert.ToByte(i);
            }
            IPAddress LocalAddress = new IPAddress(ip);

            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

            return LocalAddress;
        }

        public void Initialize(int[] addr,int port)
        {
            IPAddress LocalAddress = GetIP(addr);

            Console.WriteLine("Trying to connect...");
            while (true)
            {
                try
                {
                    client.Connect(LocalAddress, port);
                    System.Threading.Thread.Sleep(1000);
                    break;
                }
                catch { }
            }

            Console.WriteLine("Connected to " + LocalAddress.ToString());
        }

        public void Close()
        {
            client.Close();
            client.Client.Close();
            client.Client.Dispose();
            client.Dispose();
            
        }

        public string GetResponse()
        {

            NetworkStream nStream = client.GetStream();
            byte[] _buffer = new byte[1024];
            nStream.Read(_buffer, 0, _buffer.Length);
            string Message = ASCIIEncoding.ASCII.GetString(_buffer);
            return Message.Replace("\n","").Replace("\r","").Replace("\0",""); 
            
        }

        public void SendData(string message)
        {
            NetworkStream Nstream = client.GetStream();
            byte[] send = ASCIIEncoding.ASCII.GetBytes(message);
            Nstream.Write(send, 0, send.Length);
        }
    }
}
