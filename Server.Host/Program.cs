using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Server.Services;

namespace Server.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost host = new ServiceHost(typeof(EncodeService));
            host.Open();
            Console.WriteLine("Encode Service running...");
            Console.ReadLine();
            host.Close();
        }
    }
}
