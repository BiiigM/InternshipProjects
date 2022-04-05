using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace WebSocketServer
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                //Creating the TcpListiner and starting it
                TcpListener tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 8080);
                tcpListener.Start();
                Console.WriteLine("Server is running. Port: 8080");
                Console.WriteLine("Waiting for Connection");
                
                //Waiting for Client
                Socket socket = tcpListener.AcceptSocket();
                Console.WriteLine("Connection accepted");
                
                //Receive the Date from the client and show the Data
                byte[] b = new byte[100];
                int k = socket.Receive(b);
                for (int i = 0; i < k; i++)
                {
                    Console.Write(Convert.ToChar(b[i]));
                }
                
                //Sending some data to the Client
                ASCIIEncoding asciiEncoding = new ASCIIEncoding();
                socket.Send(asciiEncoding.GetBytes("Der server hat deine Nachricht bekommen"));
                
                //Colse every thing
                socket.Close();
                tcpListener.Stop();
                Console.ReadKey();
            }
            catch (Exception e)
            {
                //Ignore
            }
        }
    }
}