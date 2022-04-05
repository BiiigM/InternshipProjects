using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace HighPerformanceTCPClientServer
{
    public struct SocketNames
    {
        public Socket client;
        public string name;
    }
    
    public static class Program
    {
        #region Vars
        static IPAddress ip = IPAddress.Parse("127.0.0.1");
        static IPEndPoint localEndPoint = new IPEndPoint(ip, 8080);
        static Socket ServerSocket = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        private static int BufferSize = 1038576;
        private static byte[] buffer = new byte[BufferSize];
        private static Dictionary<int, SocketNames> clientSockets = new Dictionary<int, SocketNames>();
        private static bool _serverClosing = false;
        #endregion
        
        public static void Main(string[] args)
        {
            Console.Title = "Server";
            StartUp();
            while (!_serverClosing)
            {
                Console.Clear();
                Console.WriteLine("Press 'enter' to exit");
                Console.WriteLine("");
                Console.WriteLine("Commands:");
                Console.WriteLine("clients - to see all clients");
                Console.WriteLine("send - to send a message to all clients");
                switch (Console.ReadLine().ToLower())
                {
                    case "clients":
                        ShowClients();
                        break;
                    case "send":
                        SendToAllCommand();
                        break;
                    case "":
                        _serverClosing = true;
                        break;
                    default:
                        Console.WriteLine("Not a Command!! Press enter to cont...");
                        Console.ReadLine();
                        break;
                }
            }
            CloseAll();
        }

        private static void StartUp()
        {
            Console.WriteLine("Server starting up...");
            ServerSocket.Bind(localEndPoint);
            ServerSocket.Listen(100);
            ServerSocket.BeginAccept(AcceptCallBack, null);
            Console.Clear();
            Console.WriteLine("Server gestarted :)");
        }

        private static void CloseAll()
        {
            Console.WriteLine("Closing Server...");
            byte[] buffer = Encoding.UTF8.GetBytes("closing");
            foreach (KeyValuePair<int, SocketNames> client in clientSockets)
            {
                client.Value.client.Send(buffer);
                client.Value.client.Shutdown(SocketShutdown.Both);
                client.Value.client.Close();
            }
            ServerSocket.Close();
            Console.WriteLine("Server closed :(");
        }

        private static void AcceptCallBack(IAsyncResult ar)
        {
            Socket socket;
            try
            {
                socket = ServerSocket.EndAccept(ar);
            }
            catch (ObjectDisposedException)
            {
                return;
            }
            socket.BeginReceive(buffer, 0, BufferSize, 0, ReceiveCallBack, socket);
            ServerSocket.BeginAccept(AcceptCallBack, null);
        }

        private static void ReceiveCallBack(IAsyncResult ar)
        {
            Socket current = (Socket) ar.AsyncState;
            
            try
            {
                //Getting the Data, which the Program need
                int recieved = current.EndReceive(ar);
            
                byte[] tmpBuffer = new byte[recieved];
                Array.Copy(buffer, tmpBuffer, recieved);
                string receivedText = Encoding.UTF8.GetString(tmpBuffer);
                Console.WriteLine("Received Text: " + receivedText);
                
                //Looking if the Client want to disconnect and then disconnect him properly 
                if (receivedText.Split('_')[1].Contains("exit"))
                {
                    for (int i = 0; i < clientSockets.Count; i++)
                    {
                        if(clientSockets[i].client.Equals(current))
                            clientSockets.Remove(i);
                        ResetsKeys();
                    }
                    current.Shutdown(SocketShutdown.Both);
                    current.Close();
                    InformAboutNewClient();
                    Console.WriteLine("Ein client ist disconnected");
                    return;
                }
                
                //Doing the Handshack
                if (receivedText.Split('_')[0].Contains("<NAME>"))
                {
                    DoHandShack(current ,receivedText.Split('_')[1]);
                    return;
                }
                
                //Send it to the right Client and waiting for more data
                SendToPerson(tmpBuffer, receivedText);
                current.BeginReceive(buffer, 0, BufferSize, 0, ReceiveCallBack, current);
            }
            catch (SocketException e)
            {
                Console.WriteLine("Ein client ist disconnected");
                for (int i = 0; i < clientSockets.Count; i++)
                {
                    if(clientSockets[i].client.Equals(current))
                        clientSockets.Remove(i);
                    ResetsKeys();
                }
                InformAboutNewClient();
                current.Close();
            }
        }

        private static void SendToPerson(byte[] sendBuffer, string receivedText)
        {
            string[] messageData = receivedText.Split('_');
            
            foreach (KeyValuePair<int, SocketNames> client in clientSockets)
            {
                switch (messageData[0])
                {
                    case "<TEXT>":
                        if (client.Value.name.Equals(messageData[2]))
                            client.Value.client.Send(sendBuffer);
                        break;
                    case "<FILE>":
                        if (client.Value.name.Equals(messageData[2]))
                            client.Value.client.Send(sendBuffer);
                        break;
                }
            }
        }

        #region Handshack
        private static void DoHandShack(Socket current, string receivedText)
        {
            AddClientToList(current, receivedText);
            
            //Waiting for Client to finish
            current.BeginReceive(buffer, 0, BufferSize, 0, WaitForCallBack, current);
            
            InformAboutNewClient();
            
            //Waiting for Input from client
            current.BeginReceive(buffer, 0, BufferSize, 0, ReceiveCallBack, current);
        }

        private static void WaitForCallBack(IAsyncResult ar)
        {
            Socket current = (Socket) ar.AsyncState;
            current.EndReceive(ar);
        }

        

        private static void AddClientToList(Socket current, string name)
        {
            int attempts = 0;
            
            for (int i = 0; i < clientSockets.Count; i++)
            {
                if (clientSockets[i].name == name)
                {
                    attempts++;
                    name = Regex.Replace(name, @"[\d]", string.Empty);
                    name += attempts;
                    i = 0;
                }
            }

            if (attempts != 0)
            {
                SocketNames sn;
                sn.client = current;
                sn.name = name;
                    
                clientSockets.Add(clientSockets.Count, sn);
                Console.WriteLine("Name Changed to:" + clientSockets[clientSockets.Count - 1].name);
                
                byte[] buffer = Encoding.UTF8.GetBytes("<CHANGE>_" + name);
                current.Send(buffer);
            }
            
            if(attempts == 0)
            {
                SocketNames sn;
                sn.client = current;
                sn.name = name;
                    
                clientSockets.Add(clientSockets.Count, sn);
                
                byte[] buffer = Encoding.UTF8.GetBytes("FINISHED");
                current.Send(buffer);
            }
        }
        #endregion
        
        private static void InformAboutNewClient()
        {
            List<string> tmpNames = new List<string>();
            tmpNames.Add("<NAME>");
            foreach (KeyValuePair<int, SocketNames> clientSocket in clientSockets)
            {
                tmpNames.Add(clientSocket.Value.name);
            }

            string names = String.Join("_", tmpNames);
            byte[] buffer = Encoding.UTF8.GetBytes(names);
            
            foreach (KeyValuePair<int, SocketNames> socket in clientSockets)
            {
                socket.Value.client.Send(buffer);
            }
        }

        private static void ResetsKeys()
        {
            List<SocketNames> sn = new List<SocketNames>();
            foreach (KeyValuePair<int,SocketNames> clientSocket in clientSockets)
            {
                sn.Add(clientSocket.Value);
            }
            
            clientSockets.Clear();
            
            for (var i = 0; i < sn.Count; i++)
            {
                clientSockets.Add(i, sn[i]);
            }
        }

        #region Commands

        private static void ShowClients()
        {
            Console.Clear();
            foreach (KeyValuePair<int,SocketNames> socket in clientSockets)
            {
                Console.WriteLine(socket.Value.name);
            }
            
            Console.WriteLine("Press enter to cont...");
            Console.ReadLine();
        }

        private static void SendToAllCommand()
        {
            Console.Clear();
            Console.WriteLine("your message:");
            string message = Console.ReadLine();
            byte[] buffer = Encoding.UTF8.GetBytes("<TEXT>_SERVER__" + message);
            foreach (KeyValuePair<int, SocketNames> clientSocket in clientSockets)
            {
                clientSocket.Value.client.Send(buffer);
                Console.WriteLine("Message sendet to:" + clientSocket.Value.name);
            }
            
            Console.WriteLine("Press enter to cont...");
            Console.ReadLine();
        }

        #endregion
    }
}