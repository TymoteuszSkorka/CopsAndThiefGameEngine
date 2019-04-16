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
            Board myBoard = new Board(22, 22);
            myBoard.Init();
            while(true)
            {
                myBoard.mapBoard();
                myBoard.PrintBoard();
                System.Threading.Thread.Sleep(500);
                Console.Clear();
                myBoard.simulate();
            }
            Console.Read();
        }
    }
}
