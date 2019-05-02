using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace main
{
    class Gate
    {
        public char[,] m_cBoard;
        public short[,] m_16GatePosition;
        private float m_fProbalityOfMovement;
        private float m_fProbalityOfChangingDirection;
        private short[] m_16Direction;
        private bool[] m_bIsVertical;
        private Random generator;
        private Board m_board;

        public Gate(Board a_board, ref char[,] a_cBoard, ref Random a_generator, short a_16SizeofGate = 2, float a_fProbalityOfMovement = 0.75f, float a_fProbalityOfChangingDirection = 0.25f)
        {
            m_board = a_board;
            m_16Direction = new short[a_16SizeofGate];
            m_16Direction[0] = 1;
            m_16Direction[1] = 1;
            m_cBoard = a_cBoard;
            m_bIsVertical = new bool[a_16SizeofGate];
            m_16GatePosition = new short[a_16SizeofGate, 2];
            m_fProbalityOfMovement = a_fProbalityOfMovement;
            m_fProbalityOfChangingDirection = a_fProbalityOfChangingDirection;
            generator = a_generator;
            int num = generator.Next(0, 4);
            if (num == 0)
            {
                m_16GatePosition[0, 0] = 0;
                m_16GatePosition[0, 1] = Convert.ToInt16(generator.Next(1, 20));
                m_16GatePosition[1, 0] = 0;
                m_16GatePosition[1, 1] = Convert.ToInt16(m_16GatePosition[0, 1] + 1);
                m_bIsVertical[0] = true;
                m_bIsVertical[1] = true;
            }
            else if (num == 1)
            {
                m_16GatePosition[0, 1] = Convert.ToInt16(a_cBoard.GetLength(1) - 1);
                m_16GatePosition[0, 0] = Convert.ToInt16(generator.Next(1, 20));
                m_16GatePosition[1, 1] = Convert.ToInt16(a_cBoard.GetLength(1) - 1);
                m_16GatePosition[1, 0] = Convert.ToInt16(m_16GatePosition[0, 0] + 1);
                m_bIsVertical[0] = false;
                m_bIsVertical[1] = false;
            }
            else if (num == 2)
            {
                m_16GatePosition[0, 0] = Convert.ToInt16(a_cBoard.GetLength(0) - 1);
                m_16GatePosition[0, 1] = Convert.ToInt16(generator.Next(1, 20));
                m_16GatePosition[1, 0] = Convert.ToInt16(a_cBoard.GetLength(0) - 1);
                m_16GatePosition[1, 1] = Convert.ToInt16(m_16GatePosition[0, 1] + 1);
                m_bIsVertical[0] = true;
                m_bIsVertical[1] = true;
            }
            else if (num == 3)
            {
                m_16GatePosition[0, 1] = 0;
                m_16GatePosition[0, 0] = Convert.ToInt16(generator.Next(1, 20));
                m_16GatePosition[1, 1] = 0;
                m_16GatePosition[1, 0] = Convert.ToInt16(m_16GatePosition[0, 0] + 1);
                m_bIsVertical[0] = false;
                m_bIsVertical[1] = false;
            }
            a_cBoard[m_16GatePosition[0, 0], m_16GatePosition[0, 1]] = 'G';
            a_cBoard[m_16GatePosition[1, 0], m_16GatePosition[1, 1]] = 'G';
        }

        public void Move()
        {
            float tmp = generator.Next(1, 101) / 100f;
            if (tmp <= m_fProbalityOfChangingDirection)
            {
                for (int i = 0; i < m_16GatePosition.GetLength(0); ++i)
                {
                    m_16Direction[i] *= -1;
                }
            }
          
            float tmp1 = generator.Next(1, 101) / 100;
            if (tmp1 <= m_fProbalityOfMovement)
            {
                for (int i = 0; i < m_16GatePosition.GetLength(0); ++i)
                {
                    if (((m_16GatePosition[i, 1] == m_cBoard.GetLength(1) - 1 && m_16GatePosition[i, 0] == 0) 
                        || (m_16GatePosition[i, 0] == m_cBoard.GetLength(0) - 1 && m_16GatePosition[i, 1] == 0))
                        && tmp > m_fProbalityOfChangingDirection)
                    {
                        m_bIsVertical[i] = !m_bIsVertical[i];
                    }
                    if (((m_16GatePosition[i, 1] == 0 && m_16GatePosition[i, 0] == 0) 
                        || (m_16GatePosition[i, 0] == m_cBoard.GetLength(0) - 1 && m_16GatePosition[i, 1] == m_cBoard.GetLength(1) - 1))
                        && tmp > m_fProbalityOfChangingDirection )
                    {
                        m_bIsVertical[i] = !m_bIsVertical[i];
                        m_16Direction[i] *= -1;
                    }
                    m_16GatePosition[i, Convert.ToInt32(m_bIsVertical[i])] += m_16Direction[i];
                }
            }
            m_board.mapBoard();
        }
    }
}
