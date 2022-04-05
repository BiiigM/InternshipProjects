using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SocketClient
{
    internal class Program
    {
        private static readonly IPAddress Ip = IPAddress.Parse("127.0.0.1");
        private static readonly int Port = 8080;
        private static readonly IPEndPoint localEndPoint = new IPEndPoint(Ip, Port);

        private static Socket ClientSocket;

        private static string Name;
        
        public static void Main(string[] args)
        {
            Console.Title = "Chat v1.0";
            Console.WriteLine("Wie willst du heißen?");
            Name = Console.ReadLine();
            Console.Title = Name;
            ConnectToServer();
            LoopText();
        }

        private static void ConnectToServer()
        {
            ClientSocket = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            int attempts = 0;

            while (!ClientSocket.Connected)
            {
                try
                {
                    attempts++;
                    Console.WriteLine("Connection attempts: " + attempts);
                    ClientSocket.Connect(Ip, Port);
                }
                catch (SocketException)
                {
                    Console.Clear();
                }
            }
            
            Console.Clear();
            Console.WriteLine("Connected");

            Thread refresher = new Thread(ReceiveResponse) {IsBackground = true};
            refresher.Start();
        }

        private static void LoopText()
        {
            Console.WriteLine("write exit to close the client");
            while (true)
            {
                string data = Console.ReadLine();
                string NameIdx = Name + "_";
                
                SendData(NameIdx + data);
            }
        }

        private static void SendData(string data)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(data);
            ClientSocket.Send(buffer, 0, buffer.Length, 0);

            if (data.ToLower() == "exit")
            {
                ClientSocket.Shutdown(SocketShutdown.Both);
                ClientSocket.Close();
                Environment.Exit(0);
            }
        }

        private static void ReceiveResponse()
        {
            while (true)
            {
                byte[] buffer = new byte[1024];
                int received = ClientSocket.Receive(buffer, 0);
                if(received == 0) return;
                byte[] tmpBuffer = new byte[received];
            
                Array.Copy(buffer, tmpBuffer, received);
                string text = Encoding.UTF8.GetString(tmpBuffer);
                Console.WriteLine(text);
                Console.Beep();
            }
        }
    }
}