using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using main;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Server
{
    
    class Program
    {
        static int counter = 0;
        static void Main(string[] args)
        {
            Board plansza = new Board(20, 20);
            int port = 13000;
            string IpAddress = "127.0.0.1";
            Socket ServerListener = new Socket(AddressFamily
                .InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(IpAddress), port);
            ServerListener.Bind(ep);
            ServerListener.Listen(100);
            Console.WriteLine("Server is Listening ...");
            Socket ClientSocket = default(Socket);

            Program p = new Program();
            while (true)
            {
                ClientSocket = ServerListener.Accept();
                Program.counter++;
                Console.WriteLine(counter + " Clients connected");
                Thread UserThread = new Thread(new ThreadStart(() => p.User(ClientSocket, plansza)));

                string handShakeMsg = "Welcome to Cops&Thiefs game. Type 'start' to begin: ";
                ClientSocket.Send(System.Text.Encoding.ASCII.GetBytes(handShakeMsg),
                    0, handShakeMsg.Length, SocketFlags.None);

                
                try
                {
                    byte[] msg = new byte[1024];
                    int size = ClientSocket.Receive(msg);
                    string asciiString = Encoding.ASCII.GetString(msg, 0, msg.Length);

                    while (string.Compare(asciiString, "start")!=0)
                    {
                        string again = "failure, again:";
                        ClientSocket.Send(System.Text.Encoding.ASCII.GetBytes(again),
                        0, again.Length, SocketFlags.None);
                        ClientSocket.Receive(msg);
                        asciiString = Encoding.ASCII.GetString(msg, 0, msg.Length);
                    }
                    string welcome = "5000";
                    ClientSocket.Send(System.Text.Encoding.ASCII.GetBytes(welcome),
                        0, welcome.Length, SocketFlags.None);
                    ClientSocket.ReceiveTimeout = 5000;
                    UserThread.Start();
                }
                catch(System.Net.Sockets.SocketException sockEx)
                {
                    Console.WriteLine(sockEx.ErrorCode);
                    Program.counter--;
                    ClientSocket.Close();
                    Console.WriteLine("Client disconnected");
                    Console.WriteLine("Clients connected:" + counter);
                }
                
            }

        }
        public void User(Socket client, Board plansza)
        {
            while (client.Connected)
            {
                
                try
                {
                    byte[] msg = new byte[1024];
                    int size = client.Receive(msg);
            
                    string asciiString = Encoding.ASCII.GetString(msg, 0, msg.Length);
                    Console.WriteLine(asciiString);

                    if (msg.Length > 3)
                    {
                        string json = JsonConvert.SerializeObject(plansza);

                        byte[] msg1 = Encoding.ASCII.GetBytes(json);
                        int size2 = msg1.Length;

                        client.Send(msg1, 0, size2, SocketFlags.None);
                    }
                }
                catch(System.Net.Sockets.SocketException sockEx)
                {
                    
                    Console.WriteLine(sockEx.ErrorCode);
                    if (sockEx.ErrorCode != 10060)
                    {
                        Program.counter--;
                        client.Close();
                        Console.WriteLine("Client disconnected");
                        Console.WriteLine("Clients connected:" + counter);
                    }
                    else if(sockEx.ErrorCode == 10060)
                    {
                        Console.WriteLine("default settings for board");
                    }
                }

            }
        }
    }
}