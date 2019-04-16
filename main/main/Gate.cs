using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace main
{
    class Gate
    {
        public short[,] m_16GatePosition;
        private float m_fProbalityOfMovement;
        private float m_fProbalityOfChangingDirection;

        public Gate(ref char[,] a_cBoard, short a_16SizeofGate = 2, float a_fProbalityOfMovement = 0.5f, float a_fProbalityOfChangingDirection = 0.01f)
        {
            m_16GatePosition = new short[a_16SizeofGate, 2];
            m_fProbalityOfMovement = a_fProbalityOfMovement;
            m_fProbalityOfChangingDirection = a_fProbalityOfChangingDirection;
            Random generator = new Random();
            int num = generator.Next(0, 4);
            if (num == 0)
            {
                m_16GatePosition[0, 0] = 0;
                m_16GatePosition[0, 1] = Convert.ToInt16(generator.Next(0, 21));
                m_16GatePosition[1, 0] = 0;
                m_16GatePosition[1, 1] = Convert.ToInt16(m_16GatePosition[0, 1] + 1);
            }
            else if (num == 1)
            {
                m_16GatePosition[0, 1] = Convert.ToInt16(a_cBoard.GetLength(1) - 1);
                m_16GatePosition[0, 0] = Convert.ToInt16(generator.Next(0, 21));
                m_16GatePosition[1, 1] = Convert.ToInt16(a_cBoard.GetLength(1) - 1);
                m_16GatePosition[1, 0] = Convert.ToInt16(m_16GatePosition[0, 0] + 1);
            }
            else if (num == 2)
            {
                m_16GatePosition[0, 0] = Convert.ToInt16(a_cBoard.GetLength(0) - 1);
                m_16GatePosition[0, 1] = Convert.ToInt16(generator.Next(0, 21));
                m_16GatePosition[1, 0] = Convert.ToInt16(a_cBoard.GetLength(0) - 1);
                m_16GatePosition[1, 1] = Convert.ToInt16(m_16GatePosition[0, 1] + 1);
            }
            else if (num == 3)
            {
                m_16GatePosition[0, 1] = 0;
                m_16GatePosition[0, 0] = Convert.ToInt16(generator.Next(0, 21));
                m_16GatePosition[1, 1] = 0;
                m_16GatePosition[1, 0] = Convert.ToInt16(m_16GatePosition[0, 0] + 1);
            }
            a_cBoard[m_16GatePosition[0, 0], m_16GatePosition[0, 1]] = 'G';
            a_cBoard[m_16GatePosition[1, 0], m_16GatePosition[1, 1]] = 'G';
        }

        public void Move()
        {

        }
    }
}
