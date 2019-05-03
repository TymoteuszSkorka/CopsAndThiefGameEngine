using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace main
{
    class Program
    {
        static void Main(string[] args)
        {
            InitialMap initBoard = new InitialMap();
            Positions boardPos = new Positions();
            Settings settings = new Settings();
            Board myBoard = new Board(22, 22, 200, ref settings, ref initBoard, ref boardPos);
            myBoard.Init();
            while(myBoard.m_bIfGameOver == false)
            {
                myBoard.mapBoard();
                myBoard.PrintBoard();
                System.Threading.Thread.Sleep(200);
                if (myBoard.m_bIfGameOver == false)
                    Console.Clear();
                myBoard.simulate();
            }
            myBoard.mapBoard();
            myBoard.PrintBoard();
            Console.WriteLine("Zwycieska druzyna: " + myBoard.winner);
            Console.WriteLine("Wyplata zlodzieja: " + myBoard.m_32ThiefPayment);
            Console.WriteLine("Wyplata policjantow: " + myBoard.m_32CopsPayment);
            Console.Read();
        }
    }
}
