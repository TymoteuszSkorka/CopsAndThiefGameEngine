using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace main
{
    class Wall
    {
        public char[,] m_cBoard;
        public short[,] m_16WallPosition;
        private float m_fProbalityOfMovement;
        private float m_fProbalityOfChangingDirection;
        private short m_16Direction;
        private bool m_bIsVertical;
        private Board m_board;
        private Random generator;

        public Wall(Board a_board,ref char[,] a_cBoard, ref Random a_generator, bool a_bIsVertical = true, short a_16SizeofWall = 4, float a_fProbalityOfMovement = 0.75f, float a_fProbalityOfChangingDirection = 0.05f)
        {
            m_board = a_board;
            m_16Direction = 1;
            m_bIsVertical = a_bIsVertical;
            m_cBoard = a_cBoard;
            m_16WallPosition = new short[a_16SizeofWall, 2];
            m_fProbalityOfMovement = a_fProbalityOfMovement;
            m_fProbalityOfChangingDirection = a_fProbalityOfChangingDirection;
            generator = a_generator;

            if (a_bIsVertical)
            {
                do
                {
                    m_16WallPosition[0, 0] = Convert.ToInt16(generator.Next(1, 18));
                    m_16WallPosition[0, 1] = Convert.ToInt16(generator.Next(1, 18));
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
                    m_16WallPosition[0, 0] = Convert.ToInt16(generator.Next(1, 18));
                    m_16WallPosition[0, 1] = Convert.ToInt16(generator.Next(1, 18));
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
            float tmp = generator.Next(1, 101) / 100f;
            bool flag = false;
            if (tmp <= m_fProbalityOfChangingDirection
                || (m_bIsVertical && m_16Direction == 1 && m_16WallPosition[m_16WallPosition.GetLength(0) - 1, 1] == m_cBoard.GetLength(1) - 2)
                || (m_bIsVertical && m_16Direction == -1 && m_16WallPosition[0, 1] == 1)
                || (!m_bIsVertical && m_16Direction == -1 && m_16WallPosition[0, 0] == 1)
                || (!m_bIsVertical && m_16Direction == 1 && m_16WallPosition[m_16WallPosition.GetLength(0) - 1, 0] == m_cBoard.GetLength(0) - 2))
            {
                m_16Direction *= -1;
            }
            if (m_bIsVertical && m_16Direction == 1 && m_16WallPosition[m_16WallPosition.GetLength(0) - 1, 1] < m_cBoard.GetLength(1) - 2 && (m_cBoard[m_16WallPosition[m_16WallPosition.GetLength(0) - 1, 0], m_16WallPosition[m_16WallPosition.GetLength(0) - 1, 1] + 1] == '0' || m_cBoard[m_16WallPosition[m_16WallPosition.GetLength(0) - 1, 0], m_16WallPosition[m_16WallPosition.GetLength(0) - 1, 1] + 1] == 'W'))
            {
                flag = true;
            }
            else if (m_bIsVertical && m_16Direction == -1 && m_16WallPosition[0, 1] > 1 && (m_cBoard[m_16WallPosition[0, 0], m_16WallPosition[0, 1] - 1] == '0' || m_cBoard[m_16WallPosition[0, 0], m_16WallPosition[0, 1] - 1] == 'W'))
            {
                flag = true;
            }
            else if (!m_bIsVertical && m_16Direction == -1 && m_16WallPosition[0, 0] > 1 && (m_cBoard[m_16WallPosition[0, 0] - 1, m_16WallPosition[0, 1]] == '0' || m_cBoard[m_16WallPosition[0, 0] - 1, m_16WallPosition[0, 1]] == 'W'))
            {
                flag = true;
            }
            else if (!m_bIsVertical && m_16Direction == 1 && m_16WallPosition[m_16WallPosition.GetLength(0) - 1, 0] < m_cBoard.GetLength(0) - 2 && (m_cBoard[m_16WallPosition[m_16WallPosition.GetLength(0) - 1, 0] + 1, m_16WallPosition[m_16WallPosition.GetLength(0) - 1, 1]] == '0' || m_cBoard[m_16WallPosition[m_16WallPosition.GetLength(0) - 1, 0] + 1, m_16WallPosition[m_16WallPosition.GetLength(0) - 1, 1]] == 'W'))
            {
                flag = true;
            }
            tmp = generator.Next(1, 101) / 100;
            if (flag && tmp <= m_fProbalityOfMovement)
            {
                for (int i = 0; i < m_16WallPosition.GetLength(0); ++i)
                {
                    m_16WallPosition[i, Convert.ToInt32(m_bIsVertical)] += m_16Direction;
                }
            }
            m_board.mapBoard();
        }
    }
}
