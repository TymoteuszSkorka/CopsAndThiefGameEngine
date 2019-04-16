using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace main
{
    class Gate
    {
        public short[,] m_16GatePosition { get => m_16GatePosition; private set => m_16GatePosition = value; }
        private float m_fProbalityOfMovement;
        private float m_fProbalityOfChangingDirection;

        public Gate(short a_16SizeofGate = 2, float a_fProbalityOfMovement = 0.5f, float a_fProbalityOfChangingDirection = 0.01f)
        {
            m_16GatePosition = new short[a_16SizeofGate, 2];
            m_fProbalityOfMovement = a_fProbalityOfMovement;
            m_fProbalityOfChangingDirection = a_fProbalityOfChangingDirection;
        }

        public void Move()
        {

        }
    }
}
