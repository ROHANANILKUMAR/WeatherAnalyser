using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace WeatherAnalyser
{
    class Message
    {
        public TcpClient Client;
        public string Data;
        public IPAddress IP;
        public Message(TcpClient client,string msg)
        {
            this.Data = msg;
            this.Client = client;
            this.IP=IPAddress.Parse(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString());
        }
    }
}
