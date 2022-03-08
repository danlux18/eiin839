using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using System.Device.Location;
using System.Globalization;
using System.Text.Json.Nodes;

namespace TD3
{
    public class Contract
    {
        public String name { get; set; }
        public String commercial_name { get; set; }
        public String country_code { get; set; }
        public List<String>  cities { get; set; }
        public String toString()
        {
            String res = "Contract : "+ name + ", Commercial_Name : "+ commercial_name + ", Country code :"+ country_code + ", Cities : ";
            if(cities != null)
            {
                foreach (var city in cities)
                {
                    res +=city+" ";
                }
            }
            return res;
        }
      
    }

    public class Station
    {
        public Double number { get; set; }
        public String contract_name { get; set; }
        public String name { get; set; }
        public String adress { get; set; }
        public Double[] position { get; set; }
        public Boolean banking { get; set; }
        public Boolean bonus { get; set; }
        

        public List<String> cities { get; set; }
        public String toString()
        {
            String res = "Station : " + name + ", Commercial_Name : " + commercial_name + ", Country code :" + country_code + ", Cities : ";
            if (cities != null)
            {
                foreach (var city in cities)
                {
                    res += city + " ";
                }
            }
            return res;
        }

    }

    internal class Program
    {
        static readonly HttpClient client = new HttpClient();
        static async Task Main(string[] args)
        {
            string key = "3f2b814603be5297d19d4f009080773cf5d94278";
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                /*Part 1:
                HttpResponseMessage response = await client.GetAsync("https://api.jcdecaux.com/vls/v3/contracts?apiKey="+key);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);
                List<Contract> contracts = JsonSerializer.Deserialize<List<Contract>>(responseBody);
                if(contracts.Count == 0)
                foreach (Contract element in contracts)
                {
                    Console.WriteLine(element.toString());
                }*/
                HttpResponseMessage response = await client.GetAsync("https://api.jcdecaux.com/vls/v3/stations?contract="+args[0]+"&apiKey=" + key);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                JsonNode json = JsonValue.Parse(responseBody);
                JsonArray jsonArray = json.AsArray();
                double minDistance = Double.PositiveInfinity;
                string closerStation = "";
                GeoCoordinate cliGPS = new GeoCoordinate(Double.Parse(args[1]), Double.Parse(args[2]));
                for (int i = 0; i < jsonArray.Count; i++)
                {
                    GeoCoordinate currentGPS = new GeoCoordinate(jsonArray[i]["position"]["latitude"].GetValue<Double>(),
                        jsonArray[i]["position"]["longitude"].GetValue<Double>());
                    double d = currentGPS.GetDistanceTo(cliGPS);
                    if (d < minDistance)
                    {
                        minDistance = d;
                        closerStation = jsonArray[i]["name"].GetValue<string>();
                    }
                }
                Console.WriteLine(closerStation);

            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
        }
    }

    
}
