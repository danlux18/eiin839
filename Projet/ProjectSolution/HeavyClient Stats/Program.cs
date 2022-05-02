using System;
using System.ServiceModel;
using ClassLibrary;
namespace HeavyClient_Stats
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Launching HeavyClient !");
                Console.WriteLine("Write the start point :");
                string startPoint = Console.ReadLine();

                Console.WriteLine("Write the end point :");
                string endPoint = Console.ReadLine();
               
                Console.WriteLine("HEAVY CLIENT CONNECTING TO ROUTING SOAP : ");
                var client = new RoutingSoapReference.RoutingSoapClient();
                
                Console.WriteLine("Request from "+startPoint+" to "+endPoint+" :");
                DateTime beginning = DateTime.Now;
                var answer = client.computePathAsync(startPoint, endPoint).Result;
                DateTime end = DateTime.Now;
                Console.WriteLine("Time to compute the request : "+(end - beginning).TotalSeconds);
                Console.WriteLine("Worth to take cycle : "+answer.worthIt);
                Console.WriteLine("Same request 5 more times (average of time) but it has to be faster because it is already in the proxy :");
                double average_time = 0;
                for(int i = 0; i < 5; i++)
                {
                    beginning = DateTime.Now;
                    answer = client.computePathAsync(startPoint, endPoint).Result;
                    end = DateTime.Now;
                    average_time += (end - beginning).TotalSeconds;
                }
                average_time /= 5;

                Console.WriteLine("Average time : "+average_time);

            }
            catch(ServerTooBusyException ex)
            {
                Console.WriteLine("Exception : " + ex.Message);
            }
        }
    }
}
