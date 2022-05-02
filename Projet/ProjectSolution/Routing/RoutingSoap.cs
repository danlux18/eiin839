using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Routing
{
    public class RoutingSoap : IRoutingSoap
    {
        RoutingRest rest = new RoutingRest();
        private string endpointAdress = "http://localhost:8733/Design_Time_Addresses/Routing/ServiceSoap/secondService";
        public async Task<ResultObject> computePath(string start, string end)
        {
            Console.WriteLine("Compute path from Soap Service : start = "+start+" ,end = "+end);
            ResultObject result = null;
            result = await rest.ComputePath(start, end);
            Console.WriteLine("Request from "+start + " to " + end+" has finish !");
            return result;
        }
    }
}
