﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Proxy
{
    internal class Program
    {

        public static void Main()
        {
            Console.WriteLine("Launching proxy");
            ServiceHost proxyService = new ServiceHost(typeof(JCDecauxData));
            proxyService.Open();
            Console.WriteLine("Proxy ready");
            Console.ReadLine();
        }
    }
}
