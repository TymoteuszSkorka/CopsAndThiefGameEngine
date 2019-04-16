using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace main
{
    class Wall
    {
        public short[,] m_16WallPosition;
        private float m_fProbalityOfMovement;
        private float m_fProbalityOfChangingDirection;

        public Wall(ref char[,] a_cBoard, short a_16SizeofWall = 4, float a_fProbalityOfMovement = 0.5f, float a_fProbalityOfChangingDirection = 0.01f, bool a_bIsVertical = true)
        {
            m_16WallPosition = new short[a_16SizeofWall, 2];
            m_fProbalityOfMovement = a_fProbalityOfMovement;
            m_fProbalityOfChangingDirection = a_fProbalityOfChangingDirection;
            Random generator = new Random();

            if (a_bIsVertical)
            {
                do
                {
                    m_16WallPosition[0, 0] = Convert.ToInt16(generator.Next(1, 19));
                    m_16WallPosition[0, 1] = Convert.ToInt16(generator.Next(1, 19));
                }
                while (a_cBoard[m_16WallPosition[0, 0], m_16WallPosition[0, 1]] != '0' && a_cBoard[m_16WallPosition[0, 0], m_16WallPosition[0, 1] + 1] != '0' && a_cBoard[m_16WallPosition[0, 0], m_16WallPosition[0, 1] + 2] != '0' && a_cBoard[m_16WallPosition[0, 0], m_16WallPosition[0, 1] + 3] != '0');
                a_cBoard[m_16WallPosition[0, 0], m_16WallPosition[0, 1]] = 'W';
                for (short i = 1; i < a_16SizeofWall; ++i)
                {
                    m_16WallPosition[i, 0] = m_16WallPosition[0, 0];
                    m_16WallPosition[i, 1] = Convert.ToInt16(m_16WallPosition[0, 1] + i);
                    a_cBoard[m_16WallPosition[i, 0], m_16WallPosition[i, 1]] = 'W';
                }
            }
            else
            {
                do
                {
                    m_16WallPosition[0, 0] = Convert.ToInt16(generator.Next(1, 19));
                    m_16WallPosition[0, 1] = Convert.ToInt16(generator.Next(1, 19));
                }
                while (a_cBoard[m_16WallPosition[0, 0], m_16WallPosition[0, 1]] != '0' && a_cBoard[m_16WallPosition[0, 0] + 1, m_16WallPosition[0, 1]] != '0' && a_cBoard[m_16WallPosition[0, 0] + 2, m_16WallPosition[0, 1]] != '0' && a_cBoard[m_16WallPosition[0, 0] + 3, m_16WallPosition[0, 1]] != '0');
                a_cBoard[m_16WallPosition[0, 0], m_16WallPosition[0, 1]] = 'W';
                for (short i = 1; i < a_16SizeofWall; ++i)
                {
                    m_16WallPosition[i, 0] = Convert.ToInt16(m_16WallPosition[0, 0] + i);
                    m_16WallPosition[i, 1] = m_16WallPosition[0, 1];
                    a_cBoard[m_16WallPosition[i, 0], m_16WallPosition[i, 1]] = 'W';
                }
            }
        }

        public void Move()
        {

        }
    }
}
