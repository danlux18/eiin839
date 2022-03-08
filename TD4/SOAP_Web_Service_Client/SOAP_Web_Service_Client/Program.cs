using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using System.Device.Location;
using System.Globalization;
using System.Text.Json.Nodes;
using System;
using SOAP_Web_Service_Client.ServiceReference1;

namespace SOAP_Web_Service_Client { 

    internal class Program
    {
        static readonly HttpClient client = new HttpClient();
        static async Task Main(string[] args)
        {
            var client = new ServiceReference1.CalculatorSoapClient("CalculatorSoap");
            Console.WriteLine(await client.ChannelFactory.CreateChannel().AddAsync(2, 5));
            }
    }
}
