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

        public Board(short a_16NumOfRows, short a_16NumOfColumns)
        {
            m_16NumOfColumns = a_16NumOfColumns;
            m_16NumOfRows = a_16NumOfRows;
        }

        public void Init(short a_16NumOfCops = 5, short a_16NumOfWalls = 4, short a_16NumOfGates = 2)
        {
            m_Thief = new Thief();
            for(int i = 0; i < a_16NumOfCops; ++i)
            {
                m_listOfCops.Add(new Cop());
            }
            for (int i = 0; i < a_16NumOfWalls; ++i)
            {
                m_listOfWalls.Add(new Wall());
            }
            for (int i = 0; i < a_16NumOfGates; ++i)
            {
                m_listOfGates.Add(new Gate());
            }
        }

        public void PrintBoard()
        {
            for (int i = 0; i < m_16NumOfRows; ++i)
            {
                for (int j = 0; j < m_16NumOfColumns; ++j)
                {
                    if (i == 0 || j == 0 || i == m_16NumOfRows - 1 || j == m_16NumOfColumns)
                    {
                        Console.Write(" W ");
                    }
                    for (int k = 0; k < m_listOfCops.Count; ++k)
                    {
                        if (m_listOfCops[k].m_16CopPosition[0] == i || m_listOfCops[k].m_16CopPosition[j] == j)
                        {
                            Console.Write(" C ");
                        }
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
