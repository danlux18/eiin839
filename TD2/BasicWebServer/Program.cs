using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;

namespace BasicServerHTTPlistener
{
    public class Mymethods
    {
        public string MyMethod(string param1, string param2)
        {
            return "<html><body> Hello " + param1 + " et " + param2 + "</body></html>";
        }

        public string DoSomething(string param1, string param2)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = @"C:\windows\system32\ping.exe"; ; // Specify exe name. the exec to ping an adress
            start.Arguments = "-n 1 " + param1; // Specify arguments. with param1 an adress to ping
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            string result;
            //
            using (Process process = Process.Start(start))
            {
                //
                // Read in all the text from the process with the StreamReader.
                //
                using (StreamReader reader = process.StandardOutput)
                {
                    result = reader.ReadToEnd();
                    //Console.WriteLine(result);
                    //Console.ReadLine();
                }
            }
            if (param2 == "easteregg")
            {
                return "<html><head><meta charset='utf-8'></head><body> HAHA  \n " + result+"</body></html>"; 
            }
            else
            {
                return "<html><head><meta charset='utf-8'></head><body> Hello  \n " + result + "</body></html>";
            }
        }

        public string Incr(string param, string pramamezmemfvgb)
        {
            return (int.Parse(param)+1).ToString();
        }
    }
    internal class Program
    {
        private static void Main(string[] args)
        {

            //if HttpListener is not supported by the Framework
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("A more recent Windows version is required to use the HttpListener class.");
                return;
            }
 
 
            // Create a listener.
            HttpListener listener = new HttpListener();

            // Add the prefixes.
            if (args.Length != 0)
            {
                foreach (string s in args)
                {
                    listener.Prefixes.Add(s);
                    // don't forget to authorize access to the TCP/IP addresses localhost:xxxx and localhost:yyyy 
                    // with netsh http add urlacl url=http://localhost:xxxx/ user="Tout le monde"
                    // and netsh http add urlacl url=http://localhost:yyyy/ user="Tout le monde"
                    // user="Tout le monde" is language dependent, use user=Everyone in english 

                }
            }
            else
            {
                Console.WriteLine("Syntax error: the call must contain at least one web server url as argument");
            }
            listener.Start();

            // get args 
            foreach (string s in args)
            {
                Console.WriteLine("Listening for connections on " + s);
            }

            // Trap Ctrl-C on console to exit 
            Console.CancelKeyPress += delegate {
                // call methods to close socket and exit
                listener.Stop();
                listener.Close();
                Environment.Exit(0);
            };


            while (true)
            {
                // Note: The GetContext method blocks while waiting for a request.
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;

                string documentContents;
                using (Stream receiveStream = request.InputStream)
                {
                    using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
                    {
                        documentContents = readStream.ReadToEnd();
                    }
                }
                
                // get url 
                Console.WriteLine($"Received request for {request.Url}");

                //get url protocol
                Console.WriteLine(request.Url.Scheme);
                //get user in url
                Console.WriteLine(request.Url.UserInfo);
                //get host in url
                Console.WriteLine(request.Url.Host);
                //get port in url
                Console.WriteLine(request.Url.Port);
                //get path in url 
                Console.WriteLine(request.Url.LocalPath);

                // parse path in url 
                foreach (string str in request.Url.Segments)
                {
                    Console.WriteLine(str);

                }

                //get params un url. After ? and between &

                //Console.WriteLine(request.Url.Query);

                //parse params in url
                /*Console.WriteLine("param1 = " + HttpUtility.ParseQueryString(request.Url.Query).Get("param1"));
                Console.WriteLine("param2 = " + HttpUtility.ParseQueryString(request.Url.Query).Get("param2"));
                Console.WriteLine("param3 = " + HttpUtility.ParseQueryString(request.Url.Query).Get("param3"));
                Console.WriteLine("param4 = " + HttpUtility.ParseQueryString(request.Url.Query).Get("param4"));*/
                
                string methodToUse = "MyMethod";
                Type type = typeof(Mymethods);
                if (request.Url.Segments.Length > 2)
                {
                    methodToUse = request.Url.Segments[2];
                }
                MethodInfo method = type.GetMethod(methodToUse);
                Console.WriteLine("La méthode : " + methodToUse + "\n");
                Mymethods mymethod = new Mymethods();
                //string result = (string)method.Invoke(c, null);
                //Console.WriteLine(result);
                //Console.ReadLine();
                int i = 0;
                foreach(string seg in request.Url.Segments)
                {
                    Console.WriteLine("Segment n°"+i+": "+seg+"\n");
                    i++;
                }
                
                string param1 = HttpUtility.ParseQueryString(request.Url.Query).Get("param1");
                string param2 = HttpUtility.ParseQueryString(request.Url.Query).Get("param2");
    
                //
                Console.WriteLine(documentContents);

                // Obtain a response object.
                HttpListenerResponse response = context.Response;

                // Construct a response.
                string responseString = "Empty answer";
                if (methodToUse == "DoSomething" || methodToUse == "MyMethod" || methodToUse == "Incr")
                {
                    responseString = (string)method.Invoke(mymethod, new object[] { param1, param2 });
                }
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                // Get a response stream and write the response to it.
                response.ContentLength64 = buffer.Length;
                System.IO.Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                // You must close the output stream.
                output.Close();
            }
            // Httplistener neither stop ... But Ctrl-C do that ...
            // listener.Stop();
        }
    }
}