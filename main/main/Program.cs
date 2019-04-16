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
            myBoard.mapBoard();
            myBoard.PrintBoard();
            Console.Read();
        }
    }
}
