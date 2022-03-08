using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.IO;

namespace Echo
{
    class EchoServer
    {
        [Obsolete]
        static void Main(string[] args)
        {

            Console.CancelKeyPress += delegate
            {
                System.Environment.Exit(0);
            };

            TcpListener ServerSocket = new TcpListener(5000);
            ServerSocket.Start();

            Console.WriteLine("Server started.");
            while (true)
            {
                TcpClient clientSocket = ServerSocket.AcceptTcpClient();
                handleClient client = new handleClient();
                client.startClient(clientSocket);
            }


        }
    }

    public class handleClient
    {
        static Encoding enc = Encoding.UTF8;
        TcpClient clientSocket;
        public void startClient(TcpClient inClientSocket)
        {
            this.clientSocket = inClientSocket;
            Thread ctThread = new Thread(Echo);
            ctThread.Start();
        }


        /**/
        private void Echo()
        {
            NetworkStream stream = clientSocket.GetStream();
            MemoryStream memoryStream = new MemoryStream();
            BinaryReader reader = new BinaryReader(stream);
            BinaryWriter writer = new BinaryWriter(stream);

            byte[] data = new byte[256];
            int nb_char;
            do
            {
                nb_char = stream.Read(data, 0, data.Length);
                if (nb_char == 0)
                {
                    Console.WriteLine("client disconnected...");
                    return;
                }
                memoryStream.Write(data, 0, nb_char);
            } while (stream.DataAvailable);
            string request = enc.GetString(memoryStream.ToArray());
            string filecontent = "<p>HELLO MY DEAR</p>";
            string first;
            using (var read = new StringReader(request))
            {
                first = read.ReadLine();
            }
            Console.WriteLine(first+"\n");
            if (first.StartsWith("GET /")){
                string filename = first.Substring(4, first.Length - 13);
                Console.WriteLine(filename + "\n");
                if (filename.Equals("/")){
                    filecontent = "<title>L'exemple HTML le plus simple</title> <h1> Ceci est un sous - titre de niveau 1 </h1>Bienvenue dans le monde HTML.Ceci est un paragraphe. <p> Et ceci en est un second.</p> <a href = 'index.html'> cliquez ici </a> pour réafficher.";
                }
                else if (filename.Equals("/index.html"))
                {
                    string env = Environment.GetEnvironmentVariable("HTTP_ROOT");
                    filecontent = File.ReadAllText(env+"\\index.html");
                }
            
            }
            Console.WriteLine(filecontent + "\n");
            stream.Write(enc.GetBytes("HTTP / 1.1 200 OK\nContent-Length: "+filecontent.Length+"\nContent-Type: text/html\nConnection: Close\n\n"+filecontent));
            stream.Close();
        }



    }

}