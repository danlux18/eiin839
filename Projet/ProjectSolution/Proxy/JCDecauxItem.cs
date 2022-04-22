using System;
using System.Net.Http;
using System.Text.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proxy
{
    public class JCDecauxItem
    {
        private static string key = "3f2b814603be5297d19d4f009080773cf5d94278";
        private static readonly HttpClient client = new HttpClient();
        public List<Station> item;

        public JCDecauxItem(List<Station> stationList)
        {
            item = stationList;
        }

        public static async Task<JCDecauxItem> creationAsync(string itemName)
        {
            string request = "https://api.jcdecaux.com/vls/v3/stations?";
            if(itemName == null)
            {
                throw new ArgumentNullException("Null forbidden in constructor !");
            }
            if(itemName == "AllStations")
            {
                request += "apiKey=" + key;
            }
            else
            {
                request += "contract=" + itemName + "&apiKey=" + key;
            }
            HttpResponseMessage response = await client.GetAsync(request);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            List<Station> stationList = JsonSerializer.Deserialize<List<Station>>(responseBody);
            return new JCDecauxItem(stationList);
            
        }
    }
}
