using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace WeatherAnalyser
{
    class Log
    {
        public string ErrMsg;
        public IPAddress IP;
        public int Per;
        public Dictionary<string, string> Details = new Dictionary<string, string>();
        public DateTime Date;
        public bool HumanPresent;
        public Log(IPAddress ip, string msg, int per,Dictionary<string,string> details,bool human)
        {
            this.ErrMsg = msg;
            this.IP = ip;
            this.Per = per;
            this.Details = details;
            this.Date = DateTime.Now;
            this.HumanPresent = human;
        }

        public override string ToString()
        {
            string res="";

            res += IP.ToString();
            res += ":";
            res += Details["Location"];
            res += ":";
            res += ErrMsg;

            return res;
        }


    }
}
