using System.Threading.Tasks;
using System;

namespace SOAP_Web_Service_Client { 

    internal class Program
    {
        static async Task Main(string[] args)
        {

            var client = new ServiceReference1.CalculatorSoapClient("CalculatorSoap");
            //Console.WriteLine(await client.ChannelFactory.CreateChannel().AddAsync(2, 5));
            Console.WriteLine(await client.AddAsync(1,1));
            }
    }
}
