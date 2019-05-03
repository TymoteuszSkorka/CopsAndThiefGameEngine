using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace main
{
    class Settings
    {
        public short xDim;
        public short yDim;
        public short numOfCops;
        public short numOfWalls;
        public short numOfGates;
        public short sizeOfWalls;
        public short sizeOfGates;
        public int maxNumOfIterations;
        public short kClock;
        public float probOfWallMove;
        public float probOfWallChangeDir;
        public float probOfGateMove;
        public float probOfGateChangeDir;

        public void Init(short a_xDim, short a_yDim, short a_numOfCops, short a_numOfWalls, short a_numOfGates, short a_sizeOfWalls,
            short a_sizeOfGates, int a_maxNumOfIterations, short a_kClock, float a_probOfWallMove, float a_probOfWallChangeDir, 
            float a_probOfGateMove, float a_probOfGateChangeDir)
        {
            xDim = a_xDim;
            yDim = a_yDim;
            numOfCops = a_numOfCops;
            numOfWalls = a_numOfWalls;
            numOfGates = a_numOfGates;
            sizeOfWalls = a_sizeOfWalls;
            sizeOfGates = a_sizeOfGates;
            maxNumOfIterations = a_maxNumOfIterations;
            kClock = a_kClock;
            probOfWallMove = a_probOfWallMove;
            probOfGateMove = a_probOfGateMove;
            probOfWallChangeDir = a_probOfWallChangeDir;
            probOfGateChangeDir = a_probOfGateChangeDir;
        }
    }
}
