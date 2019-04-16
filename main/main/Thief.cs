using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace main
{
    class Thief
    {
        public short[] m_16ThiefPosition;
        public char[,] m_cBoard;

        public Thief(ref char[,] a_cBoard)
        {
            m_cBoard = a_cBoard;
            m_16ThiefPosition = new short[2];
            Random generator = new Random();
            do
            {
                m_16ThiefPosition[0] = Convert.ToInt16(generator.Next(1, 21));
                m_16ThiefPosition[1] = Convert.ToInt16(generator.Next(1, 21));
            }
            while (a_cBoard[m_16ThiefPosition[0], m_16ThiefPosition[1]] != '0');
            a_cBoard[m_16ThiefPosition[0], m_16ThiefPosition[1]] = 'T';
        }

        public void Move(short a_16Direction)
        {
            if (a_16Direction == 1)
            {
                if (m_cBoard[m_16ThiefPosition[0] - 1, m_16ThiefPosition[1]] != 'W')
                {
                    m_16ThiefPosition[0] = Convert.ToInt16(m_16ThiefPosition[0] - 1);
                }
            }
            else if (a_16Direction == 2)
            {
                if (m_cBoard[m_16ThiefPosition[0], m_16ThiefPosition[1] + 1] != 'W')
                {
                    m_16ThiefPosition[1] = Convert.ToInt16(m_16ThiefPosition[1] + 1);
                }
            }
            else if (a_16Direction == 3)
            {
                if (m_cBoard[m_16ThiefPosition[0] + 1, m_16ThiefPosition[1]] != 'W')
                {
                    m_16ThiefPosition[0] = Convert.ToInt16(m_16ThiefPosition[0] + 1);
                }
            }
            else if (a_16Direction == 4)
            {
                if (m_cBoard[m_16ThiefPosition[0], m_16ThiefPosition[1] - 1] != 'W')
                {
                    m_16ThiefPosition[1] = Convert.ToInt16(m_16ThiefPosition[1] - 1);
                }
            }
        }
    }
}
