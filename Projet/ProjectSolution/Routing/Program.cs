using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Routing
{
    internal class Program
    {

        public static void Main()
        {
            Console.WriteLine("Launching Routing Rest...");
            ServiceHost restService = new ServiceHost(typeof(RoutingRest));
            restService.Open();
            Console.WriteLine("Routing Rest ready !");
            Console.WriteLine("Launching Routing Soap...");
            Console.WriteLine("ROUTING SOAP CONNECTING TO ROUTING REST : ");
            ServiceHost soapService = new ServiceHost(typeof(RoutingSoap));
            soapService.Open();
            Console.WriteLine("Routing Soap ready !");  
            Console.ReadLine();
        }
    }
}
