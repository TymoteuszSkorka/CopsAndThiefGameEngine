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
using System.Timers;
namespace client
{
    class Program
    {
        static int time = 0;
        static bool error_flag = false;
        static string name = null;       
        static void Main(string[] args)
        {
            Settings boardSettings;
            Moves myMoves;
            InitialMap initBoard;
            Positions lastKMoves;
            Random generator = new Random();
            try
            {
                int port = 13000;
                string IpAddress = "127.0.0.1";
                //string IpAddress = "192.168.0.234";
                Socket ClientSocket = new Socket(AddressFamily
                    .InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse(IpAddress), port);
                ClientSocket.Connect(ep);
                Console.WriteLine("Client is connected!");
                //handshake ---------------------------------------------------------------------------------------------------------------------
                {
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
                    name = messageFromClient_HS;
                    Console.WriteLine(asciiString_state);

                    //dostajemy od serwera ustawienia ogólne rozgrywki
                    byte[] board_state = new byte[1024];
                    int board_state_Size = ClientSocket.Receive(board_state);
                    string json = System.Text.Encoding.ASCII.GetString(board_state, 0, board_state_Size);
                    boardSettings = JsonConvert.DeserializeObject<Settings>(json);

                    /*
                     * TUTAJ MACIE CZAS NA INICJALIZACJE POMOCNICZYCH STRUKTUR NA PODSTAWIE USTAWIEN PLANSZY
                     * Moves to klasa, do ktorej bedziecie wrzucać ruchy
                     */
                    myMoves = new Moves();
                    if (name == "T")
                    {
                        myMoves.init(boardSettings.kClock, 1, name);
                    }
                    else if (name == "P")
                    {
                        myMoves.init(boardSettings.kClock, boardSettings.numOfCops, name);
                    }

                    //wysyłamy wiadomosc ze chcemy zaczac
                    Console.Write("Type whateve if u'r ready");
                    string GO_messageFromClient_HS = Console.ReadLine();
                    ClientSocket.Send(System.Text.Encoding.ASCII.GetBytes(GO_messageFromClient_HS),
                           0, GO_messageFromClient_HS.Length, SocketFlags.None);

                    //oboje czekają na potwierdzenie od servera czy rozgrywka jest zaczęta, od servera dostajemy wylosowany stan planszy i ejst ustawiany w tym miejsu timer
                    byte[] rolled_board = new byte[1024];
                    int start_flag_size = ClientSocket.Receive(rolled_board);
                    json = System.Text.Encoding.ASCII.GetString(rolled_board, 0, start_flag_size);
                    initBoard = JsonConvert.DeserializeObject<InitialMap>(json);

                }
//end of handshake --------------------------------------------------------------------------------------------------------------------------

                //main loop
                while (true)
                {
                    try
                    {
                        //w tym miejsu dodać wysyłanie JSONEM do serwera obliczonych zmian na planszy
                        //
                        //
                        //
                        //
                        //
                        //
                        for (int a = 0; a < boardSettings.kClock; ++a)
                        {
                            if (name == "T")
                            {
                                myMoves.m_16Moves[a, 0] = Convert.ToInt16(generator.Next(0, 5));
                            }
                            else if (name == "P")
                            {
                                for (int b = 0; b < boardSettings.numOfCops; ++b)
                                {
                                    myMoves.m_16Moves[a, b] = Convert.ToInt16(generator.Next(0, 5));
                                }
                            }
                        }

                        string messageFromClient = null;
                        messageFromClient = JsonConvert.SerializeObject(myMoves);
                        ClientSocket.Send(System.Text.Encoding.ASCII.GetBytes(messageFromClient),
                            0, messageFromClient.Length, SocketFlags.None);
                        myMoves.resetMoves();
                        //byte[] MsgFromServer = new byte[1024];
                        //int size = ClientSocket.Receive(MsgFromServer);

                       // byte[] MsgFromServer_moves = new byte[1024];
                       // int size_moves = ClientSocket.Receive(MsgFromServer_moves);
                        //lastKMoves = JsonConvert.DeserializeObject<Positions>(System.Text.Encoding.ASCII.GetString(MsgFromServer_moves, 0, size_moves));

                    }
                    catch
                    {
                        if (Program.error_flag == false)
                        {
                            Console.WriteLine("connection failure");
                            Program.error_flag = true;
                        }
                        ClientSocket.Close();
                    }

                }
                ClientSocket.Close();
            }
            catch
            {
                Console.WriteLine("server not found");
                Console.ReadLine();
            }
        
        }
    }
}