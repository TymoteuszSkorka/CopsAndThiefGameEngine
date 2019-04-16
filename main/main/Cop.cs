using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace main
{
    class Cop
    {
        public short[] m_16CopPosition;
        
        public Cop(ref char[,] a_cBoard)
        {
            m_16CopPosition = new short[2];
            Random generator = new Random();
            do
            {
                m_16CopPosition[0] = Convert.ToInt16(generator.Next(1, 21));
                m_16CopPosition[1] = Convert.ToInt16(generator.Next(1, 21));
            }
            while (a_cBoard[m_16CopPosition[0], m_16CopPosition[1]] != '0');
            a_cBoard[m_16CopPosition[0], m_16CopPosition[1]] = 'C';
        }

        public void Move(short a_16Direction)
        {

        }
    }
}
