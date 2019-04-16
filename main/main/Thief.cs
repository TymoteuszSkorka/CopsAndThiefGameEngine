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

        public Thief(ref char[,] a_cBoard)
        {
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

        }
    }
}
