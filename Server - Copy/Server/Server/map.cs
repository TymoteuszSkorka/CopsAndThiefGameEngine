using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class Map
    {
        private int mapsize = 10;
        private int[][] mapa;
        public Map()
        {
            Console.WriteLine("konstruktor");
        }
        public string Showmap()
        {
            Console.WriteLine("showmap");
            return ("showmap");
        }

    }
}
