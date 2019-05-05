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
        public char[,] m_cBoard;
        private Board m_board;
        
        public Cop(Board a_board, ref char[,] a_cBoard)
        {
            m_board = a_board;
            m_cBoard = a_cBoard;
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
            if (a_16Direction == 1)
            {
                if (m_cBoard[m_16CopPosition[0] - 1, m_16CopPosition[1]] != 'W' && m_cBoard[m_16CopPosition[0] - 1, m_16CopPosition[1]] != 'G')
                {
                    m_16CopPosition[0] = Convert.ToInt16(m_16CopPosition[0] - 1);
                }
            }
            else if (a_16Direction == 2)
            {
                if (m_cBoard[m_16CopPosition[0], m_16CopPosition[1] + 1] != 'W' && m_cBoard[m_16CopPosition[0], m_16CopPosition[1] + 1] != 'G')
                {
                    m_16CopPosition[1] = Convert.ToInt16(m_16CopPosition[1] + 1);
                }
            }
            else if (a_16Direction == 3)
            {
                if (m_cBoard[m_16CopPosition[0] + 1, m_16CopPosition[1]] != 'W' && m_cBoard[m_16CopPosition[0] + 1, m_16CopPosition[1]] != 'G')
                {
                    m_16CopPosition[0] = Convert.ToInt16(m_16CopPosition[0] + 1);
                }
            }
            else if (a_16Direction == 4)
            {
                if (m_cBoard[m_16CopPosition[0], m_16CopPosition[1] - 1] != 'W' && m_cBoard[m_16CopPosition[0], m_16CopPosition[1] - 1] != 'G')
                {
                    m_16CopPosition[1] = Convert.ToInt16(m_16CopPosition[1] - 1);
                }
            }
            m_board.mapBoard();
        }
    }
}
