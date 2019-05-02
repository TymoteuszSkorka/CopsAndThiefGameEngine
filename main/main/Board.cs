using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace main
{
    class Board
    {
        private short m_16NumOfRows;
        private short m_16NumOfColumns;
        public int m_32ThiefPayment;
        public int m_32CopsPayment;
        private int m_32MaxNumberOfIterations;
        private int m_32CurrentIteration;
        private List<Cop> m_listOfCops;
        private List<Wall> m_listOfWalls;
        private List<Gate> m_listOfGates;
        private Thief m_Thief;
        public bool m_bIfGameOver;
        public string winner;
        Random generator;
        private char[,] m_cBoardMap;

        public Board(short a_16NumOfRows, short a_16NumOfColumns, int a_32MaxNumberOfIterations)
        {
            m_16NumOfColumns = a_16NumOfColumns;
            m_16NumOfRows = a_16NumOfRows;
            m_32MaxNumberOfIterations = a_32MaxNumberOfIterations;
        }

        public void Init(short a_16NumOfCops = 5, short a_16NumOfWalls = 4, short a_16NumOfGates = 2)
        {
            m_32ThiefPayment = 0;
            m_32CopsPayment = 0;
            m_32CurrentIteration = 0;
            m_bIfGameOver = false;
            generator = new Random();
            m_cBoardMap = new char[m_16NumOfRows, m_16NumOfColumns];
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
            for(int i = 0; i < a_16NumOfCops; ++i)
            {
                m_listOfCops.Add(new Cop(this, ref m_cBoardMap));
            }
            for (int i = 0; i < a_16NumOfWalls; ++i)
            {
                m_listOfWalls.Add(new Wall(this, ref m_cBoardMap, ref generator, Convert.ToBoolean(generator.Next(0, 2))));
            }
            for (int i = 0; i < a_16NumOfGates; ++i)
            {
                m_listOfGates.Add(new Gate(this, ref m_cBoardMap, ref generator));
            }
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

        public void simulate()
        {
            if (m_32CurrentIteration < m_32MaxNumberOfIterations)
            {
                if (m_bIfGameOver == false)
                {
                    for (int i = 0; i < m_listOfWalls.Count; ++i)
                    {
                        m_listOfWalls[i].Move();
                    }
                    for (int i = 0; i < m_listOfGates.Count; ++i)
                    {
                        m_listOfGates[i].Move();
                    }
                    for (int i = 0; i < m_listOfCops.Count; ++i)
                    {
                        m_listOfCops[i].Move(Convert.ToInt16(generator.Next(0, 5)));
                    }
                    m_Thief.Move(Convert.ToInt16(generator.Next(0, 5)));
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
