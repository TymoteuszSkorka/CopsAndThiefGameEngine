﻿using System;
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
        static bool isAboutToMove = false;
        static bool disconnect_event = false;
        static int iteration_num = 0;

        static int licznik = 0;

        static void Main(string[] args)
        {
            InitialMap initBoard = new InitialMap();
            Positions boardPos = new Positions();
            Settings settings = new Settings();
            Moves[] playersMoves = new Moves[2];
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

                while (true && counter < 2)
                {
                    ClientSocket = ServerListener.Accept();
                    Program.counter++;
                    Console.WriteLine(counter + " Clients connected");
                    //towrzymy wątek z graczem
                    Thread UserThread = new Thread(new ThreadStart(() => p.User(ClientSocket, ref plansza,ref settings,initBoard,ref boardPos, ref playersMoves)));
                    UserThread.Start();                   
                }
            while (plansza.m_bIfGameOver == false)
            {
                if (isAboutToMove)
                {
                    plansza.simulate(playersMoves[1].m_16Moves, playersMoves[0].m_16Moves);
                    isAboutToMove = false;
                }
            }
            Console.WriteLine("Zwyciezca: " + plansza.winner);
            Console.WriteLine("Punkty Złodzieja: " + plansza.m_32ThiefPayment);
            Console.WriteLine("Punkty Policjantów: " + plansza.m_32CopsPayment);
            Console.Read();
            }

        public string handshake(Socket client, Board plansza, Settings settings)
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
                //wysyłamy ustawienia
                string json = JsonConvert.SerializeObject(settings);
                byte[] msg1 = Encoding.ASCII.GetBytes(json);
                int size2 = msg1.Length;
                client.Send(msg1, 0, size2, SocketFlags.None);
                return role;

            }
            catch (System.Net.Sockets.SocketException sockEx)
            {
                //Console.WriteLine(sockEx.ErrorCode);
                Program.counter--;               
                client.Shutdown(SocketShutdown.Both);
                client.Close();
                if (counter < 0)
                {
                    counter = 0;
                }
                Console.WriteLine("Client disconnected");
                Console.WriteLine("Clients connected:" + counter);
                Program.Police = false;
                Program.Thief = false;
                Program.start = 0;
                return null;
            }
            
        }
        public void waiting_for_player(Socket client, Board plansza, InitialMap initBoard)
        {
            try
            {
                int Flaga = 0;
                byte[] tmp_msg = new byte[1024];
                if(client.Connected)
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

                client.ReceiveTimeout = 500;
            }
            catch
            {
                Program.counter--;
                client.Shutdown(SocketShutdown.Both);
                client.Close();
                Console.WriteLine("Client disconnected");
                if (counter < 0)
                {
                    counter = 0;
                }
                Console.WriteLine("Clients connected:" + counter);
                Program.Police = false;
                Program.Thief = false;
                Program.start = 0;
                
            }
        }

        public void User(Socket client, ref Board plansza, ref Settings settings, InitialMap initBoard, ref Positions boardPos, ref Moves[] playersMove)
        {
            string role=handshake(client, plansza, settings);
            if (client.Connected)
            {
                waiting_for_player(client, plansza, initBoard);
            }

            while (client.Connected)        
            {
                bool error_flag = false;
            while_func:
                try
                {
                    if (error_flag == true)
                    {
                        //jeżeli nie zmieścił się w ruchach
                        error_flag = false;
                        byte[] err_msg_recive = new byte[1024];
                        int err_size_recive = client.Receive(err_msg_recive);
                        string json_moves_err = JsonConvert.SerializeObject(boardPos);
                        byte[] msg1_moves_err = Encoding.ASCII.GetBytes(json_moves_err);
                        int size2_moves_err = msg1_moves_err.Length;

                        client.Send(msg1_moves_err, 0, size2_moves_err, SocketFlags.None);

                        //goto while_func;
                    }


                    byte[] msg = new byte[1024];

                    int size = client.Receive(msg);
                    try
                    {
                        Moves tmp_move = JsonConvert.DeserializeObject<Moves>(System.Text.Encoding.ASCII.GetString(msg, 0, msg.Length));
                        if (tmp_move.m_sRole == "T")
                        {
                            playersMove[0] = tmp_move;
                        }
                        else if (tmp_move.m_sRole == "P")
                        {
                            playersMove[1] = tmp_move;
                        }
                    }
                    catch
                    {
                        Console.WriteLine("ERROR");
                    }
                    
                    Program.iteration_num++;

                    while ((Program.iteration_num % 2) != 0)
                    {

                    }
                    Program.iteration_num = 0;
                    isAboutToMove = true;
                    while(isAboutToMove)
                    {

                    }
                    string json = JsonConvert.SerializeObject(boardPos);
                    byte[] msg1 = Encoding.ASCII.GetBytes(json);
                    int size2 = msg1.Length;

                    client.Send(msg1, 0, size2, SocketFlags.None);
                    licznik++;
                }
                
                catch(System.Net.Sockets.SocketException sockEx)
                {
                    
                    //Console.WriteLine(sockEx.ErrorCode);
                    if (sockEx.ErrorCode != 10060)
                    {
                        client.Shutdown(SocketShutdown.Both);
                        client.Close();
                        Program.counter--;
                        Console.WriteLine("Client disconnected");
                        if (counter < 0)
                        {
                            counter = 0;
                        }
                        Console.WriteLine("Clients connected:" + counter);
                        Program.Police = false;
                        Program.Thief = false;
                        Program.start = 0;
                    }
                    
                    else if(sockEx.ErrorCode == 10060)
                    {
                        if (role == "Thief")
                        {
                            Moves tmpMove = new Moves();
                            tmpMove.init(settings.kClock, 1, "T");
                            playersMove[0] = tmpMove;
                        }
                        else if (role == "Policeman")
                        {
                            Moves tmpMove = new Moves();
                            tmpMove.init(settings.kClock, settings.numOfCops, "C");
                            playersMove[1] = tmpMove;
                        }
                        Program.iteration_num++;
                        while ((Program.iteration_num % 2) != 0)
                        {

                        }
                        Program.iteration_num = 0;
                        isAboutToMove = true;
                        while (isAboutToMove)
                        {

                        }
                        //int iteracja = Program.iteration_num / 2;
                        //Console.WriteLine("default settings--->iteration: " + iteracja);
                        error_flag = true;
                        goto while_func;
                    }

                }

            }
        }
    }
}