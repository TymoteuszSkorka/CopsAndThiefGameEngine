using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace main
{
    class Board
    {
        private InitialMap firstBoard;
        private Positions boardPositions;
        private Settings m_Settings;
        private short m_16NumOfRows;
        private short m_16NumOfColumns;
        public int m_32ThiefPayment;
        public int m_32CopsPayment;
        private short kClock;
        private int m_32MaxNumberOfIterations;
        private int m_32CurrentIteration;
        private short m_16SizeOfWall;
        private short m_16SizeOfGate;
        private List<Cop> m_listOfCops;
        private List<Wall> m_listOfWalls;
        private List<Gate> m_listOfGates;
        private Thief m_Thief;
        public bool m_bIfGameOver;
        public string winner;
        private Random generator;
        private char[,] m_cBoardMap;

        public Board(short a_16NumOfRows, short a_16NumOfColumns, int a_32MaxNumberOfIterations, ref Settings a_Settings, ref InitialMap a_firsBoard,
            ref Positions a_positions)
        {
            m_16NumOfColumns = a_16NumOfColumns;
            m_16NumOfRows = a_16NumOfRows;
            m_32MaxNumberOfIterations = a_32MaxNumberOfIterations;
            m_Settings = a_Settings;
            firstBoard = a_firsBoard;
            boardPositions = a_positions;
        }

        public void Init(short a_kClock = 5, short a_16NumOfCops = 5, short a_16NumOfWalls = 4, short a_16NumOfGates = 2, short a_16SizeOfWalls = 4, short a_16SizeOfGates = 2,
            float a_fProbabilityOfWallMove = 0.75f, float a_fProbabilityOfGateMove = 0.5f, float a_fProbabilityOfWallChangeDir = 0.05f,
            float a_fProbabilityOfGateChangeDir = 0.01f)
        {
            kClock = a_kClock;
            m_16SizeOfWall = a_16SizeOfWalls;
            m_16SizeOfGate = a_16SizeOfGates;
            m_32ThiefPayment = 0;
            m_32CopsPayment = 0;
            m_32CurrentIteration = 0;
            m_bIfGameOver = false;
            generator = new Random();
            m_cBoardMap = new char[m_16NumOfRows, m_16NumOfColumns];
            firstBoard.Init(a_16NumOfCops, a_16NumOfWalls, a_16NumOfGates, a_16SizeOfWalls, a_16SizeOfGates);
            boardPositions.Init(kClock, a_16NumOfCops, a_16NumOfWalls, a_16NumOfGates, a_16SizeOfWalls, a_16SizeOfGates);
            m_listOfCops = new List<Cop>();
            m_listOfGates = new List<Gate>();
            m_listOfWalls = new List<Wall>();
            for (int i = 0; i < m_16NumOfRows; ++i)
            {
                for (int j = 0; j < m_16NumOfColumns; ++j)
                {
                    m_cBoardMap[i, j] = '0';
                }
            }
            m_Thief = new Thief(this, ref m_cBoardMap);
            firstBoard.ThiefPos[0] = m_Thief.m_16ThiefPosition[0];
            firstBoard.ThiefPos[0] = m_Thief.m_16ThiefPosition[1];
            for (int i = 0; i < a_16NumOfCops; ++i)
            {
                m_listOfCops.Add(new Cop(this, ref m_cBoardMap));
                firstBoard.CopsPos[i, 0] = m_listOfCops[i].m_16CopPosition[0];
                firstBoard.CopsPos[i, 1] = m_listOfCops[i].m_16CopPosition[1];
            }
            for (int i = 0; i < a_16NumOfWalls; ++i)
            {
                m_listOfWalls.Add(new Wall(this, ref m_cBoardMap, ref generator, Convert.ToBoolean(generator.Next(0, 2)),
                    a_16NumOfWalls, a_fProbabilityOfWallMove,  a_fProbabilityOfWallChangeDir));
                for (int j = 0; j < m_16SizeOfWall; ++j)
                {
                    firstBoard.WallsPos[i, j, 0] = m_listOfWalls[i].m_16WallPosition[j, 0];
                    firstBoard.WallsPos[i, j, 1] = m_listOfWalls[i].m_16WallPosition[j, 1];
                }
            }
            for (int i = 0; i < a_16NumOfGates; ++i)
            {
                m_listOfGates.Add(new Gate(this, ref m_cBoardMap, ref generator, a_16SizeOfGates, a_fProbabilityOfGateMove, a_fProbabilityOfGateChangeDir));
                for (int j = 0; j < m_16SizeOfGate; ++j)
                {
                    firstBoard.GatesPos[i, j, 0] = m_listOfGates[i].m_16GatePosition[j, 0];
                    firstBoard.GatesPos[i, j, 1] = m_listOfGates[i].m_16GatePosition[j, 1];
                }
            }
            m_Settings.Init(m_16NumOfRows, m_16NumOfColumns, a_16NumOfCops, a_16NumOfWalls, a_16NumOfGates, a_16SizeOfWalls, a_16SizeOfGates,
                m_32MaxNumberOfIterations, 5, a_fProbabilityOfWallMove, a_fProbabilityOfWallChangeDir, a_fProbabilityOfGateMove, a_fProbabilityOfGateChangeDir);
        }

        public void mapBoard()
        {
            for (int i = 0; i < m_16NumOfRows; ++i)
            {
                for (int j = 0; j < m_16NumOfColumns; ++j)
                {
                    m_cBoardMap[i, j] = '0';
                    if (i == 0 || j == 0 || i == m_16NumOfRows - 1 || j == m_16NumOfColumns - 1)
                    {
                        m_cBoardMap[i, j] = 'W';
                    }
                    for (int k = 0; k < m_listOfWalls.Count; ++k)
                    {
                        for (int h = 0; h < m_listOfWalls[k].m_16WallPosition.GetLength(0); ++h)
                        {
                            if (m_listOfWalls[k].m_16WallPosition[h, 0] == i && m_listOfWalls[k].m_16WallPosition[h, 1] == j)
                            {
                                m_cBoardMap[i, j] = 'W';
                            }
                        }
                    }
                    for (int k = 0; k < m_listOfGates.Count; ++k)
                    {
                        for (int h = 0; h < m_listOfGates[k].m_16GatePosition.GetLength(0); ++h)
                        {
                            if (m_listOfGates[k].m_16GatePosition[h, 0] == i && m_listOfGates[k].m_16GatePosition[h, 1] == j)
                            {
                                m_cBoardMap[i, j] = 'G';
                            }
                        }
                    }
                    if (m_Thief.m_16ThiefPosition[0] == i && m_Thief.m_16ThiefPosition[1] == j)
                    {
                        m_cBoardMap[i, j] = 'T';
                    }
                    for (int k = 0; k < m_listOfCops.Count; ++k)
                    {
                        if (m_listOfCops[k].m_16CopPosition[0] == i && m_listOfCops[k].m_16CopPosition[1] == j)
                        {
                            m_cBoardMap[i, j] = 'C';
                        }
                    }
                }
            }
        }

        public void PrintBoard()
        {
            for (int i = 0; i < m_16NumOfRows; ++i)
            {
                for (int j = 0; j < m_16NumOfColumns; ++j)
                {
                    if (m_cBoardMap[i, j] == 'W')
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                    }
                    else if (m_cBoardMap[i, j] == 'G')
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    else if (m_cBoardMap[i, j] == 'T')
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else if (m_cBoardMap[i, j] == 'C')
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    Console.Write(" " + m_cBoardMap[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        public void simulate(short[] a_CopsMove, short a_ThiefMove)
        {
            if (m_32CurrentIteration < m_32MaxNumberOfIterations)
            {
                if (m_bIfGameOver == false)
                {
                    for (int i = 0; i < m_listOfWalls.Count; ++i)
                    {
                        m_listOfWalls[i].Move();
                        for (int j = 0; j < m_16SizeOfWall; ++j)
                        {
                            boardPositions.WallsPos[m_32CurrentIteration % kClock, i, j, 0] = m_listOfWalls[i].m_16WallPosition[j, 0];
                            boardPositions.WallsPos[m_32CurrentIteration % kClock, i, j, 1] = m_listOfWalls[i].m_16WallPosition[j, 1];
                        }
                    }
                    for (int i = 0; i < m_listOfGates.Count; ++i)
                    {
                        m_listOfGates[i].Move();
                        for (int j = 0; j < m_16SizeOfGate; ++j)
                        {
                            boardPositions.GatesPos[m_32CurrentIteration % kClock, i, j, 0] = m_listOfGates[i].m_16GatePosition[j, 0];
                            boardPositions.GatesPos[m_32CurrentIteration % kClock, i, j, 1] = m_listOfGates[i].m_16GatePosition[j, 1];
                        }
                    }
                    for (int i = 0; i < m_listOfCops.Count; ++i)
                    {
                        m_listOfCops[i].Move(a_CopsMove[i]);
                        boardPositions.CopsPos[m_32CurrentIteration % kClock, i, 0] = m_listOfCops[i].m_16CopPosition[0];
                        boardPositions.CopsPos[m_32CurrentIteration % kClock, i, 1] = m_listOfCops[i].m_16CopPosition[1];
                    }
                    m_Thief.Move(a_ThiefMove);
                    boardPositions.ThiefPos[m_32CurrentIteration % kClock, 0] = m_Thief.m_16ThiefPosition[0];
                    boardPositions.ThiefPos[m_32CurrentIteration % kClock, 1] = m_Thief.m_16ThiefPosition[1];
                }
                for (int i = 0; i < m_listOfGates.Count; ++i)
                {
                    for (int j = 0; j < m_listOfGates[i].m_16GatePosition.GetLength(0); ++j)
                    {
                        if (m_Thief.m_16ThiefPosition[0] == m_listOfGates[i].m_16GatePosition[j, 0]
                            && m_Thief.m_16ThiefPosition[1] == m_listOfGates[i].m_16GatePosition[j, 1])
                        {
                            m_bIfGameOver = true;
                            m_32ThiefPayment = 2 * m_32MaxNumberOfIterations - m_32CurrentIteration - 1;
                            m_32CopsPayment = -m_32ThiefPayment;
                            winner = "Thief";
                        }
                    }
                }
                for (int i = 0; i < m_listOfCops.Count; ++i)
                {
                    if ((m_listOfCops[i].m_16CopPosition[0] + 1 == m_Thief.m_16ThiefPosition[0] && m_listOfCops[i].m_16CopPosition[1] == m_Thief.m_16ThiefPosition[1])
                        || (m_listOfCops[i].m_16CopPosition[0] - 1 == m_Thief.m_16ThiefPosition[0] && m_listOfCops[i].m_16CopPosition[1] == m_Thief.m_16ThiefPosition[1])
                        || (m_listOfCops[i].m_16CopPosition[1] + 1 == m_Thief.m_16ThiefPosition[1] && m_listOfCops[i].m_16CopPosition[0] == m_Thief.m_16ThiefPosition[0])
                        || (m_listOfCops[i].m_16CopPosition[1] - 1 == m_Thief.m_16ThiefPosition[1] && m_listOfCops[i].m_16CopPosition[0] == m_Thief.m_16ThiefPosition[0])
                        || (m_listOfCops[i].m_16CopPosition[1] == m_Thief.m_16ThiefPosition[1] && m_listOfCops[i].m_16CopPosition[0] == m_Thief.m_16ThiefPosition[0]))
                    {
                        m_bIfGameOver = true;
                        winner = "COP";
                        m_32ThiefPayment = m_32CurrentIteration;
                        m_32CopsPayment = -m_32ThiefPayment;
                    }

                }
                ++m_32CurrentIteration;
            }
            else
            {
                m_32ThiefPayment = m_32MaxNumberOfIterations;
                m_32CopsPayment = -m_32ThiefPayment;
                m_bIfGameOver = true;
                winner = "Thief";
            }
        }
    }
}
