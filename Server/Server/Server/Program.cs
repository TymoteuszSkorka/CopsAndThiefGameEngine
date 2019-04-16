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

            int counter = 0;
            Program p = new Program();
            while (true)
            {
                counter++;
                ClientSocket = ServerListener.Accept();
                Console.WriteLine(counter + " Clients connected");
                Thread UserThread = new Thread(new ThreadStart(()=>p.User(ClientSocket, plansza)));
                UserThread.Start();
            }
        
        }
        public void User(Socket client, Board plansza)
        {
            while (true)
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
                else
                {
                    client.Send(msg, 0, size, SocketFlags.None);
                }
            }
        }
    }
}
