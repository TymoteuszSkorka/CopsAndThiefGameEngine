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
                //Thread UserThread = new Thread(new ThreadStart(() => p.User(ClientSocket, plansza)));

                //wiadomosc powitalna czeka na odpowiedź klienta czym chce grac :
                string handShakeMsg = "Welcome to Cops&Thiefs game. Type 'T' for Thief or 'P' for Policeman: ";
                ClientSocket.Send(System.Text.Encoding.ASCII.GetBytes(handShakeMsg),
                    0, handShakeMsg.Length, SocketFlags.None);

                
                try
                {
                    //odbiera wiadomośc klienta co wybrał
                    byte[] msg = new byte[1024];
                    int size = ClientSocket.Receive(msg);
                    string asciiString = Encoding.ASCII.GetString(msg, 0, msg.Length);
                    //jeżeli źle to zatrzymuje się w pętli i oczekuje na poprawną odpowiedź
                    while ((string.Compare(asciiString, "T")!=0 || Program.Thief==true ) && (string.Compare(asciiString, "P") != 0) || Program.Police == true)
                    { 
                        string again = "already taken or input error, pick again:";
                        ClientSocket.Send(System.Text.Encoding.ASCII.GetBytes(again),
                        0, again.Length, SocketFlags.None);
                        size = ClientSocket.Receive(msg);
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
                    string welcome = "welcome to the game Mr." +role+ "!!!";
                    ClientSocket.Send(System.Text.Encoding.ASCII.GetBytes(welcome),
                        0, welcome.Length, SocketFlags.None);

                    //tutaj trzeba wylosować stan planszy jakoś
                    //
                    //
                    //
                    //

                    //wysyłamy pierwszy wylosowany stan planszy
                    string json = JsonConvert.SerializeObject(plansza);
                    byte[] msg1 = Encoding.ASCII.GetBytes(json);
                    int size2 = msg1.Length;
                    ClientSocket.Send(msg1, 0, size2, SocketFlags.None);

                    
                    //towrzymy wątek z graczem
                    Thread UserThread = new Thread(new ThreadStart(() => p.User(ClientSocket, plansza)));
                    UserThread.Start();
                }
                catch(System.Net.Sockets.SocketException sockEx)
                {
                    //Console.WriteLine(sockEx.ErrorCode);
                    Program.counter--;
                    ClientSocket.Close();
                    Console.WriteLine("Client disconnected");
                    Console.WriteLine("Clients connected:" + counter);
                    Program.Police = false;
                    Program.Thief = false;
                    Program.start = 0;
                }
                
            }

        }
        public void User(Socket client, Board plansza)
        {
            int Flaga = 0;
            //czekanie na przeciwnika
            while (Program.Thief == false || Program.Police == false)
            {
               
                if (Flaga == 0)
                {
                    Console.WriteLine("czekanie na przeciwnika...");
                    Flaga = 1;
                }
            }
            //jezeli nie ma 2 graczy i oboje nie potwierdzili swojej gotowosci
            try
            {
                while (start < 2 && counter > 1)
                {
                    byte[] msg = new byte[1024];
                    int size = client.Receive(msg);
                    start++;
                }
            //jak oboje gotowi to startujemy, i ustawiamy licznik na wyslanie wiadomosci
                client.ReceiveTimeout = 5000;
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
             
            
            while (client.Connected)
            {
                
                try
                {
    
                    //serwer czeka na odpowiedź clienta
                    byte[] msg = new byte[1024];
                    int size = client.Receive(msg);
            
                    string asciiString = Encoding.ASCII.GetString(msg, 0, msg.Length);
                    //Console.WriteLine(asciiString);

                    plansza.m_16NumOfRows += 1;
                    plansza.m_16NumOfColumns += 1;
                    string json = JsonConvert.SerializeObject(plansza);
                    byte[] msg1 = Encoding.ASCII.GetBytes(json);
                    int size2 = msg1.Length;
                    
                    client.Send(msg1, 0, size2, SocketFlags.None);
                    
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
                    //jeżeli serwer nie odpowie w ciągu okienka zmienia plansze na losowe pozycje
                        Console.WriteLine("default settings for board:");
                        plansza.m_16NumOfColumns += 1;
                        plansza.m_16NumOfRows += 1;

                    }
                }

            }
        }
    }
}