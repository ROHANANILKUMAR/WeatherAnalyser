using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace bkdr
{
    class Program
    {

        static Client c;
        static void Main(params string[] args)
        {
            Set();
        }

        static void Set()
        {
            int[] addr = new int[4] { 192, 168, 43, 70 };
            byte[] ip = new byte[4];

            string data = File.ReadAllText(@"G:\Science Exhibition\connection.txt");
            string ipaddr = data.Split(';')[1];
            int port = Convert.ToInt16(data.Split(';')[3]);
            string[] ip_data = ipaddr.Split('.');

            for (int i = 0; i < ip_data.Length; i++) addr[i] = Convert.ToInt16(ip_data[i]);

            foreach (int i in addr)
            {
                ip[Array.IndexOf(addr, i)] = Convert.ToByte(i);
            }

            c = new Client();
            c.Initialize(addr,port);
            Exec();
        }

        static void DoWork(string Data,string Print)
        {
            c.SendData(Data);
            Console.WriteLine(Print);
        }

        static bool dump = true;

        static void dumpinfo()
        {
            while (dump)
            {
                c.SendData("dumpinfo");
                
                Console.WriteLine("Dump info:"+ c.GetResponse());
                if (Console.KeyAvailable)
                {
                    if (Console.ReadKey().Key == ConsoleKey.C)
                    {
                        dump= false;
                    }
                }
                
            }
        }

        static int Exec()
        {
            while (true)
            {
                Console.Write(">>>");
                string usr = Console.ReadLine();
                if (usr.StartsWith("send"))
                {
                    DoWork(usr.Split(' ')[1],"Command "+ usr.Split(' ')[1]);
                }
                switch (usr)
                {
                    case ("gon"):
                        DoWork("greenon","Green On command");
                        break;
                    case ("gof"):
                        DoWork("greenoff","Green Off command");
                        break;
                    case ("rec"):
                        c.SendData("rec");
                        c.Close();
                        Main();
                        break;
                    case ("dumpinfo"):
                        dump = true;
                        dumpinfo();
                        break;
                    case ("newser"):
                        Console.Write("New Serial Number>>>");
                        string ser = Console.ReadLine();
                        if (ser.Length == 10)
                        {
                            DoWork("ser:"+ser,"Serial number changed");
                        }
                        else
                        {
                            Console.WriteLine("Error: Serial number must be of at least 10 digits");
                        }
                        break;
                    case ("run"):
                        DoWork("start", "Analyser.py started");
                        break;
                    case ("quit"):
                        return 0;
                    default:
                        Console.WriteLine("Unidentified command");
                        break;
                }
            }
        }
    }
}
