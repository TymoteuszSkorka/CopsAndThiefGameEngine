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
        static bool Police = false;
        static bool Thief = false;
        static int start = 0;
        static bool disconnect_event = false;
        

        static int licznik = 0;

        static void Main(string[] args)
        {
            InitialMap initBoard = new InitialMap();
            Positions boardPos = new Positions();
            Settings settings = new Settings();

            Board plansza = new Board(22, 22, 200, ref settings, ref initBoard, ref boardPos);

            plansza.Init();
            plansza.mapBoard();

            int port = 13000;
            string IpAddress = "127.0.0.1";
            Socket ServerListener = new Socket(AddressFamily
                .InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(IpAddress), port);
            ServerListener.Bind(ep);
            ServerListener.Listen(0);
            Console.WriteLine("Server is Listening ...");
            Socket ClientSocket = default(Socket);

            Program p = new Program();
            
                while (true&&counter<2)
                {
                    ClientSocket = ServerListener.Accept();
                    Program.counter++;
                    Console.WriteLine(counter + " Clients connected");
                    //towrzymy wątek z graczem
                    Thread UserThread = new Thread(new ThreadStart(() => p.User(ClientSocket, plansza,settings,initBoard,boardPos)));
                    UserThread.Start();                   

                }

            }

        public void handshake(Socket client, Board plansza, Settings settings)
        {
            string handShakeMsg = "Welcome to Cops&Thiefs game. Type 'T' for Thief or 'P' for Policeman: ";
            client.Send(System.Text.Encoding.ASCII.GetBytes(handShakeMsg),
                0, handShakeMsg.Length, SocketFlags.None);
            try
            {
                //odbiera wiadomośc klienta co wybrał
                byte[] msg = new byte[1024];
                int size = client.Receive(msg);
                string asciiString = Encoding.ASCII.GetString(msg, 0, msg.Length);

                //jeżeli źle to zatrzymuje się w pętli i oczekuje na poprawną odpowiedź
                while ((string.Compare(asciiString, "T") != 0 || Program.Thief == true) && ((string.Compare(asciiString, "P") != 0) || Program.Police == true))
                {
                    string again = "already taken or input error, pick again:";
                    client.Send(System.Text.Encoding.ASCII.GetBytes(again),
                    0, again.Length, SocketFlags.None);
                    size = client.Receive(msg);
                    asciiString = Encoding.ASCII.GetString(msg, 0, size);
                }

                //przypisuje do ROLE literkę
                string role;
                if (string.Compare(asciiString, "T") == 0)
                {
                    role = "Thief";
                    Program.Thief = true;

                }
                else
                {
                    role = "Policeman";
                    Program.Police = true;
                }

                //wiadomosć o powodzeniu wybrania postaci
                string welcome = "welcome to the game Mr." + role + "!!!";
                client.Send(System.Text.Encoding.ASCII.GetBytes(welcome),
                    0, welcome.Length, SocketFlags.None);

                //tutaj trzeba wylosować stan planszy jakoś
                //
                //
                //
                //
                //wysyłamy pierwszy wylosowany stan planszy
                string json = JsonConvert.SerializeObject(settings);
                byte[] msg1 = Encoding.ASCII.GetBytes(json);
                int size2 = msg1.Length;
                client.Send(msg1, 0, size2, SocketFlags.None);
            }
            catch (System.Net.Sockets.SocketException sockEx)
            {
                //Console.WriteLine(sockEx.ErrorCode);
                Program.counter--;
                client.Close();
                Console.WriteLine("Client disconnected");
                Console.WriteLine("Clients connected:" + counter);
                Program.Police = false;
                Program.Thief = false;
                Program.start = 0;
            }

        }

        public void waiting_for_player(Socket client, Board plansza, InitialMap initBoard)
        {
            try
            {
                int Flaga = 0;
                byte[] tmp_msg = new byte[1024];
                client.Receive(tmp_msg);//wiadomosc potwierdzajaca gotowosc
                //czekanie na przeciwnika
                while (Program.Thief == false || Program.Police == false)
                {

                    if (Flaga == 0)
                    {
                        Console.WriteLine("czekanie na przeciwnika...");
                        Flaga = 1;
                    }
                    if (!client.Connected)
                    {
                        Console.WriteLine("client disconnected");
                        client.Close();
                        
                    }

                }



                Program.start++;
                while (start < 2)
                {
                    //pierwszy który się połączył musi poczekać na drugiego aż odpiszę i wtedy time jest ustawiany
                }

                start++;

                //wysyłamy wylosowany stan planszy
                string json = JsonConvert.SerializeObject(initBoard);
                byte[] msg1 = Encoding.ASCII.GetBytes(json);
                int size2 = msg1.Length;

                client.Send(msg1, 0, size2, SocketFlags.None);

                client.ReceiveTimeout = 3000;
            }
            catch
            {
                Program.counter--;
                client.Close();
                Console.WriteLine("Client disconnected");
                Console.WriteLine("Clients connected:" + counter);
                Program.Police = false;
                Program.Thief = false;
                Program.start = 0;
            }
        }

        public void User(Socket client, Board plansza, Settings settings, InitialMap initBoard, Positions boardPos)
        {
            handshake(client, plansza, settings);
            waiting_for_player(client, plansza, initBoard);

        while_func:
            while (true)        
            {
                bool error_flag = false;
                try
                {
                    if (error_flag == true)
                    {
                        //jeżeli nie zmieścił się w ruchach 
                        error_flag = false;
                        byte[] err_msg_recive = new byte[1024];
                        int err_size_recive = client.Receive(err_msg_recive);

                        string err_json = JsonConvert.SerializeObject(plansza);
                        byte[] err_msg = Encoding.ASCII.GetBytes(err_json);
                        int err_size = err_msg.Length;

                        client.Send(err_msg, 0, err_size, SocketFlags.None);


                        string json_moves_err = JsonConvert.SerializeObject(boardPos);
                        byte[] msg1_moves_err = Encoding.ASCII.GetBytes(json_moves_err);
                        int size2_moves_err = msg1_moves_err.Length;

                        client.Send(msg1_moves_err, 0, size2_moves_err, SocketFlags.None);

                        //goto while_func;
                    }
                    //client.ReceiveTimeout = 3000;

                    byte[] msg = new byte[1024];


                    int size = client.Receive(msg);
                    string asciiString = Encoding.ASCII.GetString(msg, 0, msg.Length);
                    Console.WriteLine(asciiString);

                    string json = JsonConvert.SerializeObject(plansza);
                    byte[] msg1 = Encoding.ASCII.GetBytes(json);
                    int size2 = msg1.Length;

                    client.Send(msg1, 0, size2, SocketFlags.None);


                    string json_moves = JsonConvert.SerializeObject(boardPos);
                    byte[] msg1_moves = Encoding.ASCII.GetBytes(json_moves);
                    int size2_moves = msg1_moves.Length;

                    client.Send(msg1_moves, 0, size2_moves, SocketFlags.None);


                    licznik++;

                }
                catch(System.Net.Sockets.SocketException sockEx)
                {
                    
                    //Console.WriteLine(sockEx.ErrorCode);
                    if (sockEx.ErrorCode != 10060)
                    {
                        Program.counter--;
                        client.Close();
                        Console.WriteLine("Client disconnected");
                        Console.WriteLine("Clients connected:" + counter);
                        Program.Police = false;
                        Program.Thief = false;
                        Program.start = 0;
                    }
                    else if(sockEx.ErrorCode == 10060)
                    {
                        Console.WriteLine("default settings, licznik : "+Program.licznik);
                        error_flag = true;
                        goto while_func;

                    }
                }

            }
        }
    }
}