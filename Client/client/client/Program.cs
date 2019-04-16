using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using main;

namespace client
{
    class Program
    {
        static void Main(string[] args)
        {
            int port = 13000;
            string IpAddress = "127.0.0.1";
            Socket ClientSocket = new Socket(AddressFamily
                .InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(IpAddress), port);
            ClientSocket.Connect(ep);
            Console.WriteLine("Client is connected!");

            while (true)
            {
                string messageFromClient = null;
                Console.WriteLine("Ented the message");
                messageFromClient = Console.ReadLine();
                ClientSocket.Send(System.Text.Encoding.ASCII.GetBytes(messageFromClient),
                    0, messageFromClient.Length, SocketFlags.None);

                byte[] MsgFromServer = new byte[1024];
                int size = ClientSocket.Receive(MsgFromServer);
                Console.WriteLine("Server answer: " + System.Text.Encoding.ASCII.GetString(MsgFromServer, 0, size));
                string asciiString = Encoding.ASCII.GetString(MsgFromServer, 0, MsgFromServer.Length);
                Board plansza = JsonConvert.DeserializeObject<Board>(asciiString);
                Console.WriteLine(plansza.m_16NumOfRows);
                Console.WriteLine(plansza.m_16NumOfColumns);


            }
        }
    }
}