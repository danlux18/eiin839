using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soap_Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var client = new MathServiceReference.MathsOperationsClient();
            Console.WriteLine(client.Add(1, 45));
            Console.ReadLine(); 
        }
    }
}
