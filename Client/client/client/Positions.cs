using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace main
{
    class Positions
    {
        private short kClock;
        public short[,] ThiefPos;
        public short[,,] CopsPos;
        public short[,,,] WallsPos;
        public short[,,,] GatesPos;

        public void Init(short a_kClock, short a_16NumOfCops, short a_16NumOfWalls, short a_16NumOfGates, short a_16SizeOfWalls, short a_16SizeOfGates)
        {
            kClock = a_kClock;
            ThiefPos = new short[kClock, 2];
            CopsPos = new short[kClock,a_16NumOfCops, 2];
            WallsPos = new short[kClock, a_16NumOfWalls, a_16SizeOfWalls, 2];
            GatesPos = new short[kClock, a_16NumOfGates, a_16SizeOfGates, 2];
        }
    }
}
