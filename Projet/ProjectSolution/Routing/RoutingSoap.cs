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
        private HttpClient httpClient = new HttpClient();
        RoutingRest rest = new RoutingRest();
        public async Task<ResultObject> computePath(string start, string end)
        {
            ResultObject result = null;
            result = await rest.ComputePath(start, end);   
            Console.WriteLine(result.ToString());
            return result;
        }
    }
}
