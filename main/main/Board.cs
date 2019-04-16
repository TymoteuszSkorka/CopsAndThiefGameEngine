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
        private List<Cop> m_listOfCops;
        private List<Wall> m_listOfWalls;
        private List<Gate> m_listOfGates;
        private Thief m_Thief;
        private char[,] m_cBoardMap;

        public Board(short a_16NumOfRows, short a_16NumOfColumns)
        {
            m_16NumOfColumns = a_16NumOfColumns;
            m_16NumOfRows = a_16NumOfRows;
        }

        public void Init(short a_16NumOfCops = 5, short a_16NumOfWalls = 4, short a_16NumOfGates = 2)
        {
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
            m_Thief = new Thief(ref m_cBoardMap);
            for(int i = 0; i < a_16NumOfCops; ++i)
            {
                m_listOfCops.Add(new Cop(ref m_cBoardMap));
            }
            for (int i = 0; i < a_16NumOfWalls; ++i)
            {
                m_listOfWalls.Add(new Wall(ref m_cBoardMap));
            }
            for (int i = 0; i < a_16NumOfGates; ++i)
            {
                m_listOfGates.Add(new Gate(ref m_cBoardMap));
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
                }
            }
        }

        public void PrintBoard()
        {
            for (int i = 0; i < m_16NumOfRows; ++i)
            {
                for (int j = 0; j < m_16NumOfColumns; ++j)
                {
                    if (m_cBoardMap[i, j] == 'C')
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                    }
                    else if (m_cBoardMap[i, j] == 'T')
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else if (m_cBoardMap[i, j] == 'W')
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                    }
                    else if (m_cBoardMap[i, j] == 'G')
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
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
    }
}
