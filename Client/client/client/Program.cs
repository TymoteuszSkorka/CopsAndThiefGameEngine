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
using System.Diagnostics;
using main;

namespace client
{
    class Program
    {
        static int time = 0;
        static void Main(string[] args)
        {
            try
            {
                int port = 13000;
                string IpAddress = "127.0.0.1";
                Socket ClientSocket = new Socket(AddressFamily
                    .InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse(IpAddress), port);
                ClientSocket.Connect(ep);
                Console.WriteLine("Client is connected!");
//handshake ---------------------------------------------------------------------------------------------------------------------

                byte[] MsgFromServer_hello = new byte[1024];
                int size_h = ClientSocket.Receive(MsgFromServer_hello);
                Console.WriteLine("Server: " + System.Text.Encoding.ASCII.GetString(MsgFromServer_hello, 0, size_h));
                string asciiString_h = Encoding.ASCII.GetString(MsgFromServer_hello, 0, MsgFromServer_hello.Length);


                string messageFromClient_HS = null;
                Console.Write(">");
                messageFromClient_HS = Console.ReadLine();
                ClientSocket.Send(System.Text.Encoding.ASCII.GetBytes(messageFromClient_HS),
                    0, messageFromClient_HS.Length, SocketFlags.None);


                byte[] MsgFromServer_state = new byte[1024];
                int size_state = ClientSocket.Receive(MsgFromServer_state);
                string asciiString_state = Encoding.ASCII.GetString(MsgFromServer_state, 0, MsgFromServer_state.Length);

                while (string.Compare(asciiString_state, "already taken or input error, pick again:") == 0)
                { 
                    Console.WriteLine(asciiString_state);
                    Console.Write(">");
                    messageFromClient_HS = Console.ReadLine();
                    ClientSocket.Send(System.Text.Encoding.ASCII.GetBytes(messageFromClient_HS),
                        0, messageFromClient_HS.Length, SocketFlags.None);
                    size_state = ClientSocket.Receive(MsgFromServer_state);
                    asciiString_state = Encoding.ASCII.GetString(MsgFromServer_state, 0, size_state);
                }

                Console.WriteLine(asciiString_state);

                byte[] board_state = new byte[1024];
                int board_state_Size = ClientSocket.Receive(board_state);

                Console.WriteLine(System.Text.Encoding.ASCII.GetString(board_state, 0, board_state_Size));


                string GO_messageFromClient_HS = Console.ReadLine();
                ClientSocket.Send(System.Text.Encoding.ASCII.GetBytes(GO_messageFromClient_HS),
                       0, GO_messageFromClient_HS.Length, SocketFlags.None);

                //end of handshake ---------------------------------------------------------------------------------------------------------------------
                while (true)
                {
                    try
                    {   
                        //w tym miejsu dodać wysyłanie JSONEM do serwera obliczonych zmian na planszy
                        string messageFromClient = null;
                        Console.Write(">");
                        messageFromClient = Console.ReadLine();
                        ClientSocket.Send(System.Text.Encoding.ASCII.GetBytes(messageFromClient),
                            0, messageFromClient.Length, SocketFlags.None);

                        //otrzymujemy od serwera zmianioną planszę następnie mamy 500 ms na ponowne obliczenie i wysłanie odpowiedzi
                        byte[] MsgFromServer = new byte[1024];
                        int size = ClientSocket.Receive(MsgFromServer);
                        Console.WriteLine("Server answer: " + System.Text.Encoding.ASCII.GetString(MsgFromServer, 0, size));
                        string asciiString = Encoding.ASCII.GetString(MsgFromServer, 0, MsgFromServer.Length);
                        Board plansza = JsonConvert.DeserializeObject<Board>(asciiString);
                        //Console.WriteLine(plansza.m_16NumOfRows);
                        //Console.WriteLine(plansza.m_16NumOfColumns);
                    }
                    catch
                    {
                        Console.WriteLine("connection failure");
                        ClientSocket.Close();
                    }

                }
            }
            catch
            {
                Console.WriteLine("no server found");
                Console.ReadLine();
            }
        
        }
    }
}