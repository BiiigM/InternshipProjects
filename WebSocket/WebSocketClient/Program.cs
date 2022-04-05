using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace WebSocketClient
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                //Creating a Client
                TcpClient client = new TcpClient();
                Console.WriteLine("Connecting...");
                
                //Connection the Client to the Server
                client.Connect(IPAddress.Parse("127.0.0.1"), 8080);
                Console.WriteLine("Connected");
                Console.WriteLine("Write something to the server");
                
                //Get the User Input
                string userInput = Console.ReadLine();
                Stream stream = client.GetStream();

                ASCIIEncoding asciiEncoding = new ASCIIEncoding();
                byte[] b = asciiEncoding.GetBytes(userInput);
                Console.WriteLine("Trasmitting...");
                
                //Sending the data to the server
                stream.Write(b,0,b.Length);
                
                //Get the data from the server
                byte[] byteBack = new byte[100];
                int k = stream.Read(byteBack, 0, 100);

                for (int i = 0; i < k; i++)
                {
                    Console.Write(Convert.ToChar(byteBack[i]));
                }
                
                client.Close();
                Console.ReadKey();
            }
            catch (Exception e)
            {
                //Ignore
            }
        }
    }
}