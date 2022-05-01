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
                var client = new RoutingSoapReference.RoutingSoapClient();
              

                Console.WriteLine("Request from Paris to Marseille :");
                DateTime beginning = DateTime.Now;
                var answer = client.computePathAsync("Paris", "Marseille").Result;
                DateTime end = DateTime.Now;
                Console.WriteLine("Time to compute the request : "+(end - beginning).TotalSeconds);
                Console.WriteLine("Result : " + answer.ToString());
                Console.WriteLine("Worth it : " + answer.worthIt);
                Console.WriteLine("Start Position: " + answer.startPosition);

            }
            catch(ServerTooBusyException ex)
            {
                Console.WriteLine("Exception : " + ex.Message);
            }
        }
    }
}
