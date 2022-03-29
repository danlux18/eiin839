using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math_SOAP_Web_Service_Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var client1 = new MathServiceReference.MathsOperationsClient();
            var client2 = new MathServiceReference.MathsOperationsClient();
            var client3 = new MathServiceReference.MathsOperationsClient();
            var client4 = new MathServiceReference.MathsOperationsClient();
            Console.WriteLine(client1.Add(1, 45));
        }
    }
}